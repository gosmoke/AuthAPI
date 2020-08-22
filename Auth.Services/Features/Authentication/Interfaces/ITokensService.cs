using Auth.Models;
using System;
using System.Threading.Tasks;

namespace Auth.Services
{
    public interface ITokensService
    {
        Task<FullToken> GetByRefreshAsync(string refreshToken);
        Task<FullToken> GetRefreshByProfileIdAsync(Guid profileId);
        string GetJwtToken(string id);
    }
}
