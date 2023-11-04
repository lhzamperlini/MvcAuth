using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MvcAuth.Application.Services;
using MvcAuth.Domain.Enums;
using MvcAuth.Domain.Interfaces.Repositories;
using MvcAuth.Domain.Interfaces.Repositories.common;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Domain.Models;
using MvcAuth.Repository.Data;
using MvcAuth.Repository.Repositories;
using MvcAuth.Repository.Repositories.common;
using System.Reflection;
using System.Security.AccessControl;

namespace MvcAuth.Mvc;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("MainDbConnection") ?? throw new InvalidOperationException("Connection string 'MainDbConnection' not found.");

        // Add services to the container.
        builder.Services.AddDbContext<MainDbContext>(options =>
           options.UseSqlServer(connectionString));

        builder.Services.AddControllersWithViews();

        builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                });

        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped(typeof(ICrudRepositoryBase<>), typeof(CrudRepositoryBase<>));

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


        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<IUsuarioService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();

            var usuario = new Usuario
            {
                Nome = "Luis",
                Sobrenome = "Henrique",
                Email = "lhzamperlini@gmail.com",
                Senha = "lhzamperlini",
                Ativo = true,
                Confirmado = true,
                TipoUsuario = TipoUsuario.Administrador
            };

            var usuarioExiste = await userRepository.VerificarExistente(usuario.Email);

            if (!usuarioExiste)
            {
                await userManager.Cadastrar(usuario);
            }
        }

        app.Run();
    }
}
