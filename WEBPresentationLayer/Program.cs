
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using WEBPresentationLayer.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("CookieAuthentication")
       .AddCookie("CookieAuthentication", config =>
       {
           config.Cookie.Name = "UserLoginCookie";
           config.LoginPath = "/Login";
           config.AccessDeniedPath = "/Home/Index"; // Adicionar uma p�gina de n�o autorizado
       });
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<ChamadoController>();
builder.Services.AddHttpClient<ClienteController>();
builder.Services.AddHttpClient<LoginController>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();  

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
