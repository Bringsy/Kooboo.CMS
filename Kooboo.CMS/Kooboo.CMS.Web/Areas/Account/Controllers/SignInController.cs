using Kooboo.CMS.Web.Authorizations;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    public class SignInController : Controller
    {
        public virtual ActionResult Index(string returnUrl)
        {
            return new RedirectResult(AuthorizationHelpers.GetSignInQueryString());
        }
    }
}