using UniversityApp.BLL.Interfaces;
using UniversityApp.BLL.Services;
using UniversityApp.Components;
using UniversityApp.DAL;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var config = builder.Configuration;

// ✅ Google Authentication + Cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "7253961132-ang2nom5hcjlk8eqomolb91sii4svudh.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
});

// ✅ Authorization services
builder.Services.AddAuthorization();

// ✅ AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// ✅ Register BLL services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();

// ✅ Add Razor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Google Login/Logout Middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ✅ Google Login Endpoint
app.MapGet("/login", async context =>
{
    var props = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
    {
        RedirectUri = "/"
    };
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
});

// ✅ Google Logout Endpoint
app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.Run();
