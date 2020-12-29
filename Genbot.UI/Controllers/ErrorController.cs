using Genbot.UI.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Genbot.UI.Controllers
{
    [Auth]
    public class ErrorController : BaseController
    {
        // GET: Error

        public ActionResult PageError()
        {
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View("PageError");
        }

        public ActionResult Page403()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            return View("PageError");
        }
        public ActionResult Page500()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            return View("PageError");
        }
        public ActionResult Page520()
        {
            Response.StatusCode = 520;
            Response.TrySkipIisCustomErrors = true;
            return View("PageError");
        }
        public ActionResult Page502()
        {
            Response.StatusCode = 502;
            Response.TrySkipIisCustomErrors = true;
            return View("PageError");
        }
    }
}