using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N5.Infrastructure.Data;
using N5.Infrastructure.IoC;
using Serilog;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

var basicLoggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console();

var loggerConfiguration = builder.Environment.IsDevelopment()
    ? basicLoggerConfiguration
        .MinimumLevel.Warning()
    : basicLoggerConfiguration
        .MinimumLevel.Warning()
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Error);

builder.Host.UseSerilog(loggerConfiguration.CreateLogger());

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_AllowWebAppOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_AllowWebAppProdOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDependencyInjection(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    await dbContext!.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "N5 v1");
    });
}

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseCors(app.Environment.IsProduction() ? "_AllowWebAppProdOrigin" : "_AllowWebAppOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
