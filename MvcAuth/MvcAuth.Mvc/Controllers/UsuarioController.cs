using Microsoft.AspNetCore.Mvc;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Mvc.Mappers;
using MvcAuth.Mvc.ViewModels.Auth;
using MvcAuth.Mvc.ViewModels.Usuario;

namespace MvcAuth.Mvc.Controllers;
public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<IActionResult> Index()
    {
        return View(UsuarioMapper.ModelToViewModelLista(await _usuarioService.ObterLista()));
    }

    public async Task<IActionResult> Detalhes(Guid id)
    {
        var model = await _usuarioService.ObterPorId(id);
        return model switch { null => NotFound(), _ => View(UsuarioMapper.ModelToViewModel(model)) };
    }

    [HttpGet("/Cadastro")]
    public async Task<IActionResult> Cadastro() => await Task.FromResult(View());


    public async Task<IActionResult> Deletar(Guid id)
    {
        var model = await _usuarioService.ObterPorId(id);
        return model switch { null => NotFound(), _ => View(UsuarioMapper.ModelToViewModel(model)) };
    }

    public async Task<IActionResult> Editar(Guid Id)
    {
        var usuario = await _usuarioService.ObterPorId(Id);
        if (usuario == null)
            return NotFound();

        return View(UsuarioMapper.ModelToViewModel(usuario));
    }

    #region Ações

    [HttpPost]
    public async Task<IActionResult> CadastroConfirm(UsuarioCadastroViewModel viewModel)
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

    [HttpPost]
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
