using eLearning.Models;
using eLearning.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<EmailService>();

            builder.Services.AddIdentity<IdentityUserModel, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;

                options.User.RequireUniqueEmail = true;
            });

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

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
