using Microsoft.AspNetCore.Mvc;
using MvcAuth.Mvc.Auth;
using MvcAuth.Mvc.Controllers.common;

namespace MvcAuth.Mvc.Controllers;

[CookieAuthorize]
public class MenuController : AuthenticatedController
{
    [UserConfirmedFilter]
    public IActionResult Index()
    {
        return View();
    }
}
