using Auth.Data.Repositories;
using ContextLifeManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Data
{
    public interface IUnitOfWork : IContextLifeManager
    {
        ILoggingRepository LoggingRepository { get; set; }
        IProfileRepository ProfileRepository { get; set; }
        ITokenRepository TokenRepository { get; set; }
    }
}
