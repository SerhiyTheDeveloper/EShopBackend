
using MINT.EShop.Business;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Business.Services;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();



