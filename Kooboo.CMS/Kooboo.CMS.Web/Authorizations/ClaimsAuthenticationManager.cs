using Kooboo.CMS.Sites.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Linq; 

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
            // TODO: check if JWT is used, if yes strip claims if not use whole claims
            var nameClaim = principal.FindFirstStripped(ClaimTypes.NameIdentifier);
            if (nameClaim == null)
                nameClaim = principal.FindFirstStripped(ClaimTypes.Name);
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
                    Email = principal.FindFirstStripped(ClaimTypes.Email).Value
                });
            }
            else
            {
                // Update user 
                user.IsAdministrator = principal.IsInRole("Administrator");
                user.Email = principal.FindFirstStripped(ClaimTypes.Email).Value;
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