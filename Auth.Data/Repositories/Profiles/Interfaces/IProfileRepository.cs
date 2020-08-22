using Auth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Data.Repositories
{
    public interface IProfileRepository
    {
        IQueryable<Profile> GetAll(bool withTracking = false);
        Task<Profile> GetByIdAsync(Guid id, bool withTracking = true);
        void Save(Profile entity);
    }
}
