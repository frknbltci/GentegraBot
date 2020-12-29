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
    public class N11Controller : BaseController
    {
        
        // GET: N11
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
                return Redirect("/N11/Index");
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
                    List<N11VM> n11list = new List<N11VM>();
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
                        foreach (var item2 in n11Service.N11Pull(item))
                        {
                            N11VM vm = new N11VM()
                            {
                                HPrice = item2.HPrice,
                                LPrice = item2.LPrice,
                                ProductName = item2.ProductName,
                                ProductURL = item2.ProductURL,
                                SellerURL = item2.SellerURL,
                                SellerName = item2.SellerName,
                                SearchName = item2.SearchName
                            };
                            n11list.Add(vm);
                        }
                    }

                    return View(n11list);
                }
            }
            return View();



        }
    }
}