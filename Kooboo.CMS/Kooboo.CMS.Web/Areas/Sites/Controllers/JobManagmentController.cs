using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.View;
using Kooboo.Job;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class JobManagmentController : Controller
    {
        //
        // GET: /Sites/ApplicationManagment/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Recycle")]
        public ActionResult RecyclePost()
        {
            System.Web.Hosting.HostingEnvironment.InitiateShutdown();
            return RedirectToAction("Index");
        }

        public ActionResult Test()
        {
            var userName = (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) ? System.Web.HttpContext.Current.User.Identity.Name : string.Empty;

            var folder = ContentHelper.TextFolder("TestEntities");
            var query = folder.CreateQuery();

            var nvc = new NameValueCollection {
                {"UserKey", Guid.NewGuid().ToString()},
                {"Name", "123132"},
                {"Value", "321321"},
                {"ActiveFrom", DateTime.Now.ToString()}
            };


            //order.UUID = UUIDGenerator.DefaultGenerator.Generate(null);
            //nvc.Add("UserKey", Guid.NewGuid().ToString());

            Kooboo.CMS.Content.Services
                 .ServiceFactory.TextContentManager.Add(folder.Repository, folder, null, null, nvc, null, null, userName);

            return null;
        }

    }
}
