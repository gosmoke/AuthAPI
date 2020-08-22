using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.Data.EF;
using Auth.Entities;
using ContextLifeManager;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Repositories
{

    public class ProfileRepository : BaseRepository, IProfileRepository
    {
        public ProfileRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public IQueryable<Profile> GetAll(bool withTracking = false)
        {
            var query = _context.Profiles.Include(a => a.Application).Include(a => a.UserDetail);

            if (!withTracking)
                return query.AsNoTracking();

            return query;
        }

        public async Task<Profile> GetByIdAsync(Guid id, bool withTracking = true)
        {
            var query = _context.Profiles.Include(a => a.Application).Include(a => a.UserDetail);

            if (!withTracking)
                return await query.AsNoTracking().SingleOrDefaultAsync(a => a.Id == id);

            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }

        public void Save(Profile entity)
        {
            var id = entity.Id;
            if (_context.Profiles.Any(a => a.Id == id))
            {
                _context.Profiles.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Add(entity);
            }
        }
    }
}
