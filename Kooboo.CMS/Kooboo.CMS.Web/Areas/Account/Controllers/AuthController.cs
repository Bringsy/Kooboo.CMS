using System;
using System.IdentityModel.Services;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    public class AuthController : ControllerBase
    {
        public ActionResult SignIn(string returnUrl)
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var request = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm)
            {
                /* Reply = fam.Reply, */
                Context = string.Format("ru={0}", returnUrl)
            };

            return new RedirectResult(request.WriteQueryString());
        }

        public ActionResult SignOut()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var request = new SignOutRequestMessage(new Uri(fam.Issuer), fam.Realm) { Reply = fam.Reply };
            return new RedirectResult(request.WriteQueryString());
        }
    }
}