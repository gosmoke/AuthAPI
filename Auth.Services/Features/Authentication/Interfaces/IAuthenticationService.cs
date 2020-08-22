using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(string accountId, string appToken);
    }
}
