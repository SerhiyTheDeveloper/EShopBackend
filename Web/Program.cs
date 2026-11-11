using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.Components;
using Web.Extensions;
using Web.Middleware;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("JwtSettings"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UiStateService>();
builder.Services.AddScoped<TokenStorageService>();
builder.Services.AddScoped<CookieAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CookieAuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();

Action<HttpClient> configureApiClient = client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]!);

    client.Timeout = TimeSpan.FromSeconds(10);

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
};

builder.Services.AddHttpClient<AuthApiClient>(configureApiClient);
builder.Services.AddHttpClient<UserApiClient>(configureApiClient);
builder.Services.AddHttpClient<AdminApiClient>(configureApiClient);
builder.Services.AddHttpClient<CatalogApiClient>(configureApiClient);
builder.Services.AddHttpClient<ManagerApiClient>(configureApiClient);
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<AdminUsersService>();
builder.Services.AddScoped<ManagerService>();
builder.Services.AddScoped<TokenRefreshService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseMiddleware<TokenRefreshMiddleware>();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapAdminEndpoints();
app.MapManagerEndpoints();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
