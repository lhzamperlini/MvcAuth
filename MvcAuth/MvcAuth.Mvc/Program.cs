using Microsoft.EntityFrameworkCore;
using MvcAuth.Application.Services;
using MvcAuth.Domain.Interfaces.Repositories;
using MvcAuth.Domain.Interfaces.Repositories.common;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Repository.Data;
using MvcAuth.Repository.Repositories;
using MvcAuth.Repository.Repositories.common;
using System.Reflection;
using System.Security.AccessControl;

namespace MvcAuth.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("MainDbConnection") ?? throw new InvalidOperationException("Connection string 'MainDbConnection' not found.");

        // Add services to the container.
        builder.Services.AddDbContext<MainDbContext>(options =>
           options.UseSqlServer(connectionString));

        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped(typeof(ICrudRepositoryBase<>), typeof(CrudRepositoryBase<>));

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
