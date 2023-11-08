using MvcAuth.CrossCutting.IoC;

namespace MvcAuth.Mvc;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddControllersWithViews();
        builder.Services.AddApplicationServices();
        builder.Services.AddCookieAuthentication();

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
        app.MapDefaultControllerRoute();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Privacy}/{id?}");

        await app.Services.AddAdminAsync();
    

        app.Run();
    }
}
