using System.Security.Claims;
using System.Linq;

namespace Kooboo.CMS.Web.Authorizations
{
    public static class ClaimsPrincipalExtensions
    {
        private static string LastSegment(this string str)
        {
            return str.Trim('/').Split('/').Last();
        }

        private static string GetNameClaimType(this ClaimsPrincipal principal)
        {
            if (principal.FindFirstStripped(ClaimTypes.NameIdentifier) != null)
                return ClaimTypes.NameIdentifier.LastSegment();

            if (principal.FindFirstStripped(ClaimTypes.Name) != null)
                return ClaimTypes.Name.LastSegment();

            return ClaimTypes.Email.LastSegment();
        }

        public static ClaimsPrincipal MapToLocalPrincipal(this ClaimsPrincipal principal)
        {
            var identity = new ClaimsIdentity(principal.Claims, "Kooboo",
                GetNameClaimType(principal),
                ClaimTypes.Role.LastSegment());

            return new ClaimsPrincipal(identity);
        }

        public static Claim FindFirstStripped(this ClaimsPrincipal principal, string type)
        {
            var claim = principal.FindFirst(type);

            if (claim == null)
                claim = principal.FindFirst(type.LastSegment());

            return claim;
        }
    }
}