using System.Security.Claims;

namespace Kooboo.CMS.Web.Authorizations
{
    public static class ClaimsPrincipalExtensions
    {
        public static ClaimsPrincipal MapToLocalPrincipal(this ClaimsPrincipal principal)
        {
            var identity = new ClaimsIdentity(principal.Claims, "NDC", ClaimTypes.Email, ClaimTypes.Role);
            var pricipal = new ClaimsPrincipal(identity);
    
            return pricipal;
        }
    }
}