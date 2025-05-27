using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using FoodWebApp.Utility;
using Stripe;

namespace FoodWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC + API controllers
            builder.Services.AddControllersWithViews();  // includes MVC + Authorization

            // EF Core +  Repos/UoW
            builder.Services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Password hasher for User entity
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            //Stripe Configuration

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

            // JWT Authentication
            var jwt = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwt["Key"]);
            builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
                {
                opts.LoginPath = "/Account/Login";
                opts.LogoutPath = "/Account/Logout";
                opts.AccessDeniedPath = "/Account/AccessDenied";
                opts.ExpireTimeSpan = TimeSpan.FromHours(1);
                })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                       ValidateIssuer = true,
                       ValidIssuer = jwt["Issuer"],
                       ValidateAudience = true,
                       ValidAudience = jwt["Audience"],
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero
                   };
               });

            var app = builder.Build();


            // Middleware pipeline
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

            // Routing for Areas + default MVC
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Map attribute‐routed API controllers
            app.MapControllers();

            app.Run();
        }
    }
}
