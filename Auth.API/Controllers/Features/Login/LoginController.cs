using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Auth.Services;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Auth.Models;
using Auth.Common;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private IAuthenticationService _authenticationService;
        private ILoggingService _loggingService;
        private ITokensService _tokensService;

        public LoginController(IAuthenticationService authenticationService, ILoggingService loggingService, ITokensService tokensService)
        {
            _authenticationService = authenticationService;
            _loggingService = loggingService;
            _tokensService = tokensService;
        }

        [HttpGet]
        public async Task<IActionResult> GetToken(string refreshToken)
        {
            SecurityResponse response = new SecurityResponse();
            try
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    FullToken fullToken = await _tokensService.GetByRefreshAsync(refreshToken);
                    if (fullToken != null && !string.IsNullOrEmpty(fullToken.RefreshToken))
                    {
                        response.Content = fullToken.AccessToken;
                        response.RefreshToken = fullToken.RefreshToken;
                        response.ExpiresOn = fullToken.ExpiresOn;

                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.Add(new Models.AppException
                {
                    CreatedBy = new Guid(User.Identity.Name),
                    CreatedOn = DateTime.Now,
                    Severity = (int)Severity.Critical,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });

                response.ResponseCode = StatusCodes.Status500InternalServerError;
                return Ok(response);
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            SecurityResponse response = new SecurityResponse();

            if (string.IsNullOrWhiteSpace(request.AccountId) || string.IsNullOrWhiteSpace(request.AppToken))
                return Unauthorized();

            try
            {
                string id = await _authenticationService.LoginAsync(request.AccountId, request.AppToken);
                if (string.IsNullOrEmpty(id))
                    return Unauthorized();

                response.Content = _tokensService.GetJwtToken(id);
                var fullToken = await _tokensService.GetRefreshByProfileIdAsync(new Guid(id));
                if (fullToken != null)
                {
                    response.RefreshToken = fullToken.RefreshToken;
                    response.ExpiresOn = fullToken.ExpiresOn;
                }
                else
                    return Unauthorized();

                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.Add(new Models.AppException
                {
                    CreatedBy = new Guid(User.Identity.Name),
                    CreatedOn = DateTime.Now,
                    Severity = (int)Severity.Critical,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });

                response.ResponseCode = StatusCodes.Status500InternalServerError;
                return Ok(response);
            }
        }
    }
}