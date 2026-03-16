using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MINT.EShop.API.Middlewares;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Business.Services;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture;
using MINT.EShop.Infrastracture.Repositories;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options => {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var response = APIResponse<object>.FailureResponse("Validation failed", errors);

            return new BadRequestObjectResult(response);
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SupportNonNullableReferenceTypes();

    var apiXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiXml));

    var businessXml = "MINT.EShop.Business.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, businessXml));
});


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();