using MvcAuth.Domain.Interfaces.Repositories;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Domain.Models;
using System.Data;

namespace MvcAuth.Application.Services;
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmailService _emailService;

    public UsuarioService(IUsuarioRepository usuarioRepository, IEmailService emailService)
    {
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
    }

    #region Auth

    public async Task<Usuario> Autenticar(string email, string senha)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(email);

        if (usuario is null)
            throw new Exception("Usuario não encontrado.");

        if (!usuario.Senha.Equals(senha))
            throw new Exception("Usuario ou senha incorretos");

        return usuario;
    }

    #endregion

    #region CRUD

    //TODO Soft Delete
    public async Task Deletar(Guid Id) => await _usuarioRepository.Deletar(Id);

    public async Task<List<Usuario>> ObterLista() => await _usuarioRepository.ObterLista();

    public async Task<Usuario?> ObterPorId(Guid Id) => await _usuarioRepository.ObterPorId(Id);

    public async Task Cadastrar(Usuario usuario)
    {
        if (await _usuarioRepository.VerificarExistente(usuario.Email))
            throw new Exception("Um usuario com este email já está cadastrado.");

        usuario.Confirmado = false;
        usuario.Ativo = true;

        await _usuarioRepository.Cadastrar(usuario);
        _emailService.ConfirmacaoCadastro(usuario.Email);
    }

    public async Task Editar(Usuario usuario)
    {
        try
        {
            var usuarioAtual = await _usuarioRepository.ObterPorId(usuario.Id);

            if (usuarioAtual is null)
                throw new Exception("Usuario não encontrado.");

            if (!usuario.Email.Equals(usuarioAtual.Email) && await _usuarioRepository.VerificarExistente(usuario.Email))
                throw new Exception("Um usuario com este email já está cadastrado.");

            usuarioAtual.Nome = usuario.Nome;
            usuarioAtual.Sobrenome = usuario.Sobrenome;
            usuarioAtual.Email = usuario.Email;

            await _usuarioRepository.Atualizar(usuarioAtual);
        }
        catch (DBConcurrencyException)
        {
            throw new Exception("Outra pessoa está tentando editar esse usuario no momento.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task Atualizar(Usuario model)
    {
        var usuario = await _usuarioRepository.ObterPorId(model.Id);

        if (usuario is null)
            throw new Exception("Usuario não encontrado.");

        usuario.Nome = model.Nome;
        usuario.Sobrenome = model.Sobrenome;
        usuario.Email = model.Email;

        await _usuarioRepository.Atualizar(usuario);
    }

    public async Task AlterarSenha(Guid usuarioId, string senhaAntiga, string novaSenha)
    {
        var usuario = await _usuarioRepository.ObterPorId(usuarioId);
        if (usuario is null)
            throw new Exception("Usuario não encontrado.");

        if (usuario.Senha != senhaAntiga)
            throw new Exception("Senha antiga incorreta.");

        usuario.Senha = novaSenha;
        await _usuarioRepository.Atualizar(usuario);
    }


    #endregion
}
