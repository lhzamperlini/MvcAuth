using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MvcAuth.Mvc.Auth;

public class UserConfirmedFilter : ActionFilterAttribute
{

    public UserConfirmedFilter() { }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        var isConfirmado = filterContext.HttpContext.User.FindFirstValue("Confirmado");
        var isRotaConfirmarCadastro = filterContext.RouteData.Values.Values.Contains("ConfirmarCadastro");


        if (isConfirmado != null && !isRotaConfirmarCadastro)
        {
            if (!isConfirmado.ToLower().Equals("true"))
            {
                filterContext.Result = new RedirectToRouteResult(new { action = "confirmar-cadastro", controller = "usuario"});
                base.OnActionExecuted(filterContext);
            }
        }       
    }
}
