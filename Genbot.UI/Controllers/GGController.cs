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
    public class GGController : BaseController
    {
        // GET: GG
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExcelInsert(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                TempData["Mesaj"] = "Lütfen Dosya Seçiniz";
                return Redirect("/GG/Index");
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
                    List<GGVM> gglist = new List<GGVM>();
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
                        foreach (var item2 in ggService.GGPull(item))
                        {
                            GGVM vm = new GGVM()
                            {
                                SearchName = item2.SearchName,
                                HPrice = item2.HPrice,
                                LPrice = item2.LPrice,
                                ProductName = item2.ProductName,
                                ProductURL = item2.ProductURL,
                                SellerURL = item2.SellerURL,
                                SellerName = item2.SellerName
                            };
                            gglist.Add(vm);
                        }
                    }

                    return View(gglist);
                }
            }
            return View();
        }
    }
}