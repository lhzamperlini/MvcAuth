
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MvcAuth.Mvc.Controllers.common;
public class AuthenticatedController : Controller
{

    public Guid UsuarioId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty; } }
    public string? NomeDeUsuario { get { return User.FindFirstValue("Nome"); } }
    public string? Email { get { return User.FindFirstValue(ClaimTypes.Email); } }
    public string? TipoUsuario { get { return User.FindFirstValue(ClaimTypes.Role); } }

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
}
