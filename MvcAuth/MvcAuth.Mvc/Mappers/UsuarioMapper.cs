using MvcAuth.Domain.Enums;
using MvcAuth.Domain.Models;
using MvcAuth.Mvc.ViewModels.Usuario;

namespace MvcAuth.Mvc.Mappers;

public static class UsuarioMapper
{

    public static Usuario ViewModelToModel(UsuarioCadastroViewModel viewModel)
    {
        var model = new Usuario()
                        {
            
                            Nome = viewModel.Nome,
                            Sobrenome = viewModel.Sobrenome,
                            Email = viewModel.Email,
                            Senha = viewModel.Senha,
                        };

        if (viewModel.Id != null)
            model.Id = viewModel.Id.Value;
        else
            model.TipoUsuario = TipoUsuario.Comum;

        return model;
    }
    public static Usuario ViewModelToModel(UsuarioViewModel viewModel)
    {
        var model = new Usuario()
                        {
                            Id = viewModel.Id,            
                            Nome = viewModel.Nome,
                            Sobrenome = viewModel.Sobrenome,
                            Email = viewModel.Email,
                        };

        return model;
    }

    public static UsuarioViewModel ModelToViewModel(Usuario model)
    {
        return new UsuarioViewModel
        {
            Id = model.Id,
            Nome = model.Nome,
            Sobrenome = model.Sobrenome,
            Email = model.Email,
            TipoUsuario = model.TipoUsuario
        };
    }

    public static List<UsuarioViewModel> ModelToViewModelLista(List<Usuario> models) 
    {
        var viewModels = new List<UsuarioViewModel>();

        foreach (var model in models) 
            viewModels.Add(ModelToViewModel(model));
        return viewModels;
    }
}
