using System;
using System.IdentityModel.Services;
using System.Web.Mvc;
using Kooboo.CMS.Web.Areas.Account.Controllers;

namespace Kooboo.CMS.IdentityModel
{
    public class LogOnHandler : ILogOnHandler
    {
        public ActionResult SignIn()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var request = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm) { Reply = fam.Reply };
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
