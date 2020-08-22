using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContextLifeManager
{
    public interface IAmbientDbContextLocator
    {
        /// <summary>
        /// If called within the scope of a DbContextScope, gets or creates 
        /// the ambient DbContext instance for the provided DbContext type. 
        /// 
        /// Otherwise returns null. 
        /// </summary>
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
