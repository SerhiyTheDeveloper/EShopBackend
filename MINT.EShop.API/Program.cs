using MINT.EShop.API.Extensions;
using MINT.EShop.API.Middlewares;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Business.Services;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Core.Options;
using MINT.EShop.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AdminOptions>(builder.Configuration.GetSection("AdminSetup"));
builder.Services.Configure<ManagerOptions>(builder.Configuration.GetSection("ManagerSetup"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailSettings"));

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
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProducerService, ProducerService>();

builder.Services.AddRedisExtension(builder.Configuration);
builder.Services.AddControllersExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenExtension();

builder.Services.AddAuthenticationExtension(builder.Configuration);

builder.Services.AddAuthorizationBuilderExtension();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowBlazor", policy =>
        policy.WithOrigins("https://localhost:7136")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

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

app.UseCors("AllowBlazor");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();