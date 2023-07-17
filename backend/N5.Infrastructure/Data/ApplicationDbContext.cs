using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using N5.Core.Domain.Entities;

namespace N5.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PermissionType>().HasData(InitialPermissionTypes);
        }

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            context.PermissionTypes.AddRange(InitialPermissionTypes);
            await context.SaveChangesAsync();
        }

        private static readonly PermissionType[] InitialPermissionTypes =
        {
            new()
            {
                Id = 1,
                Description = "Tipo 1"
            },
            new()
            {
                Id = 2,
                Description = "Tipo 2"
            }
        };
    }
}
