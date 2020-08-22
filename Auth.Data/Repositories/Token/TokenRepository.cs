using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Auth.Entities;
using ContextLifeManager;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Repositories
{
    public class TokenRepository : BaseRepository, ITokenRepository
    {
        public TokenRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public IQueryable<Token> GetByProfileId(Guid profileId, bool withTracking = false)
        {
            if (!withTracking)
                return _context.Tokens.AsNoTracking().Where(a => a.ProfileId == profileId && a.IsEnabled);

            return _context.Tokens.Where(a => a.ProfileId == profileId && a.IsEnabled);
        }

        public async Task<Token> GetByRefreshAsync(Guid refreshToken, bool withTracking = true)
        {
            if (!withTracking)
                return await _context.Tokens.AsNoTracking().SingleOrDefaultAsync(a => a.RefreshToken == refreshToken && a.IsEnabled);

            return await _context.Tokens.SingleOrDefaultAsync(a => a.RefreshToken == refreshToken && a.IsEnabled);
        }
    }
}
