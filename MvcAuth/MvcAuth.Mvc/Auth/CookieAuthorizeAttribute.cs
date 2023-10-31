using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcAuth.Mvc.Auth;
public class CookieAuthorizeAttribute : AuthorizeAttribute
{
    public CookieAuthorizeAttribute() { }
    public CookieAuthorizeAttribute(params string[] profiles) => this.Roles = string.Join(",", profiles);

}