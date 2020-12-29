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
    public class MHPSearchService
    {
        Uri urlmh = new Uri("https://www.morhipo.com/arama/?q=");
        List<MHPVM> _productList = new List<MHPVM>();

        WebClient client = new WebClient();


        public List<MHPVM> MHPPull(string barcode)
        {

            var headPUrl = "https://www.morhipo.com";
            _productList.Clear();
            bool PageActive = true;
            client.Encoding = Encoding.UTF8;

            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
            string aranacak = barcode;

            if (PageActive)
            {
                string html = client.DownloadString(urlmh + aranacak + "&qcat=ps&sr=2");
                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument dokuman1 = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);

                string sonuc = dokuman.DocumentNode.SelectSingleNode("//p[@class='bigger']") != null ? dokuman.DocumentNode.SelectSingleNode("//p[@class='bigger']").InnerText : "";

                var product = new MHPVM();
                if (sonuc.Contains("ulaşamadık"))
                {
                    product.ProductName = barcode;
                    product.SellerURL = "Ürün Bulunamadı";
                    product.SellerName = "Ürün Bulunamadı";
                    product.ProductURL = "Ürün Bulunamadı";
                    product.LPrice = "Ürün Bulunamadı";
                    product.HPrice = "Ürün Bulunamadı";
                    product.PBarcode = barcode;
                    _productList.Add(product);

                }

                else
                {
                    try
                    {
                        string PUrl = dokuman.DocumentNode.SelectSingleNode("//a[contains(@class,'js-product')]").Attributes["href"].Value;
                        string html2 = client.DownloadString(headPUrl + PUrl.Trim());
                        dokuman1.LoadHtml(html2);
                        HtmlNode test12 = dokuman1.DocumentNode;
                        product.ProductName = StringOperations.slugnbsp(test12.SelectSingleNode("//p[contains(@class,'pd-block')]").InnerText);
                        product.SellerName = test12.SelectSingleNode("//span[@class='warn-txt']/strong") != null ? test12.SelectSingleNode("//span[@class='warn-txt']/strong").InnerText : test12.SelectSingleNode("//*[@id='merchant-info']/div/span[2]/strong").InnerText;
                        //test12.SelectSingleNode("//span[@class='warn-txt']/strong").InnerText;
                        product.HPrice = test12.SelectSingleNode("//div[@class='price-row']/span[2]") != null ? test12.SelectSingleNode("//div[@class='price-row']/span[2]").InnerText : "Yüksek Fiyat Bulunamadı";
                        var dusuk = test12.SelectSingleNode("//div[@class='price-row']/span") != null ? test12.SelectSingleNode("//div[@class='price-row']/span").InnerText : "Düşük Fiyat Bulunamadı";


                            //test12.SelectSingleNode("//div[@class='price-row']/span").InnerText;
                        product.LPrice = StringOperations.slugnbsp(dusuk);
                        // &nbst yazısı silinmeli
                        product.SellerURL = "Satıcı Linki Bulunmamaktadır.";
                        product.ProductURL = headPUrl + PUrl.Trim();
                        product.PBarcode = barcode;
                        _productList.Add(product);

                        return _productList;
                    }
                    catch
                    {
                        product.ProductName = barcode;
                        product.SellerName = "-";
                        product.HPrice = "-";
                        product.LPrice = "-";
                        // &nbst yazısı silinmeli
                        product.SellerURL = "-";
                        product.ProductURL = "-";
                        product.PBarcode = barcode;
                        _productList.Add(product);
                        return _productList;
                    }
                }
            }
            return _productList;


        }

    }
}