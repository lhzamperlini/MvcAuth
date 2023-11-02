using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Mvc.ViewModels.Auth;
using System.Security.Claims;
using MvcAuth.Mvc.Controllers.common;
using Microsoft.AspNetCore.Authorization;

namespace MvcAuth.Mvc.Controllers;

[AllowAnonymous]
public class LoginController : AuthenticatedController
{
    private readonly IUsuarioService _usuarioService;
    public LoginController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [Route("/Login")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/Logout")]
    public async Task<IActionResult> Logout()
    {
        await FazerLogout();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> LoginPost(LoginViewModel viewModel)
    {
        try
        {
            var usuario = await _usuarioService.Autenticar(viewModel.Email, viewModel.Senha);

            await SalvarAutenticacao(usuario);
            
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction(nameof(Index), viewModel);
        }
    }

    #region Metodos

    public async Task SalvarAutenticacao(Domain.Models.Usuario usuario)
    {
        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("NomeCompleto", usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString()),
        };

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), authProperties);
    }

    public async Task FazerLogout() => await HttpContext.SignOutAsync();

    #endregion
}
