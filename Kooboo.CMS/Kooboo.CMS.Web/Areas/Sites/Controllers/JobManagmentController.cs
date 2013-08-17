using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    }
}
