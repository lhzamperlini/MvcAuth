using MvcAuth.Domain.Enums;
using MvcAuth.Domain.Models.common;
using System;

namespace MvcAuth.Domain.Models;
public class Usuario : EntityBase
{

    public Usuario()
    {
        GerarCodigoConfirmacao();
    }

    public void GerarCodigoConfirmacao()
    {
        var random = new Random();
        CodigoConfirmacao = random.Next(1000, 100000);
    }

    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    //Sim, no começo a senha não vai ter criptografia
    public string Senha { get; set; }
    public bool Ativo { get; set; }
    public bool Confirmado { get; set; }
    public int CodigoConfirmacao { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
}
