namespace MvcAuth.Mvc.ViewModels.Auth;

public class AuthResult
{
    public AuthResult(Domain.Models.Usuario usuario)
    {
        Id = usuario.Id;
        Email = usuario.Email;
        Nome = usuario.Nome;
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Nome { get; set; }
    
}
