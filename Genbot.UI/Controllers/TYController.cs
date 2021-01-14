using Codaxy.Xlio;
using Genbot.UI.Attribute;
using Genbot.UI.Models;
using Genbot.UI.PYService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Genbot.UI.Controllers
{
    [Auth]
    public class TYController : BaseController
    {
        // GET: TY
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExcelInsert(HttpPostedFileBase excelFile)
        {
            List<TYVM> vm = new List<TYVM>();
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                TempData["Mesaj"] = "Lütfen Dosya Seçiniz";
                return Redirect("/TY/Index");
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {

                    //string path2 = Server.MapPath("~/Assets/Upload/turan");

                    //string path = Server.MapPath((SessionContext)Session["SessionContext"]).FileURL + "/" + excelFile.FileName;

                    string path = Server.MapPath(((SessionContext)Session["SessionContext"]).FileURL) + "/" + excelFile.FileName;

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
             
                    excelFile.SaveAs(path);

                    var workbook = Workbook.Load(path);
                    var sheet = workbook.Sheets[0];
                    int count = int.MaxValue;

                    List<string> gelenbar = new List<string>();
                 
                    for (int i = 2; i < count; i++)
                    {
                        if (sheet["A" + i.ToString()].Value != null)
                        {
                            gelenbar.Add(sheet["A" + i.ToString()].Value.ToString());
                        }
                        else
                        {
                            break;
                        }
                    }

                    //int sayi = 1;
                    foreach (var item in gelenbar)
                    {
                        //if (sayi % 20 == 0)
                        //{
                        //    Thread.Sleep(1000 * 60);
                        //}

                        var sonuc = tyservice.TYPull(item);
                        vm.Add(sonuc);
                        //sayi++;
                    }
                }
            
                return View(vm);
            }
        }
    }
}
