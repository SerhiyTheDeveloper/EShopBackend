using MINT.EShop.API.Extensions;
using MINT.EShop.API.Middlewares;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Business.Services;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AdminOptions>(builder.Configuration.GetSection("AdminSetup"));

var dbType = (DatabaseType)builder.Configuration.GetValue<int>("DatabaseType");

builder.Host.UseSerilogExtension(builder.Configuration, dbType);

builder.Services.AddDbContextExtension(builder.Configuration, dbType);

builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddControllersExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenExtension();

builder.Services.AddAuthenticationExtension(builder.Configuration);

builder.Services.AddAuthorizationBuilderExtension();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

await app.UseDbInitializerExtension();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesExtension();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();