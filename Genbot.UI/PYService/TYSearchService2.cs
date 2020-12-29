using Genbot.UI.Attribute;
using Genbot.UI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Genbot.UI.PYService
{
    public class TYSearchService2
    {

        Uri urlty = new Uri("https://www.trendyol.com/tum--urunler?q=");

        WebClient client = new WebClient();

        public TYVM TYPull2(string barcode)
        {
            var headPurl = "https://www.trendyol.com";

            bool PageActive = true;
            client.Encoding = Encoding.UTF8;

            client.Headers.Add("user-agent", "Mozilla/5.0(Windows NT 10.0; Win64; x64) AppleWebkit/537.36 (KHTML,like Gecko)");


            if (PageActive)
            {
                string html = client.DownloadString(urlty + barcode);
                HtmlDocument dokuman = new HtmlDocument();
                HtmlDocument dokuman1 = new HtmlDocument();
                dokuman.LoadHtml(html);
                var sonuc = "";
                if (dokuman.DocumentNode.SelectSingleNode("div[@class='no-rslt-info']") == null)
                {
                    sonuc = "urunlervar";
                }
                else
                {
                    sonuc = "urunleryok";
                }

                string tyPurl = "";

                if (dokuman.DocumentNode.SelectSingleNode("//div[@class='p-card-wrppr add-to-bs-card']/div[1]/a") != null)
                {
                    tyPurl = dokuman.DocumentNode.SelectSingleNode("//div[@class='p-card-wrppr add-to-bs-card']/div[1]/a").Attributes["href"].Value;
                }
                else
                {
                    sonuc = "urunleryok";
                }

                var productErr = new TYVM();
                productErr.tyseller = new List<TYSELLER>();
                if (sonuc == "urunleryok")
                {
                    productErr.ProductName = "Ürün Bulunamadı";
                    productErr.tyseller.Add(new TYSELLER
                    {
                        HPrice = "Ürün Bulunamadı",
                        KPrice = "Ürün Bulunamadı",
                        LPrice = "Ürün Bulunamadı",
                        SellerName = "Ürün Bulunamadı",
                        SellerURL = "Ürün Bulunamadı"
                    });
                    productErr.ProductURL = "Ürün Bulunamadı";
                    productErr.PBarcode = barcode;
                    return productErr;
                }
                else
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0(Windows NT 10.0; Win64 x64)AppleWebKit/537.36(KTHML,like Gecko)");
                    string html2 = client.DownloadString(headPurl + tyPurl.Trim());
                    dokuman1.LoadHtml(html2);
                    HtmlNode test1 = dokuman1.DocumentNode;

                    var product = new TYVM();
                    product.tyseller = new List<TYSELLER>();
                    var otherSell = test1.SelectNodes("//div[@class='pr-mc-w']");

                    if (otherSell == null)
                    {
                        TYSELLER vm = new TYSELLER();
                        vm.SellerURL = headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value;
                        vm.SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText;
                        try
                        {
                            vm.LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[2]").InnerText;
                            vm.HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[1]").InnerText;
                            vm.KPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/span").InnerText;
                        }
                        catch (Exception)
                        {
                            vm.LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/span").InnerText;
                            vm.HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span").InnerText;
                            vm.KPrice = "Kampanya Fiyatı Bulunamadı";
                        }
                        product.tyseller.Add(vm);
                        return product;
                    }

                    else
                    {
                        TYSELLER seller1 = new TYSELLER();
                        TYSELLER seller2 = new TYSELLER();

                        seller1.SellerURL = headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value;
                        seller1.SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText;
                        try
                        {
                            seller1.LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[2]").InnerText;
                            seller1.HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[1]").InnerText;
                            seller1.KPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/span").InnerText;
                        }
                        catch (Exception)
                        {
                            seller1.LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/span").InnerText;
                            seller1.HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span").InnerText;
                            seller1.KPrice = "Kampanya Fiyatı Bulunamadı";
                        }
                        product.tyseller.Add(seller1);

                        seller2.SellerURL = headPurl + test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]").Attributes["href"].Value;
                        seller2.SellerName = StringOperations.delnumb(test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]/div[2]/div[1]").InnerText);

                        try
                        {
                            seller2.KPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]/div[1]/div/div[3]/div/span").InnerText;
                            seller2.HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]/div[1]/div/div[1]/div/span[1]").InnerText;
                            seller2.LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]/div[1]/div/div[1]/div/span[2]").InnerText;
                        }
                        catch (Exception)
                        {
                            seller2.KPrice = "Kampanya Fiyatı Bulunamadı";
                            seller2.LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]/div[1]/div/span[2]").InnerText;
                            seller2.HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[6]/div[2]/a[1]/div[1]/div/span[1]").InnerText;
                        }

                        product.tyseller.Add(seller2);

                        return product;
                    }
                }
            }
            return default(TYVM);
        }
    }
}
