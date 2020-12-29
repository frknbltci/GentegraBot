using Codaxy.Xlio;
using Genbot.UI.Attribute;
using Genbot.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Genbot.UI.Controllers
{
    [Auth]
    public class HBController : BaseController
    {

        // GET: HB
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExcelInsert(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                TempData["Mesaj"] = "Lütfen Dosya Seçiniz";
                return Redirect("/HB/Index");
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    string path = ((SessionContext)Session["SessionContext"]).FileURL + "/" + excelFile.FileName;

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    excelFile.SaveAs(path);

                    var workbook = Workbook.Load(path);
                    var sheet = workbook.Sheets[0];
                    int count = int.MaxValue;

                    List<string> gelenbar = new List<string>();
                    List<HBVM> hblist = new List<HBVM>();
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

                    foreach (var item in gelenbar)
                    {
                        var sonuc = hbService.HBPull(item);
                        hblist.Add(sonuc);

                    }

                    return View(hblist);
                }
                TempData["Mesaj"] = "Lütfen Dosya Seçiniz";
                return Redirect("/HB/Index");
            }
        }
    }
}