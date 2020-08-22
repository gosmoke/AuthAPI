using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ContextLifeManager
{
    public class DbContextFactory : IDbContextFactory
    {
        public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            return Activator.CreateInstance(typeof(TDbContext), new object[] { }) as TDbContext;
        }
    }
}
