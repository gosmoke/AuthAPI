using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Auth.Data.Repositories;
using ContextLifeManager;

namespace Auth.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public ILoggingRepository LoggingRepository { get; set; }
        public IProfileRepository ProfileRepository { get; set; }
        public ITokenRepository TokenRepository { get; set; }

        public UnitOfWork(IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        {
            _dbContextScopeFactory = dbContextScopeFactory;

            LoggingRepository = new LoggingRepository(ambientDbContextLocator);
            ProfileRepository = new ProfileRepository(ambientDbContextLocator);
            TokenRepository = new TokenRepository(ambientDbContextLocator);
        }

        public IDbContextScope Create(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting)
        {
            return _dbContextScopeFactory.Create(joiningOption);
        }

        public IDbContextReadOnlyScope CreateReadOnly(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting)
        {
            return _dbContextScopeFactory.CreateReadOnly(joiningOption);
        }

        public IDbContextScope CreateWithTransaction(IsolationLevel isolationLevel)
        {
            return _dbContextScopeFactory.CreateWithTransaction(isolationLevel);
        }

        public IDbContextReadOnlyScope CreateReadOnlyWithTransaction(IsolationLevel isolationLevel)
        {
            return _dbContextScopeFactory.CreateReadOnlyWithTransaction(isolationLevel);
        }

        public IDisposable SuppressAmbientContext()
        {
            return _dbContextScopeFactory.SuppressAmbientContext();
        }
    }
}
