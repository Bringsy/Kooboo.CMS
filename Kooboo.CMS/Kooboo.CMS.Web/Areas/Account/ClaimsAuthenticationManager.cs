using System.Linq;
using Kooboo.CMS.Sites.Models;
using System.Security.Claims;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Account
{
    public class ClaimsAuthenticationManager : System.Security.Claims.ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            var principal = ClaimsAuthenticationManager.MapToLocalPrincipal(incomingPrincipal);

            if (principal.Identity.IsAuthenticated)
            {
                this.CreateOrUpdateKoobooUser(principal);
                // this.CreateOrUpdateBringsyProfile(principal);
            }

            return base.Authenticate(resourceName, principal);
        }

        public static ClaimsPrincipal MapToLocalPrincipal(ClaimsPrincipal principal)
        {
            var identity = new ClaimsIdentity(principal.Claims, "Kooboo",
                ClaimsAuthenticationManager.GetNameClaimType(principal),
                ClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }

        private static string GetNameClaimType(ClaimsPrincipal principal)
        {
            if (principal.FindFirst(ClaimTypes.NameIdentifier) != null)
                return ClaimTypes.NameIdentifier;

            if (principal.FindFirst(ClaimTypes.Name) != null)
                return ClaimTypes.Name;

            return ClaimTypes.Email;
        }

        public void CreateOrUpdateKoobooUser(ClaimsPrincipal principal)
        {
            // TODO: check if JWT is used, if yes strip claims if not use whole claims
            var nameClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (nameClaim == null)
                nameClaim = principal.FindFirst(ClaimTypes.Name);
            var uuid = nameClaim.Value;

            // Create a user if not already created
            var user = Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Get(uuid);

            if (user == null)
            {
                // Create user 
                Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Add(new Kooboo.CMS.Account.Models.User
                {
                    UUID = uuid,
                    UserName = uuid,
                    IsAdministrator = principal.IsInRole("Administrator") || principal.IsInRole("Administrators"),
                    Email = principal.FindFirst(ClaimTypes.Email).Value,
                });
            }
            else
            {
                // Update user 
                user.IsAdministrator = principal.IsInRole("Administrator") || principal.IsInRole("Administrators");
                user.Email = principal.FindFirst(ClaimTypes.Email).Value;
                Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Update(user.UserName, user);
            }

            // Assign user to current site if not already assigned
            var site = Kooboo.CMS.Sites.Persistence.Providers.SiteProvider.GetSiteByHostNameNPath(
                HttpContext.Current.Request.Url.Host, "");

            if (site != null)
            {
                var siteUser = Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Get(site,
                    principal.Identity.Name);

                if (siteUser == null)
                {
                    siteUser = new User
                    {
                        UUID = uuid,
                        UserName = principal.Identity.Name,
                        Profile = new Kooboo.CMS.Sites.Models.Profile(),
                        Site = site,
                        Roles = principal.FindAll(ClaimTypes.Role).Select(s => s.Value).ToList()
                    };

                    siteUser.Profile.Add("Email", principal.FindFirst(ClaimTypes.Email).Value);

                    Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Add(site, siteUser);
                }
                else
                {
                    var newSiteUser = new User
                    {
                        UUID = siteUser.UUID,
                        UserName = siteUser.UserName,
                        Profile = siteUser.Profile,
                        Site = siteUser.Site,
                        Roles = principal.FindAll(ClaimTypes.Role).Select(s => s.Value).ToList()
                    };

                    if (newSiteUser.Profile == null)
                        newSiteUser.Profile = new Kooboo.CMS.Sites.Models.Profile();

                    newSiteUser.Profile["Email"] = principal.FindFirst(ClaimTypes.Email).Value;

                    Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Update(site, newSiteUser, siteUser);
                }
            }
        }
    }
}