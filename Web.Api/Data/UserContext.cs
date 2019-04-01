using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Web.Api.Model;

namespace Web.Api.Data
{
    public class UserContext : DbContext
    {

        public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
        {
            private IConfiguration _configuration;

            public UserContextFactory()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                _configuration = builder.Build();
            }


            public UserContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

                return new UserContext(optionsBuilder.Options);
            }
        }


        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Email);
        }


        public DbSet<User> Users { get; set; }

        




    }


}
