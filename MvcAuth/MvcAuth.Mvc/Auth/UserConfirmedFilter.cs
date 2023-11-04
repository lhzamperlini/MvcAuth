using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MvcAuth.Mvc.Auth;

public class UserConfirmedFilter : ActionFilterAttribute
{

    public UserConfirmedFilter() { }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var isConfirmado = filterContext.HttpContext.User.FindFirstValue("Confirmado");

        if (isConfirmado != null)
        {
            if (!isConfirmado.ToLower().Equals("true"))
            {
                filterContext.Result = new RedirectToRouteResult(new { action = "confirmar-cadastro", controller = "usuario"});
                base.OnActionExecuting(filterContext);
            }
        }       
    }
}
