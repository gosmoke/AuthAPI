using Auth.Data.EF;
using ContextLifeManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Data.Repositories
{
    public class BaseRepository
    {
        protected IAmbientDbContextLocator _ambientDbContextLocator;

        protected EFDataContext _context
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<EFDataContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext found. This means that this repository method has been called outside of the scope of a DbContextScope. A repository must only be accessed within the scope of a DbContextScope, which takes care of creating the DbContext instances that the repositories need and making them available as ambient contexts. This is what ensures that, for any given DbContext-derived type, the same instance is used throughout the duration of a business transaction. To fix this issue, use IDbContextScopeFactory in your top-level business logic service method to create a DbContextScope that wraps the entire business transaction that your service method implements. Then access this repository within that scope. Refer to the comments in the IDbContextScope.cs file for more details.");

                return dbContext;
            }
        }

        public void Save<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        /// <summary>
        /// Save or Update.  Predicate required.
        /// </summary>
        /// <typeparam name="T">Type of entity to save.</typeparam>
        /// <param name="entity">Entity to save.</param>
        /// <param name="predicate">Ex:  //Expression<Func<Token, bool>> testForEquality = (x) => x.Id == token.Id;</param>
        /// <returns></returns>
        public async Task SaveAsync<T>(T entity, Expression<Func<T, bool>> predicate) where T : class
        {
            if (await _context.Set<T>().AnyAsync(predicate))
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Set<T>().Add(entity);
            }
        }

    }
}
