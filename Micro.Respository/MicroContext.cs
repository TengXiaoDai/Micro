using Micro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Micro.Respository
{
    public class MicroContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", true, true);
                var ConfigRoot = builder.Build();//根节点
                string connStr = ConfigRoot.GetSection("ConnectionStrings:DefaultConnection").Value;
                optionsBuilder.UseSqlServer(connStr);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
