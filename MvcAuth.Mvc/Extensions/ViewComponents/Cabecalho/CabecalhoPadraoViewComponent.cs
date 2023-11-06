using Microsoft.AspNetCore.Mvc;

namespace MvcAuth.Mvc.Extensions.ViewComponents.Cabecalho;

[ViewComponent(Name = "CabecalhoPadrao")]
public class CabecalhoPadraoViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult(View());
    }
}
