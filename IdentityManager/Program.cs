using IdentityManager.Data;
using IdentityManager.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<IEmailService>();
        builder.Services
            .AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>(a =>
            {
                a.Password.RequiredLength = 5;
                a.Password.RequireLowercase = true;
                a.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                a.Lockout.MaxFailedAccessAttempts = 6;
                a.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthentication().AddFacebook(a =>
        {
            a.AppId = ""; //your appId from facebook
            a.AppSecret = ""; //your appSecret from facebook
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
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
    }
}