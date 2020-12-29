using Genbot.DAL.DB;
using Genbot.UI.Attribute;
using Genbot.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Genbot.UI.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult LoginControl(LoginVM cre)
        {
            if (ModelState.IsValid)
            {
                var urlistek = Request.Url.AbsoluteUri;
                var donencevap = licheck.LicenceChe(urlistek);
                if (donencevap == true)
                {
                    if (service.UsersService.login(cre.UserName, cre.Password))
                    {
                        Users use = service.UsersService.FindUserName(cre.UserName);
                        if (use != null)
                        {
                            SessionContext _sessionContext = new SessionContext()
                            {
                                UserName = use.UserName,
                                IsActive = (bool)use.IsActive,
                                Role = (bool)use.Role,
                                FileURL = use.FileURL
                            };
                            Session["SessionContext"] = _sessionContext;
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("gameover",JsonRequestBehavior.AllowGet);

                }

            }
            return Redirect("/Login/Index");
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return Redirect("/Login/Index");
        }
    }
}