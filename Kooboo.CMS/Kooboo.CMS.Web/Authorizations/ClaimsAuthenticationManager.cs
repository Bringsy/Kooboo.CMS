using Kooboo.CMS.Sites.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace Kooboo.CMS.Web.Authorizations
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

        public void CreateOrUpdateUser(ClaimsPrincipal principal)
        {
            // REPLACE IT BY CUSTOM CODE, ITS JUST A SAMPLE CODE ...

            // Create a user if not already created
            var user = Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Get(principal.Identity.Name);

            var nameClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (nameClaim == null)
                nameClaim = principal.FindFirst(ClaimTypes.Name);

            var uuid = nameClaim.Value;

            if (user == null)
            {
                Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Add(new Kooboo.CMS.Account.Models.User
                {
                    UUID = uuid,
                    IsAdministrator = principal.IsInRole("Administrator"),
                    UserName = principal.Identity.Name,
                    Email = principal.FindFirst(ClaimTypes.Email).Value,
                    /* Create a fake password */
                    Password = string.Format("{0}87r8r7923r230", uuid)
                });
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