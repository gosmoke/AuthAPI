using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContextLifeManager
{
    public interface IDbContextCollection : IDisposable
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
