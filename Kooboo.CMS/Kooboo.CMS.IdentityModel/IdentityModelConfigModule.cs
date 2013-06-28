using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.Security.Claims;
using System.Web.Helpers;
using Kooboo.CMS.Common.Runtime.Dependency.Ninject;
using Kooboo.CMS.Web.Areas.Account.Controllers;

namespace Kooboo.CMS.IdentityModel
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "AntiForgeryConfigModule")]
    public class IdentityModelConfigModule : Kooboo.CMS.Common.HttpApplicationEvents
    {
        public override void Application_Start(object sender, System.EventArgs e)
        {
            // set the anti CSRF for name (that's a unqiue claim in our system)
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            // register logon handler
            EngineContext.Current.ContainerManager.AddComponent<ILogOnHandler, LogOnHandler>();

            base.Application_Start(sender, e);
        }
    }
}