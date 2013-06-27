using Kooboo.CMS.Common.Runtime.Dependency;
using System.Security.Claims;
using System.Web.Helpers;

namespace Kooboo.CMS.Web.Authorizations
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "AntiForgeryConfigModule")]
    public class AntiForgeryConfigModule : Kooboo.CMS.Common.HttpApplicationEvents
    {
        public override void Application_Start(object sender, System.EventArgs e)
        {
            // set the anti CSRF for name (that's a unqiue claim in our system)
            // TODO: check if JWT then last segment otherwise whole claim name 
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            base.Application_Start(sender, e);
        }
    }
}