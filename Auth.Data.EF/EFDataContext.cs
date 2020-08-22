using Auth.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Data.EF
{
    public class EFDataContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<AppException> Exceptions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AuthData;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
