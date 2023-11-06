using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Mvc.Auth;
using MvcAuth.Mvc.Controllers.common;
using MvcAuth.Mvc.Mappers;
using MvcAuth.Mvc.ViewModels.Auth;
using MvcAuth.Mvc.ViewModels.Usuario;

namespace MvcAuth.Mvc.Controllers;
[UserConfirmedFilter]
public class UsuarioController : AuthenticatedController
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }


    #region Usuario

    [HttpGet("/Cadastro")]
    public async Task<IActionResult> Cadastro() => await Task.FromResult(View());

    [HttpGet("/Meu-Perfil")]
    [CookieAuthorize]
    public async Task<IActionResult> MeuPerfil()
    {
        var usuario = await _usuarioService.ObterPorId(UsuarioId);
        return View(UsuarioMapper.ModelToViewModel(usuario));
    }

    [HttpGet]
    [Route("/Usuario/Confirmar-Cadastro")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmarCadastro()
    {
        return await Task.FromResult(View());
    }

    [HttpGet("/Alterar-Senha")]
    public async Task<IActionResult> AlterarSenha()
    {
        return await Task.FromResult(View());
    }

    #endregion

    #region Ações Usuario

    [HttpPost("/Cadastro")]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastro(UsuarioCadastroViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (!viewModel.Senha.Equals(viewModel.SenhaConfirmacao))
                {
                    TempData["Erro"] = "As senhas não coincidem.";
                    return RedirectToAction(nameof(Cadastro));
                }

                await _usuarioService.Cadastrar(UsuarioMapper.ViewModelToModel(viewModel));

                TempData["Sucesso"] = "Cadastro efetuado com sucesso, realize o login abaixo.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
                return View(viewModel);
            }
        }
        return View(viewModel);
    }

    [HttpPost("/ConfirmarAlteracao")]
    [CookieAuthorize]
    public async Task<IActionResult> MeuPerfil(UsuarioViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (!UsuarioId.Equals(viewModel.Id))
            {
                TempData["Erro"] = "Você não pode alterar um perfil que não seja o seu.";
                return View(viewModel);
            }

            try
            {
                await _usuarioService.Atualizar(UsuarioMapper.ViewModelToModel(viewModel));
                TempData["Sucesso"] = "Seu perfil foi atualizado com sucesso!";
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Algo deu errado ao atualizar o seu perfil";
                return View(viewModel);
            }
        }

        TempData["Erro"] = "Revise seus dados.";
        return View(viewModel);
    }

    [HttpPost("/Alterar-Senha")]
    public async Task<IActionResult> AlterarSenha(UsuarioAlterarSenha viewModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (!viewModel.NovaSenha.Equals(viewModel.ConfirmacaoSenha))
                {
                    TempData["Erro"] = "A nova senha e a confirmação não coincidem.";
                    return View(viewModel);
                }

                await _usuarioService.AlterarSenha(UsuarioId, viewModel.SenhaAntiga, viewModel.NovaSenha);
                TempData["Sucesso"] = "Sua senha foi atualizada com sucesso!";
                return View();
            }
            
            return View(viewModel); 
        }
        catch (Exception ex)
        {
            TempData["Erro"] = "Algo deu errado ao atualizar a sua senha.";
            return View(viewModel);
        }
    }

    #endregion

    #region Admin

    [CookieAuthorize("Administrador")]
    public async Task<IActionResult> Index()
    {
        return View(UsuarioMapper.ModelToViewModelLista(await _usuarioService.ObterLista()));
    }

    [CookieAuthorize("Administrador")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var model = await _usuarioService.ObterPorId(id);
        return model switch { null => NotFound(), _ => View(UsuarioMapper.ModelToViewModel(model)) };
    }

    [CookieAuthorize("Administrador")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var model = await _usuarioService.ObterPorId(id);
        return model switch { null => NotFound(), _ => View(UsuarioMapper.ModelToViewModel(model)) };
    }

    [CookieAuthorize("Administrador")]
    public async Task<IActionResult> Editar(Guid Id)
    {
        var usuario = await _usuarioService.ObterPorId(Id);
        if (usuario == null)
            return NotFound();

        return View(UsuarioMapper.ModelToViewModel(usuario));
    }

    #endregion

    #region Ações Admin

    [HttpPost]
    [CookieAuthorize("Administrador")]
    public async Task<IActionResult> Editar(Guid Id, UsuarioViewModel viewModel)
    {
        if (Id != viewModel.Id)
            return View(viewModel);

        if (ModelState.IsValid)
        {
            try
            {
                await _usuarioService.Editar(UsuarioMapper.ViewModelToModel(viewModel));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return View(viewModel);
    }

    [HttpPost]
    [CookieAuthorize("Administrador")]
    public async Task<IActionResult> ConfirmDelete(Guid Id)
    {
        try
        {
            await _usuarioService.Deletar(Id);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion

}
