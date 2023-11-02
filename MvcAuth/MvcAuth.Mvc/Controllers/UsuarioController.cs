using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Mvc.Auth;
using MvcAuth.Mvc.Controllers.common;
using MvcAuth.Mvc.Mappers;
using MvcAuth.Mvc.ViewModels.Usuario;

namespace MvcAuth.Mvc.Controllers;

[CookieAuthorize("Administrador")]
public class UsuarioController : AuthenticatedController
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    #region Views

    public async Task<IActionResult> Index() => View(UsuarioMapper.ModelToViewModelLista(await _usuarioService.ObterLista()));

    [Route("detalhes")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var model = await _usuarioService.ObterPorId(id);
        return model switch { null => NotFound(), _ => View(UsuarioMapper.ModelToViewModel(model)) };
    }

    [Route("/cadastro")]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastro() => await Task.FromResult(View());

    [Route("deletar")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var model = await _usuarioService.ObterPorId(id);
        return model switch { null => NotFound(), _ => View(UsuarioMapper.ModelToViewModel(model)) };
    }

    [Route("Editar")]
    public async Task<IActionResult> Editar(Guid Id)
    {
        var usuario = await _usuarioService.ObterPorId(Id);
        if (usuario == null)
            return NotFound();

        return View(UsuarioMapper.ModelToViewModel(usuario));
    }

    #endregion

    #region Ações

    [HttpPost("Cadastro")]
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
                    return View();
                }

                await _usuarioService.Cadastrar(UsuarioMapper.ViewModelToModel(viewModel));
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
                return View(viewModel);
            }
        }
        return View(viewModel);
    }

    [HttpPost("Editar")]
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
