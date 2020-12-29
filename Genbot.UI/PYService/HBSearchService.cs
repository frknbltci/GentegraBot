using Genbot.UI.Attribute;
using Genbot.UI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;



namespace Genbot.UI.PYService
{
    public class HBSearchService
    {
        Uri hbURL = new Uri("https://www.hepsiburada.com/ara?q=");

        WebClient client = new WebClient();



        public HBVM HBPull(string barcode)
        {
            var headPUrl = "https://www.hepsiburada.com";
            //var searchURL = "https://www.hepsiburada.com/ara?q=";

            bool PageActive = true;
            client.Encoding = Encoding.UTF8;
           

            
            string aranacak = barcode;

            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

            if (PageActive)
            {
                string html = client.DownloadString(hbURL + barcode);
                HtmlDocument dokuman = new HtmlDocument();
                HtmlDocument dokuman1 = new HtmlDocument();
                dokuman.LoadHtml(html);
                var sonuc = "";
                if (dokuman.DocumentNode.SelectSingleNode("//i[contains(@class,'icon no-result')]") == null)
                {
                    sonuc = "urunlervar";
                }
                else
                {
                    sonuc = "urunleryok";
                }
                string hbPurl = "";
                if (dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'box product')]/a") != null)
                {
                    hbPurl = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'box product')]/a").Attributes["href"].Value;
                }
                else
                {
                    sonuc = "urunleryok";
                }

                var productErr = new HBVM();
                productErr.hbseller = new List<HBSELLER>();
                if (sonuc == "urunleryok")
                {
                    productErr.ProductName = "Ürün Bulunamadı";
                    productErr.hbseller.Add(new HBSELLER
                    {
                        HPrice = "Ürün Bulunamadı",
                        KPrice = "Ürün Bulunamadı",
                        LPrice = "Ürün Bulunamadı",
                        SellerName = "Ürün Bulunamadı",
                        SellerURL = "Ürün Bulunamadı",
                    });
                    productErr.ProductURL = "Ürün Bulunamadı";
                    productErr.PBarcode = barcode;
                    return productErr;
                }
                else
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0(Windows NT 10.0; Win64 x64)AppleWebKit/537.36(KTHML,like Gecko)");
                    string html2 = client.DownloadString(headPUrl + hbPurl.Trim());
                    dokuman1.LoadHtml(html2);
                    HtmlNode test1 = dokuman1.DocumentNode;


                    var product = new HBVM();
                    product.hbseller = new List<HBSELLER>();
                    var otherSell = test1.SelectSingleNode("//div[@class='merchantList']").Attributes["style"].Value;
                    if (otherSell == "display: none")
                    {
                        HBSELLER vm = new HBSELLER();
                        vm.SellerURL = headPUrl + test1.SelectSingleNode("//span[@class='seller-container']//span//a").Attributes["href"].Value;
                        vm.SellerName = test1.SelectSingleNode("//[@class='seller']//span").InnerText;
                        
                        try
                        {
                            var f1 = test1.SelectSingleNode("//span[@id='offering-price']/span[1]").InnerText;
                            var f2 = test1.SelectSingleNode("//span[@id='offering-price']/span[2]").InnerText;
                            vm.KPrice = test1.SelectSingleNode("/html/body/div[2]/main/div[3]/section[1]/div[4]/div/div[4]/div[1]/div[2]/div/div[1]/div[2]/div[2]/span").InnerText;
                            vm.LPrice = f1 + "," + f2;
                            vm.HPrice = test1.SelectSingleNode("//del[@id='originalPrice']").InnerText;
                        }
                        catch (Exception)
                        {
                            var f1 = test1.SelectSingleNode("//span[@id='offering-price']/span[1]").InnerText;
                            var f2 = test1.SelectSingleNode("//span[@id='offering-price']/span[2]").InnerText;
                            vm.HPrice = test1.SelectSingleNode("//del[@id='originalPrice']").InnerText;
                            vm.LPrice = f1 + "," + f2;
                            vm.KPrice = "Kampanyalı Fiyat Bulunamadı";
                        }
                    }
                    else
                    {
                        HBSELLER seller1 = new HBSELLER();
                        //HBSELLER seller2 = new HBSELLER();

                        seller1.SellerName = test1.SelectSingleNode("//div[contains(@class,'seller-container')]/span/span[2]/a").InnerText;
                        seller1.SellerURL = headPUrl + "/magaza/"+ StringOperations.slugnbsp(seller1.SellerName.Trim().ToLower());
                        product.ProductName = test1.SelectSingleNode("//*[@id='product-name']").InnerText;
                        product.ProductURL = headPUrl + hbPurl.Trim();
                        product.PBarcode = barcode;
                        //try
                        //{
                            var f1 = test1.SelectSingleNode("//span[@id='offering-price']/span[1]").InnerText;
                            var f2 = test1.SelectSingleNode("//span[@id='offering-price']/span[2]").InnerText;
                            seller1.HPrice = test1.SelectSingleNode("//del[@id='originalPrice']").InnerText;
                            seller1.LPrice = f1 + "," + f2;

                            seller1.KPrice = test1.SelectSingleNode("//div[@class='extra-price']//div[2]") != null ? test1.SelectSingleNode("//div[@class='extra-price']//div[2]").InnerText : "Kampanyalı Fiyat Bulunamadı";
                        //}
                        //catch (Exception)
                        //{
                        //    var f1 = test1.SelectSingleNode("//span[@id='offering-price']/span[1]").InnerText;
                        //    var f2 = test1.SelectSingleNode("//span[@id='offering-price']/span[2]").InnerText;
                        //    seller1.HPrice = test1.SelectSingleNode("//del[@id='originalPrice']").InnerText;
                        //    seller1.LPrice = f1 + "," + f2;
                        //    seller1.KPrice = "Kampanyalı Fiyat Bulunamadı";
                        //}
                        product.hbseller.Add(seller1);
                        
                        //seller2.SellerName = test1.SelectSingleNode("//td[@class='shipping-and-campaigns']/div[1]/a").InnerText;
                        ////html/body/div[2]/main/div[3]/section[1]/div[4]/div/div[4]/div[2]/div[2]/div/div[2]/table/tbody/tr[1]/td[1]
                        //seller2.SellerURL = test1.SelectSingleNode("/html/body/div[2]/main/div[3]/section[1]/div[4]/div/div[4]/div[2]/div[2]/div/div[2]/table/tbody/tr[1]/td[1]/div[1]/a[1]").Attributes["href"].Value;
                        
                        //seller2.LPrice = test1.SelectSingleNode("/html/body/div[2]/main/div[3]/section[1]/div[4]/div/div[4]/div[2]/div[2]/div/div[2]/table/tbody/tr[1]/td[2]/span").InnerText;
                        //seller2.KPrice = "Kampanyalı Fiyat Bulunamadı";
                        //seller2.HPrice = "Kampanyalı Fiyat Bulunamadı";

                        //product.hbseller.Add(seller2);
                        return product;
                    }
                }
            }
            return default(HBVM);
        }
    }
}