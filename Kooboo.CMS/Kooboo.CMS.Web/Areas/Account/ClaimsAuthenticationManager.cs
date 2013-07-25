using Kooboo.CMS.Sites.Models;
using System.Security.Claims;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Account
{
    public class ClaimsAuthenticationManager : System.Security.Claims.ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            var principal = incomingPrincipal.MapToLocalPrincipal();

            if (principal.Identity.IsAuthenticated)
            {
                this.CreateOrUpdateUser(principal);
            }

            return base.Authenticate(resourceName, principal);
        }

        public virtual void CreateOrUpdateUser(ClaimsPrincipal principal)
        {
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
                    IsAdministrator = principal.IsInRole("Administrator"),
                    Email = principal.FindFirst(ClaimTypes.Email).Value
                });
            }
            else
            {
                // Update user 
                user.IsAdministrator = principal.IsInRole("Administrator");
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
                        Profile = new Profile(),
                        Site = site
                    };
                    Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Add(site, siteUser);
                }
            }
        }
    }
}