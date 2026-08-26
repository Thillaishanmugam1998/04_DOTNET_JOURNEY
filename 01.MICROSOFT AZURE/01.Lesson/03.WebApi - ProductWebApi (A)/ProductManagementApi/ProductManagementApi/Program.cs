using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.Repositories;
using ProductManagementApi.Services;
using Serilog;

namespace ProductManagementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Create and configure the Serilog logger by reading settings
            // from appsettings.json, appsettings.Development.json, or
            // appsettings.Production.json based on the current environment.
            // This allows us to manage log levels, file sinks, and other logging behavior
            // from configuration instead of hardcoding everything here.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            // Tell ASP.NET Core to use Serilog as the main logging provider
            // instead of the default built-in logging providers.
            builder.Host.UseSerilog();

            // Add controller support and keep JSON property names exactly
            // the same as the C# property names in our DTOs and models.
            builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
             });

            // Add Swagger services so API endpoints can be tested easily
            // during development.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register ApplicationDbContext and configure EF Core to use SQL Server.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repository and service classes in the dependency injection container.
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            // Enable Swagger UI only in Development environment.
            // This is useful for local testing but usually avoided in production.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Logs details about every HTTP request such as:
            // request path, method, status code, and execution time.
            // This helps a lot when troubleshooting production issues.
            app.UseSerilogRequestLogging();

            // Redirect all HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Adds authorization middleware to the request pipeline
            app.UseAuthorization();

            // Maps controller endpoints to incoming HTTP requests
            app.MapControllers();

            // Starts the application
            app.Run();
        }
    }
}