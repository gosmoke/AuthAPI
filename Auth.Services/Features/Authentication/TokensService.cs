using Auth.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Models;
using Auth.Entities;
using Auth.Common;
using Auth.Common.Constants;

namespace Auth.Services
{
    public class TokensService : ITokensService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TokensService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<FullToken> GetByRefreshAsync(string refreshToken)
        {
            FullToken fullToken = new FullToken();

            using (var contextScope = _unitOfWork.Create())
            {
                Guid decodedToken = new Guid(EncodeHelper.Base64Decode(refreshToken));
                Token token = await _unitOfWork.TokenRepository.GetByRefreshAsync(decodedToken);
                if (token != null)
                {
                    var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(token.ProfileId);
                    if (profile != null)
                    {
                        string jwtToken = GetJwtToken(profile.Id.ToString());

                        token.RefreshToken = Guid.NewGuid();
                        token.LastUpdatedOn = DateTime.Now;

                        fullToken = new FullToken { AccessToken = jwtToken, RefreshToken = EncodeHelper.Base64Encode(token.RefreshToken.ToString()), ExpiresOn = token.ExpiresOn };

                        Expression<Func<Token, bool>> testForEquality = (x) => x.Id == token.Id;
                        await _unitOfWork.TokenRepository.SaveAsync(token, testForEquality);

                        await contextScope.SaveChangesAsync();
                    }
                }
            }

            return fullToken;
        }

        /// <summary>
        /// This should only be called after a successful login.  If getting a refresh token, use GetByRefreshAsync.
        /// </summary>
        /// <param name="profileId">Unique identifier associated with the user.</param>
        /// <returns>Returns a refresh token.</returns>
        public async Task<FullToken> GetRefreshByProfileIdAsync(Guid profileId)
        {
            using (var contextScope = _unitOfWork.Create())
            {
                var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(profileId);
                if (profile != null)
                {
                    // disable previous refresh tokens assigned for this profileId.
                    List<Token> previousTokens = await _unitOfWork.TokenRepository.GetByProfileId(profileId, true).ToListAsync();
                    previousTokens.ForEach(a => a.IsEnabled = false);

                    Token token = new Token
                    {
                        IsEnabled = true,
                        ProfileId = profileId,
                        RefreshToken = Guid.NewGuid(),
                        LastUpdatedOn = DateTime.Now,
                        ExpiresOn = DateTime.Now.Date.AddDays(Configuration.Tokens.Refresh.EXPIRES_IN_DAYS)
                    };

                    _unitOfWork.TokenRepository.Save(token);

                    await contextScope.SaveChangesAsync();

                    FullToken fullToken = new FullToken
                    {
                        RefreshToken = EncodeHelper.Base64Encode(token.RefreshToken.ToString()),
                        ExpiresOn = token.ExpiresOn
                    };

                    return fullToken;
                }

                return null;
            }
        }

        public string GetJwtToken(string id)
        {
            return new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(Configuration.KEY))
                .AddSubject(Configuration.Tokens.NAME)
                .AddIssuer(Configuration.Tokens.ISSUER)
                .AddAudience(Configuration.Tokens.AUDIENCE)
                .AddClaim(ClaimTypes.NameIdentifier, id)
                .Build().Value;
        }
    }
}
