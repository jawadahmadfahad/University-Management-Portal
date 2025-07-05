using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.OpenApi.Models; // ✅ Needed for Swagger
using UniversityApp.BLL.Interfaces;
using UniversityApp.BLL.Services;
using UniversityApp.BLL.Services.Api;
using UniversityApp.Components;
using UniversityApp.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<CourseApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7099/"); // ✅ change if port changes
});

// Add services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// ✅ Register BLL services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();

// ✅ Razor Components
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// ✅ API Controllers + Swagger
builder.Services.AddControllers(); // ← Needed to enable Web API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "University API", Version = "v1" });
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// ✅ Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "University API V1");
});

// ✅ Map controllers
app.MapControllers();

// ✅ Map Blazor app
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// ✅ Google login/logout endpoints (optional)
app.MapGet("/login", async context =>
{
    var props = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
    {
        RedirectUri = "/"
    };
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
});

app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.Run();
