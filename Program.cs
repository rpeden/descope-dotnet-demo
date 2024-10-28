using Descope;
using DescopeTestApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.HttpSys;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

var config = new DescopeConfig(
    projectId: builder.Configuration?["DescopeProjectId"] 
    ?? throw new Exception("Unable to load Descope project ID."));
var descopeClient = new DescopeClient(config);

builder.Services.AddSingleton(new DescopeClient(config));

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication().AddCookie();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
	options.LoginPath = "/Login";
	options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<BearerAuthenticationMiddleware>();

app.Run();
