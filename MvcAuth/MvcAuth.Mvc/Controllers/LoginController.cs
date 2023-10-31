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
}
