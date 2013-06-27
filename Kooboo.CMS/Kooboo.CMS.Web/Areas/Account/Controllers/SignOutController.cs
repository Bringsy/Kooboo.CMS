using Kooboo.CMS.Web.Authorizations;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    [Authorize]
    public class SignOutController : Controller
    {
        public virtual ActionResult Index(string returnUrl)
        {
            return new RedirectResult(AuthorizationHelpers.GetSignOutQueryString());
        }
    }
}