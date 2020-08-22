using Auth.Data;
using Auth.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggingService _loggingService;

        public AuthenticationService(IUnitOfWork unitOfWork, ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _loggingService = loggingService;
        }

        public async Task<string> LoginAsync(string accountId, string appToken)
        {
            using (var scopeContext = _unitOfWork.Create())
            {
                Profile profile = await _unitOfWork.ProfileRepository.GetAll().SingleOrDefaultAsync(a => a.AccountId == accountId && a.Application.AppToken == appToken && a.Application.IsActive);
                if (profile != null)
                {
                    return profile.Id.ToString();
                }
            }

            return "";
        }
    }
}
