using Auth.Common;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services
{
    public interface ILoggingService
    {
        Task<List<AppException>> GetAllAsync(Severity severity);
        Task Add(AppException exception);
    }
}
