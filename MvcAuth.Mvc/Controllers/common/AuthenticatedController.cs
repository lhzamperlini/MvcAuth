
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MvcAuth.Mvc.Controllers.common;
public class AuthenticatedController : Controller
{

    public Guid UsuarioId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty; } }
    public string NomeDeUsuario { get { return User.FindFirstValue("Nome"); } }
    public string Email { get { return User.FindFirstValue(ClaimTypes.Email); } }
    public string TipoUsuario { get { return User.FindFirstValue(ClaimTypes.Role); } }
    public bool IsConfirmado { get { return VerificaIsConfirmado(); } }

    private bool VerificaIsConfirmado()
    {
        return true;
    }

}
