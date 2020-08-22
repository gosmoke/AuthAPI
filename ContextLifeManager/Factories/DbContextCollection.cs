﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContextLifeManager
{
    public class DbContextCollection : IDbContextCollection
    {
        private Dictionary<Type, DbContext> _initializedDbContexts;
        private Dictionary<DbContext, IDbContextTransaction> _transactions;
        private IsolationLevel? _isolationLevel;
        private readonly IDbContextFactory _dbContextFactory;
        private bool _disposed;
        private bool _completed;
        private bool _readOnly;

        internal Dictionary<Type, DbContext> InitializedDbContexts { get { return _initializedDbContexts; } }

        public DbContextCollection(bool readOnly = false, IsolationLevel? isolationLevel = null, IDbContextFactory dbContextFactory = null)
        {
            _disposed = false;
            _completed = false;

            _initializedDbContexts = new Dictionary<Type, DbContext>();
            _transactions = new Dictionary<DbContext, IDbContextTransaction>();

            _readOnly = readOnly;
            _isolationLevel = isolationLevel;
            _dbContextFactory = dbContextFactory;
        }

        public TDbContext Get<TDbContext>() where TDbContext : DbContext
        {
            if (_disposed)
                throw new ObjectDisposedException("DbContextCollection");

            var requestedType = typeof(TDbContext);

            if (!_initializedDbContexts.ContainsKey(requestedType))
            {
                // First time we've been asked for this particular DbContext type.
                // Create one, cache it and start its database transaction if needed.
                var dbContext = _dbContextFactory != null
                    ? _dbContextFactory.CreateDbContext<TDbContext>()
                    : Activator.CreateInstance<TDbContext>();

                _initializedDbContexts.Add(requestedType, dbContext);

                if (_isolationLevel.HasValue)
                {
                    var tran = dbContext.Database.BeginTransaction();
                    _transactions.Add(dbContext, tran);
                }
            }

            return _initializedDbContexts[requestedType] as TDbContext;
        }
        public int Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException("DbContextCollection");
            if (_completed)
                throw new InvalidOperationException("You can't call Commit() or Rollback() more than once on a DbContextCollection. All the changes in the DbContext instances managed by this collection have already been saved or rollback and all database transactions have been completed and closed. If you wish to make more data changes, create a new DbContextCollection and make your changes there.");
                       
            ExceptionDispatchInfo lastError = null;

            var c = 0;

            foreach (var dbContext in _initializedDbContexts.Values)
            {
                try
                {
                    if (!_readOnly)
                    {
                        c += dbContext.SaveChanges();
                    }

                    // If we've started an explicit database transaction, time to commit it now.
                    var tran = GetValueOrDefault(_transactions, dbContext);
                    if (tran != null)
                    {
                        tran.Commit();
                        tran.Dispose();
                    }
                }
                catch (Exception e)
                {
                    lastError = ExceptionDispatchInfo.Capture(e);
                }
            }

            _transactions.Clear();
            _completed = true;

            if (lastError != null)
                lastError.Throw(); // Re-throw while maintaining the exception's original stack track

            return c;
        }

        public Task<int> CommitAsync()
        {
            return CommitAsync(CancellationToken.None);
        }

        public async Task<int> CommitAsync(CancellationToken cancelToken)
        {
            if (cancelToken == null)
                throw new ArgumentNullException("cancelToken");
            if (_disposed)
                throw new ObjectDisposedException("DbContextCollection");
            if (_completed)
                throw new InvalidOperationException("You can't call Commit() or Rollback() more than once on a DbContextCollection. All the changes in the DbContext instances managed by this collection have already been saved or rollback and all database transactions have been completed and closed. If you wish to make more data changes, create a new DbContextCollection and make your changes there.");

            ExceptionDispatchInfo lastError = null;

            var c = 0;

            foreach (var dbContext in _initializedDbContexts.Values)
            {
                try
                {
                    if (!_readOnly)
                    {
                        c += await dbContext.SaveChangesAsync(cancelToken).ConfigureAwait(false);
                    }

                    // If we've started an explicit database transaction, time to commit it now.
                    var tran = GetValueOrDefault(_transactions, dbContext);
                    if (tran != null)
                    {
                        tran.Commit();
                        tran.Dispose();
                    }
                }
                catch (Exception e)
                {
                    lastError = ExceptionDispatchInfo.Capture(e);
                }
            }

            _transactions.Clear();
            _completed = true;

            if (lastError != null)
                lastError.Throw(); // Re-throw while maintaining the exception's original stack track

            return c;
        }

        public void Rollback()
        {
            if (_disposed)
                throw new ObjectDisposedException("DbContextCollection");
            if (_completed)
                throw new InvalidOperationException("You can't call Commit() or Rollback() more than once on a DbContextCollection. All the changes in the DbContext instances managed by this collection have already been saved or rollback and all database transactions have been completed and closed. If you wish to make more data changes, create a new DbContextCollection and make your changes there.");

            ExceptionDispatchInfo lastError = null;

            foreach (var dbContext in _initializedDbContexts.Values)
            {
                // There's no need to explicitly rollback changes in a DbContext as
                // DbContext doesn't save any changes until its SaveChanges() method is called.
                // So "rolling back" for a DbContext simply means not calling its SaveChanges()
                // method. 

                // But if we've started an explicit database transaction, then we must roll it back.
                var tran = GetValueOrDefault(_transactions, dbContext);
                if (tran != null)
                {
                    try
                    {
                        tran.Rollback();
                        tran.Dispose();
                    }
                    catch (Exception e)
                    {
                        lastError = ExceptionDispatchInfo.Capture(e);
                    }
                }
            }

            _transactions.Clear();
            _completed = true;

            if (lastError != null)
                lastError.Throw(); // Re-throw while maintaining the exception's original stack track
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            // Do our best here to dispose as much as we can even if we get errors along the way.
            // Now is not the time to throw. Correctly implemented applications will have called
            // either Commit() or Rollback() first and would have got the error there.

            if (!_completed)
            {
                try
                {
                    if (_readOnly) Commit();
                    else Rollback();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }

            foreach (var dbContext in _initializedDbContexts.Values)
            {
                try
                {
                    dbContext.Dispose();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }

            _initializedDbContexts.Clear();
            _disposed = true;
        }

        /// <summary>
        /// Returns the value associated with the specified key or the default 
        /// value for the TValue  type.
        /// </summary>
        private static TValue GetValueOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }
    }
}
