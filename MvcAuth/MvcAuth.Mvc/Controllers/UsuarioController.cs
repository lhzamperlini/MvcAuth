using Microsoft.AspNetCore.Mvc;
using MvcAuth.Domain.Interfaces.Services;
using MvcAuth.Mvc.Mappers;
using MvcAuth.Mvc.ViewModels.Usuario;

namespace MvcAuth.Mvc.Controllers;

[Route("/usuario/")]
public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    #region Views

    public async Task<IActionResult> Index() => View(UsuarioMapper.ModelToViewModelLista(await _usuarioService.ObterLista()));

    [Route("/cadastro")]
    public async Task<IActionResult> Cadastro() => await Task.FromResult(View());


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
    public async Task<IActionResult> Cadastro(UsuarioCadastroViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (!viewModel.Senha.Equals(viewModel.SenhaConfirmacao))
                    return BadRequest("A senha e a confirmação não coincidem.");

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

    [HttpPost("Deletar")]
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
