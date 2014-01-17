using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Sitemap.txt", Order = 1)]
    public class Sitemap_XmlController : Kooboo.CMS.Sites.AreaControllerBase
    {
        //
        // GET: /Sites/Robot_Txt/
        public virtual ActionResult Index()
        {
            Sitemap_Xml sitemap_xml = new Sitemap_Xml(this.Site);

            var sitemapStr = sitemap_xml.Read();

            sitemap_xml.Body = sitemapStr;
            return this.View(sitemap_xml);
        }

        [HttpPost]
        public virtual ActionResult Index(string body)
        {
            JsonResultData data = new JsonResultData(this.ModelState);
            data.RunWithTry((resultData) =>
            {
                Sitemap_Xml sitemap_xml = new Sitemap_Xml(this.Site);
                sitemap_xml.Save(body);
                data.AddMessage("The robots.txt has been saved.".Localize());
            });
            return this.Json(data);
        }
    }
}