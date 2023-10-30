using MvcAuth.Domain.Models;
using MvcAuth.Mvc.ViewModels.Usuario;
using System.Reflection.Metadata.Ecma335;

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
