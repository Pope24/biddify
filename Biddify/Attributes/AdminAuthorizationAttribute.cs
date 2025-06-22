using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Biddify.Attributes
{
    public class AdminAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToPageResult("/Authen/Login");
                return;
            }

            var isAdmin = user.HasClaim(ClaimTypes.Role, "Admin");
            var isSeller = user.HasClaim(ClaimTypes.Role, "Seller");

            if (!isAdmin && !isSeller)
            {
                context.Result = new RedirectToPageResult("/Error", new { statusCode = 403 });
                return;
            }

            if (context.HttpContext.Request.Path.StartsWithSegments("/Admin/Users") && !isAdmin)
            {
                context.Result = new RedirectToPageResult("/Error", new { statusCode = 403 });
                return;
            }
        }
    }
} 