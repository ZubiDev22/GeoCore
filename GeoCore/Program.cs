using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GeoCore.Repositories;
using FluentValidation.AspNetCore;
using GeoCore.Logging;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<GeoCore.Validators.MaintenanceEventDtoValidator>();
});

// Configuración de DbContext
builder.Services.AddDbContext<GeoCore.Persistence.GeoCoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorios simulados como Singleton para mantener los datos en memoria
builder.Services.AddSingleton<IBuildingRepository, BuildingRepositoryStub>();
builder.Services.AddSingleton<ICashFlowRepository, CashFlowRepositoryStub>();
builder.Services.AddSingleton<IMaintenanceEventRepository, MaintenanceEventRepositoryStub>();
builder.Services.AddSingleton<IManagementBudgetRepository, ManagementBudgetRepositoryStub>();

// Registro de ILoguer y Loguer
builder.Services.AddScoped<ILoguer, Loguer>();

// Registro de MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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
app.UseAuthorization();
app.MapControllers();
app.Run();
