using Genbot.DAL.DB;
using Genbot.UI.Attribute;
using Genbot.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Genbot.UI.Controllers
{
    [Auth]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            if (((SessionContext)Session["SessionContext"]).Role == true)
            {
                var li = service.UsersService.GetAll();
                return View(li);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        public ActionResult Update(int? ID)
        {
            if (((SessionContext)Session["SessionContext"]).Role == true)
            {
                if (ID != null)
                {
                    var bulunan = service.UsersService.Find((int)ID);
                    return View(bulunan);
                }
                else
                {
                    return Redirect("/User/Index");
                }
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult Update(Users gelen)
        {
            if (gelen != null)
            {
                service.UsersService.Update(gelen);
                return Redirect("/User/Index");
            }
            else
            {
                return Redirect("/User/Index");
            }
        }

        public ActionResult Insert()
        {
            if (((SessionContext)Session["SessionContext"]).Role == true)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult Insert(UserVM gelen)
        {
            var Varmi = service.UsersService.FindUserName(gelen.UserName);
            if (Varmi == null)
            {
                var folderName = StringOperations.ToSlug(gelen.UserName);
                string folder = Server.MapPath("~/Assets/Upload/" + folderName);
                Directory.CreateDirectory(folder);

                Users use = new Users()
                {
                    UserName = gelen.UserName,
                    Password = gelen.Password,
                    IsActive = gelen.IsActive,
                    Role = gelen.Role,
                    FileURL = folder
                };

                service.UsersService.Insert(use);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            

        }
    }
}