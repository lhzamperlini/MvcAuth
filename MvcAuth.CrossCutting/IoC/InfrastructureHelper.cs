using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcAuth.Application.Extensions;
using MvcAuth.Application.Services;
using MvcAuth.Domain.Enums;
using MvcAuth.Domain.Interfaces.Repositories;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Domain.Models;
using MvcAuth.Repository.Data;
using MvcAuth.Repository.Repositories.common;
using System.Reflection;

namespace MvcAuth.CrossCutting.IoC;
public static class InfrastructureHelper
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        /* Adicionar esse primeiro método faz com que todo o repositorie que herdar de RepositoryBase
        seja adicionado dinamicamente */
     
        services.AddScopedByBaseType(typeof(RepositoryBase))
                .AddScoped<IUsuarioService, UsuarioService>()
                .AddTransient<IEmailService, EmailService>();

        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                });

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MainDbConnection") 
                                  ?? throw new InvalidOperationException("Connection string 'MainDbConnection' not found.");

        services.AddDbContext<MainDbContext>(options =>
           options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection AddScopedByBaseType(this IServiceCollection services, Type baseType)
    {
        Assembly.GetAssembly(baseType)
                .GetTypesOf(baseType)
                .ForEach(type => services.AddScoped(type.GetInterface($"I{type.Name}"), type));

        return services;
    }

    public static async Task<IServiceProvider> AddAdminAsync(this IServiceProvider service)
    {
        using (var scope = service.CreateScope())
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
                await userRepository.Cadastrar(usuario);
            }
        }

        return service;
    }

}
