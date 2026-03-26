using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MINT.EShop.API.Middlewares;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Business.Services;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using MINT.EShop.API.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options => {
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireClaim("Admin", "true");
    })
    .AddPolicy("ManagerPolicy", policy =>
    {
        policy.RequireClaim("Manager", "true");
    })
    .AddPolicy("ClientPolicy", policy =>
    {
        policy.RequireClaim("Client", "true");
    });

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStatusCodePages(async context =>
{
    context.HttpContext.Response.ContentType = "application/json";

    var statusCode = context.HttpContext.Response.StatusCode;

    string message = ReasonPhrases.GetReasonPhrase(statusCode);

    var response = APIResponse.FailureResponse(message);

    await context.HttpContext.Response.WriteAsJsonAsync(response);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();