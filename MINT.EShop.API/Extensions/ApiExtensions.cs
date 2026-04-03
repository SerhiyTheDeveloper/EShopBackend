using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MINT.EShop.API.Filters;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace MINT.EShop.API.Extensions
{
    public static class ApiExtensions
    {
        public static IHostBuilder UseSerilogExtension(this IHostBuilder host, IConfiguration configuration, DatabaseType dbType)
        {
            var logConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console()
                .Enrich.FromLogContext();

            if (dbType == DatabaseType.PostgreSQL)
            {
                logConfig.WriteTo.PostgreSQL(
                    connectionString: configuration.GetConnectionString("PostgreSQL"),
                    tableName: "Logs",
                    needAutoCreateTable: true);
            }

            Log.Logger = logConfig.CreateLogger();

            return host.UseSerilog();
        }
        public static IServiceCollection AddDbContextExtension(this IServiceCollection services, IConfiguration configuration, DatabaseType dbType)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                if (dbType == DatabaseType.PostgreSQL)
                {
                    var connectionString = configuration.GetConnectionString("PostgreSQL");
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    var name = configuration.GetConnectionString("InMemory")!;
                    options.UseInMemoryDatabase(name);
                }
            });
            return services;
        }
        public static IServiceCollection AddControllersExtension(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList();

                        var response = APIResponse.FailureResponse("Validation failed", errors);

                        return new BadRequestObjectResult(response);
                    };
                    options.SuppressMapClientErrors = true;
                });

            return services;
        }
        public static IServiceCollection AddSwaggerGenExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SupportNonNullableReferenceTypes();

                var apiXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiXml));

                var businessXml = "MINT.EShop.Business.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, businessXml));

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header,
                    Description = "Enter pure JwtToken below."
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }
        public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!)),
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }
        public static IServiceCollection AddAuthorizationBuilderExtension(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                })
                .AddPolicy("ManagerPolicy", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Manager");
                })
                .AddPolicy("ClientPolicy", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Client");
                });
            return services;
        }
        public static async Task UseDbInitializerExtension(this IHost app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var seeder = services.GetRequiredService<IDataSeeder>();

                if (context.Database.IsRelational())
                {
                    await context.Database.MigrateAsync();
                }

                await seeder.SeedAsync();
                logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Critical database initialization error.");
                throw;
            }
        }
        public static IApplicationBuilder UseStatusCodePagesExtension(this IApplicationBuilder app)
        {
            return app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                var statusCode = context.HttpContext.Response.StatusCode;

                string message = ReasonPhrases.GetReasonPhrase(statusCode);

                var response = APIResponse.FailureResponse(message);

                await context.HttpContext.Response.WriteAsJsonAsync(response);
            });
        }
    }
}
