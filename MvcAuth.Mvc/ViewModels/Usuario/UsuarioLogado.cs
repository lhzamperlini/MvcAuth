using System.Security.Claims;

namespace MvcAuth.Mvc.ViewModels.Usuario;

public class UsuarioLogado
{
    private readonly ClaimsPrincipal _principal;

    public UsuarioLogado(ClaimsPrincipal principal)
    {
        _principal = principal;
    }

    public Guid UsuarioId { get { return _principal.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_principal.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty; } }
    public bool IsAutenticado { get { return _principal.Identity.IsAuthenticated; } }
    public string NomeDeUsuario { get { return _principal.FindFirstValue("NomeCompleto"); } }
    public string Email { get { return _principal.FindFirstValue(ClaimTypes.Email); } }
    public string TipoUsuario { get { return _principal.FindFirstValue(ClaimTypes.Role); } }
    public bool IsComum { get { return _principal.IsInRole("Comum"); } }
    public bool IsPremium { get { return _principal.IsInRole("Premium"); } }
    public bool IsAdmin { get { return _principal.IsInRole("Administrador"); } }
}
