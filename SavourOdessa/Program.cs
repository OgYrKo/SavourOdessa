using DataLayer.EfClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SavourOdessa.Factory;
using SavourOdessa.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
        opts =>
        {
            opts.IdleTimeout = TimeSpan.FromMinutes(30);
            opts.Cookie.IsEssential = true;
        }
    ) ;
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDataContextFactory, DataContextFactory>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<ConnectionStringMiddleware>();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapBlazorHub();
app.Run();
