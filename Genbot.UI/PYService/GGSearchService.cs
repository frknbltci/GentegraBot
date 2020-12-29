using Genbot.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using HtmlAgilityPack;

namespace Genbot.UI.PYService
{
    public class GGSearchService
    {
        Uri ggURL = new Uri("https://www.gittigidiyor.com/arama/?k=");
        List<GGVM> _ggProductList = new List<GGVM>();
        WebClient client = new WebClient();

        public List<GGVM> GGPull(string Pname)
        {
            var sellerURL = "https://www.gittigidiyor.com/magaza/";
            _ggProductList.Clear();
            bool PageActive = true;
            client.Encoding = Encoding.UTF8;
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

            if (PageActive)
            {
                string html = client.DownloadString(ggURL + Pname);
                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument dokuman1 = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);

                string sonuc = dokuman.DocumentNode.SelectSingleNode("//span[@class='search-info']//span[2]") != null ? "bulamadı" : "buldu";

                string sonuc2 = dokuman.DocumentNode.SelectSingleNode("//p[@class='result-mean-text-mm']") != null ? dokuman.DocumentNode.SelectSingleNode("//p[@class='result-mean-text-mm']").InnerText : "";

                if (sonuc == "bulamadı" || sonuc2.Contains("bulunamadı"))
                {
                    GGVM productErr = new GGVM()
                    {
                        HPrice = "Ürün Bulunamadı",
                        LPrice = "Ürün Bulunamadı",
                        ProductName = Pname,
                        ProductURL = "#",
                        SellerURL = "#",
                        SellerName = "Ürün Bulunamadı"
                    };
                    _ggProductList.Add(productErr);
                }
                else
                {
                    List<string> ggli = new List<string>();
                    ggli = dokuman.DocumentNode.SelectNodes("//li[contains(@class,'catalog-seem-cell')]").Select(li => li.OuterHtml).ToList();

                    int sayi = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        if (ggli.Count >= 2)
                        {
                            dokuman1.LoadHtml(ggli[i]);
                            HtmlNode test12 = dokuman1.DocumentNode;
                            GGVM vm = new GGVM();
                            vm.HPrice = test12.SelectSingleNode("//strike[@class='market-price-sel']") != null ? test12.SelectSingleNode("//strike[@class='market-price-sel']").InnerText : "Yüksek Fiyat Bulunamadı";
                            vm.LPrice = test12.SelectSingleNode("//p[contains(@class,'fiyat')]") != null ? test12.SelectSingleNode("//div[contains(@class,'priceListener ')]//div//p").InnerText : "Düşük Fiyat Bulunamadı";
                            vm.ProductName = test12.SelectSingleNode("//h3[contains(@class,'product-title')]").InnerText;
                            vm.ProductURL = test12.SelectSingleNode("//a").Attributes["href"].Value;
                            vm.SellerName = test12.SelectSingleNode("//span[@class='seller-nickname']").InnerText;
                            vm.SellerURL = sellerURL + vm.SellerName.Trim();
                            vm.SearchName = Pname;
                            _ggProductList.Add(vm);
                            sayi++;
                            if (sayi >= 2)
                            {
                                break;
                            }
                        }
                        else
                        {
                            dokuman1.LoadHtml(ggli[i]);
                            HtmlNode test12 = dokuman1.DocumentNode;
                            GGVM vm = new GGVM();
                            vm.HPrice = test12.SelectSingleNode("//strike[@class='market-price-sel']") != null ? test12.SelectSingleNode("//strike[@class='market-price-sel']").InnerText : "Yüksek Fiyat Bulunamadı";
                            vm.LPrice = test12.SelectSingleNode("//p[contains(@class,'fiyat')]").InnerText;
                            vm.ProductName = test12.SelectSingleNode("//h3[contains(@class,'product-title')]").InnerText;
                            vm.ProductURL = test12.SelectSingleNode("//a").Attributes["href"].Value;
                            vm.SellerName = test12.SelectSingleNode("//span[@class='seller-nickname']").InnerText;
                            vm.SellerURL = sellerURL + vm.SellerName.Trim();
                            _ggProductList.Add(vm);
                            sayi++;
                            if (sayi >= 1)
                            {
                                break;
                            }

                        }
                    }
                }
            }

            return _ggProductList;
        }
    }
}