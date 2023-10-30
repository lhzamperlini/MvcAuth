using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MvcAuth.Mvc.ViewModels.Usuario;

public class UsuarioViewModel
{
    [Key]
    public Guid Id { get; set; }

    [DisplayName("Nome")]
    public string Nome { get; set; }

    [DisplayName("Sobrenome")]
    public string Sobrenome { get; set; }

    [DisplayName("Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "O {0} é obrigatorio.")]
    public string Email { get; set; }
}
