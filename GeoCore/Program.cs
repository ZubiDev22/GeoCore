using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GeoCore.Repositories;
using MediatR;
using Serilog;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using FluentValidation; // <-- necesario para AddValidatorsFromAssemblyContaining

// Elimina las instrucciones de nivel superior y encapsula en un método Main
namespace GeoCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de Serilog para Application Insights
            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.ApplicationInsights(
                    builder.Configuration["ApplicationInsights:ConnectionString"],
                    TelemetryConverter.Traces
                )
                .ReadFrom.Configuration(ctx.Configuration)
            );

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssemblyContaining<GeoCore.Validators.MaintenanceEventDtoValidator>();

            // Configuración de DbContext
            builder.Services.AddDbContext<GeoCore.Persistence.GeoCoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Cambia los repositorios Stub por los repositorios reales que usan Entity Framework
            builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
            builder.Services.AddScoped<IMaintenanceEventRepository, MaintenanceEventRepository>();
            builder.Services.AddScoped<ICashFlowRepository, CashFlowRepository>();
            builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();

            // Registro de MediatR
            builder.Services.AddMediatR(typeof(Program).Assembly);

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GeoCore API",
                    Version = "v1",
                    Description = "API para GeoCore"
                });
                // Agrupa y ordena los tags anteponiendo un número para el orden visual
                c.TagActionsBy(api =>
                {
                    var controller = api.GroupName ?? api.ActionDescriptor.RouteValues["controller"];
                    return controller switch
                    {
                        "Buildings" => new[] { "1. Buildings" },
                        "CashFlows" => new[] { "2. CashFlows" },
                        "MaintenanceEvents" => new[] { "3. MaintenanceEvents" },
                        "AssetAssessments" => new[] { "4. AssetAssessments" },
                        _ => new[] { controller }
                    };
                });
                c.DocInclusionPredicate((docName, apiDesc) => true);
            });

            // Configuración de CORS para permitir Angular en desarrollo
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder.WithOrigins("http://localhost:4200", "https://appgeocore.netlify.app")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoCore API v1");
                    // Elimina RoutePrefix para que Swagger esté en /swagger
                    // c.RoutePrefix = string.Empty; // Comentado para que Swagger esté en /swagger
                });
            }

            app.UseMiddleware<GeoCore.Middlewares.ExceptionLoggingMiddleware>();
            app.UseHttpsRedirection();
            // Usar la política CORS antes de Authorization y MapControllers
            //app.UseCors("AllowAngularDev");
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
