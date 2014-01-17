using Kooboo.CMS.Common.Runtime.Dependency;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Sitemap.xml", Order = 1)]
    public class Sitemap_XmlController : Kooboo.CMS.Sites.AreaControllerBase
    {
        //
        // GET: /Sites/Sitemap_Xml/
        public virtual ActionResult Index()
        {
            var sitemapXml = new Sitemap_Xml(this.Site);
            sitemapXml.Body =  sitemapXml.Read();
            return this.View(sitemapXml);
        }

        [HttpPost]
        public virtual ActionResult Index(string body)
        {
            var data = new JsonResultData(this.ModelState);
            data.RunWithTry((resultData) =>
            {
                var sitemapXml = new Sitemap_Xml(this.Site);
                sitemapXml.Save(body);
                data.AddMessage("The sitemap.xml has been saved.".Localize());
            });
            return this.Json(data);
        }
    }

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISubscriber), Key = "SitemapXmlSubsciber")]
    public class CustomContentEventSubscriber : ISubscriber
    {
        EventResult ISubscriber.Receive(IEventContext context)
        {


            return new EventResult() { IsCancelled = false };
        }
    }

    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "SitemapXmlModule")]
    public class SitemapXmlModule : Kooboo.CMS.Common.HttpApplicationEvents
    {
        public override void Application_Start(object sender, System.EventArgs e)
        {

        }
    }
}