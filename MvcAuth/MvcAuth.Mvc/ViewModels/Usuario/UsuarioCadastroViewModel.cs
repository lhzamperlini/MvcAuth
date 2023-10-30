using MvcAuth.Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcAuth.Mvc.ViewModels.Usuario;

public class UsuarioCadastroViewModel
{
    [Key]
    public Guid? Id { get; set; }

    [DisplayName("Nome")]
    public string Nome { get; set; }

    [DisplayName("Sobrenome")]
    public string Sobrenome { get; set; }

    [DisplayName("Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "O {0} é obrigatorio.")]
    public string Email { get; set; }

    [DisplayName("Senha")]
    public string Senha { get; set; }

    [DisplayName("Confirmação de Senha")]
    public string SenhaConfirmacao { get; set; }
}
