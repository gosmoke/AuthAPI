using Auth.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Auth.Data.Repositories
{
    public interface ITokenRepository
    {
        Task<Token> GetByRefreshAsync(Guid refreshToken, bool withTracking = true);
        IQueryable<Token> GetByProfileId(Guid profileId, bool withTracking = false);
        void Save<T>(T entity) where T : class;
        Task SaveAsync<T>(T entity, Expression<Func<T, bool>> predicate) where T : class;
    }
}
