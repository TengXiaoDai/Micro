using Micro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Micro.Respository
{
    public class MicroContext : DbContext
    {
        public MicroContext(DbContextOptions<MicroContext> options) :base(options) 
        {
          
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

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
            //打印Sql
            optionsBuilder.LogTo(logMessage =>
            {
                Debugger.Break();
                Debug.WriteLine(logMessage);
                Console.WriteLine(logMessage);
            },
       new[] {
          DbLoggerCategory.Database.Command.Name }, LogLevel.Information
   );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }

    public class StoreDbContexttFactory : IDesignTimeDbContextFactory<MicroContext>
    {

        public MicroContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MicroContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=Micro;Trusted_Connection=True;uid=sa;password=zxc520;TrustServerCertificate=true");
            return new MicroContext(optionsBuilder.Options);
        }
    }
}
