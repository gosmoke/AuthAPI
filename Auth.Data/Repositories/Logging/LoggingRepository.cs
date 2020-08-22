using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auth.Entities;
using ContextLifeManager;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Repositories
{
    public class LoggingRepository : BaseRepository, ILoggingRepository
    {
        public LoggingRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public IQueryable<AppException> GetAll(bool withTracking = false)
        {
            if (!withTracking)
                return _context.Exceptions.AsNoTracking();

            return _context.Exceptions;
        }

        public void Save(AppException entity)
        {
            var id = entity.Id;
            if (_context.Exceptions.Any(a => a.Id == id))
            {
                _context.Exceptions.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                Save(entity);
            }
        }
    }
}
