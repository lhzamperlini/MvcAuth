using MvcAuth.Domain.Interfaces.Repositories;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Domain.Models;
using System.Data;

namespace MvcAuth.Application.Services;
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
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

    public async Task Cadastrar(Usuario usuario)
    {
        if (await _usuarioRepository.VerificarExistente(usuario.Email))
            throw new Exception("Um usuario com este email já está cadastrado.");

        await _usuarioRepository.Cadastrar(usuario);
    }

    //TODO Soft Delete
    public async Task Deletar(Guid Id) => await _usuarioRepository.Deletar(Id);

    public async Task Editar(Usuario usuario)
    {
        try
        {
            var usuarioAtual = await _usuarioRepository.ObterPorId(usuario.Id);

            if (usuarioAtual == null)
                throw new Exception("Usuario não encontrado.");

            if(!usuario.Email.Equals(usuarioAtual.Email) && await _usuarioRepository.VerificarExistente(usuario.Email))
                throw new Exception("Um usuario com este email já está cadastrado.");

            usuarioAtual.Nome = usuario.Nome;
            usuarioAtual.Sobrenome = usuario.Sobrenome;
            usuarioAtual.Email = usuario.Email;

            await _usuarioRepository.Atualizar(usuarioAtual);
        }
        catch (DBConcurrencyException ex)
        {
            throw new Exception("Outra pessoa está tentando editar esse usuario no momento");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Usuario>> ObterLista() => await _usuarioRepository.ObterLista();

    public async Task<Usuario?> ObterPorId(Guid Id) => await _usuarioRepository.ObterPorId(Id);

    #endregion
}
