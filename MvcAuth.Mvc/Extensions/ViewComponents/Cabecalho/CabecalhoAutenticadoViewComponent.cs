using Microsoft.AspNetCore.Mvc;

namespace MvcAuth.Mvc.Extensions.ViewComponents.Cabecalho;

[ViewComponent(Name = "CabecalhoAutenticado")]
public class CabecalhoAutenticadoViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult(View());
    }
}
