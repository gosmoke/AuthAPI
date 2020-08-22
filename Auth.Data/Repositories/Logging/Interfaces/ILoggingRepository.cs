using Auth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Data.Repositories
{
    public interface ILoggingRepository
    {
        IQueryable<AppException> GetAll(bool withTracking = false);
        void Save(AppException entity);
    }
}
