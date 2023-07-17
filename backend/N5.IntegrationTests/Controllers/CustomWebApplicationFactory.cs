using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using N5.Infrastructure.Data;

namespace N5.IntegrationTests.Controllers;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(dbContextDescriptor);

            // Use in-memory database for SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connection = new InMemoryDatabaseRoot();
                InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(options, databaseName: "TestDb2", connection);
            });
        });

        builder.UseEnvironment("Development");
    }
}