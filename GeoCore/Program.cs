using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GeoCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de DbContext
builder.Services.AddDbContext<GeoCore.Persistence.GeoCoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorios simulados (puedes implementar la lógica real luego)
builder.Services.AddScoped<IBuildingRepository, BuildingRepositoryStub>();
builder.Services.AddScoped<ICashFlowRepository, CashFlowRepositoryStub>();
builder.Services.AddScoped<IMaintenanceEventRepository, MaintenanceEventRepositoryStub>();
builder.Services.AddScoped<IAssetAssessmentRepository, AssetAssessmentRepositoryStub>();

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
    // Configuración adicional para asegurar la versión
    c.DocInclusionPredicate((docName, apiDesc) => true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
