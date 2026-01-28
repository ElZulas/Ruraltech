using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RuralTech.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RuralTech API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
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
            Array.Empty<string>()
        }
    });
});

// Database - Supabase PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("your-project"))
{
    // Si no hay configuración de Supabase, usar SQLite temporalmente
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=ruraltech_temp.db"));
}
else
{
    // Usar Supabase PostgreSQL
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
        }));
}

// CORS
builder.Services.AddCors(options =>
{
    // Para desarrollo (web y dispositivos móviles)
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000", 
                "http://localhost:5173", 
                "https://localhost:3000",
                "http://10.234.89.228:3000",  // IP actual
                "http://10.3.0.235:3000",  // IP anterior (por si acaso)
                "http://192.168.1.64:3000" // IP de casa
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
    
    // Para producción (app móvil y web)
    // Permite todas las conexiones desde apps móviles (no tienen origen específico)
    options.AddPolicy("AllowMobileApp", policy =>
    {
        policy.AllowAnyOrigin()  // Apps móviles no tienen origen específico
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "RuralTechSecretKeyForJWTTokenGeneration2024";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "RuralTech";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "RuralTechUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar CORS según el entorno
// En desarrollo, permitir tanto web como apps móviles
if (app.Environment.IsDevelopment())
{
    // Permitir apps móviles también en desarrollo
    app.UseCors("AllowMobileApp");
}
else
{
    // En producción, permitir apps móviles
    app.UseCors("AllowMobileApp");
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created and seeded
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            // Intentar conectar a la base de datos
            logger.LogInformation("Intentando conectar a la base de datos...");
            var canConnect = db.Database.CanConnect();
            
            if (canConnect)
            {
                logger.LogInformation("Conexión a la base de datos exitosa");
                db.Database.EnsureCreated();
                logger.LogInformation("Database created/verified successfully");
                
                // Seed database with test data
                RuralTech.Infrastructure.Data.DatabaseSeeder.Seed(db);
                logger.LogInformation("Database seeded successfully");
            }
            else
            {
                logger.LogWarning("No se pudo conectar a la base de datos. Verifica la conexión a Supabase.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error conectando a la base de datos: {Message}", ex.Message);
            logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
            if (ex.InnerException != null)
            {
                logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
            }
            logger.LogWarning("API continuará ejecutándose pero las operaciones de base de datos pueden fallar.");
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Critical error during database initialization");
}

app.Run();
