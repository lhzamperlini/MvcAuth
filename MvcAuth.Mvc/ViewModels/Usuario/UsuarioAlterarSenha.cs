using System.ComponentModel;

namespace MvcAuth.Mvc.ViewModels.Usuario;

public class UsuarioAlterarSenha
{
    [DisplayName("Senha Antiga")]
    public string SenhaAntiga { get; set; }

    [DisplayName("Nova Senha")]
    public string NovaSenha { get; set; }

    [DisplayName("Confirmação de Senha")]
    public string ConfirmacaoSenha { get; set; }
}
