using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Infrastructure.Persistence;
using BaseCleanArchitecture.Infrastructure.Repositories;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Application.Mappings;
using BaseCleanArchitecture.Application.DTOs;
using MediatR;
using System.Reflection;
using Microsoft.OpenApi.Models;
using BaseCleanArchitecture.Application;
using BaseCleanArchitecture.Infrastructure;
using BaseCleanArchitecture.Infrastructure.Services;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.RateLimiting;
using BaseCleanArchitecture.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Verificar configuraciones
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var rateLimiting = builder.Configuration.GetSection("RateLimiting");

Console.WriteLine("=== Configuraciones ===");
Console.WriteLine($"Connection String: {connectionString}");
Console.WriteLine($"Rate Limit Permit: {rateLimiting["PermitLimit"]}");
Console.WriteLine($"Rate Limit Window: {rateLimiting["Window"]}");
Console.WriteLine($"Rate Limit Queue: {rateLimiting["QueueLimit"]}");
Console.WriteLine("=====================");

// Configurar límites de Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50MB
});

// ----------- SECCIN DE CONFIGURACIN DE SERVICIOS -------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar CORS según el entorno
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTestFrontend", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Configuración para desarrollo
            policy.WithOrigins("http://localhost:8000", "http://127.0.0.1:8000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            // Configuración para producción (IIS) - Leer desde appsettings.json
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[]
            {
                "https://iespro.com",
                "https://www.iespro.com",
                "https://api.iespro.com",
                "https://app.iespro.com"
            };
            
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "IESPRO Taller API", 
        Version = "v1",
        Description = "CRM Taller",
        Contact = new OpenApiContact
        {
            Name = "IESPRO",
            Email = "admin@iespro.com"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Configurar JWT Authentication en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Agregar tags para organizar los endpoints
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((docName, apiDesc) => true);
});

builder.Services.AddHttpContextAccessor();

// Configurar JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// Configurar Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    var rateLimitSettings = builder.Configuration.GetSection("RateLimiting");
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = int.Parse(rateLimitSettings["PermitLimit"]),
                Window = TimeSpan.FromMinutes(int.Parse(rateLimitSettings["Window"])),
                QueueLimit = int.Parse(rateLimitSettings["QueueLimit"])
            }));
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
    options.UseMySql(connectionString, serverVersion);
});

// ----------- TERMINA CONFIGURACIN DE SERVICIOS -------------

// Application Services
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<ISucursalRepository, SucursalRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ITipoUnidadRepository, TipoUnidadRepository>();
builder.Services.AddScoped<IAlmacenRepository, AlmacenRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IModeloRepository, ModeloRepository>();
builder.Services.AddScoped<ITipoCombustibleRepository, TipoCombustibleRepository>();
builder.Services.AddScoped<IPuestoRepository, PuestoRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IArticuloRepository, ArticuloRepository>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<INotificacionPlantillaRepository, NotificacionPlantillaRepository>();
builder.Services.AddScoped<IMailService, SmtpMailService>();
builder.Services.AddScoped<IUserContext, UserContextService>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(BaseCleanArchitecture.Application.Mappings.UsuarioMappingProfile));

// Configurar MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(BaseCleanArchitecture.Application.Features.Usuarios.Commands.CreateUsuario.CreateUsuarioCommand).Assembly);
});

// Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Configuración de Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog();

// Configuración de Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.WithProperty("ApplicationName", "IESPRO-Taller")
    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName));

var app = builder.Build(); // Aqu se construye la aplicacin (ya no modificar servicios despus de esta lnea)

// SEED DINÁMICO DE DATOS (solo una vez)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        // Verificar si las tablas existen antes de intentar acceder
        if (db.Database.CanConnect())
        {
            // Verificar si ya se ejecutó el seed
            // if (!db.Usuarios.Any())
            // {
            //     BaseCleanArchitecture.Infrastructure.Persistence.Configurations.SeedData.SeedOnStartup(db);
            // }
        }
    }
    catch (Exception ex)
    {
        // Log del error pero no fallar la aplicación
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "No se pudo ejecutar el seed dinámico. Asegúrate de ejecutar las migraciones primero.");
    }
}

// ----------- SECCIN DE MIDDLEWARE Y PIPELINE -------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Taller");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger se cargue en la raíz
        
        // Configurar para persistir el token automáticamente
        c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
        
        // Configurar para mostrar el botón Authorize
        c.DisplayRequestDuration();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowTestFrontend");

// Habilitar Authentication y Authorization
app.UseAuthentication();
app.UseAuthorization();

// Configuración de Rate Limiting en el pipeline
app.UseRateLimiter();

// Agregar middleware de seguridad
app.UseMiddleware<SecurityMiddleware>();

app.MapControllers();

app.Run();
