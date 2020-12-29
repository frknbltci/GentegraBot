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
    public class N11SearchService
    {
        Uri urln11 = new Uri("https://www.n11.com/arama?q=");
        List<N11VM> _productList = new List<N11VM>();

        WebClient client = new WebClient();

        public List<N11VM> N11Pull(string Pname)
        {
            _productList.Clear();
            bool PageActive = true;
            client.Encoding = Encoding.UTF8;

            if (PageActive)
            {
                string html = client.DownloadString(urln11 + Pname);

                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument dokuman1 = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);

                string sonuc = dokuman.DocumentNode.SelectSingleNode("//div[@class='notFoundContainer']") != null ? "bulamadı" : "buldu";
                string sonuc2 = dokuman.DocumentNode.SelectSingleNode("//p[@class='result-mean-text-mm']") != null ? dokuman.DocumentNode.SelectSingleNode("//p[@class='result-mean-text-mm']").InnerText : "";




                if (sonuc == "bulamadı" || sonuc2.Contains("bulunamadı"))
                {
                    var productErr = new N11VM();
                    productErr.ProductName = "Ürün Bulunamadı";
                    productErr.ProductURL = "#";
                    productErr.SellerURL = "#";
                    productErr.HPrice = "Ürün Bulunamadı";
                    productErr.LPrice = "Ürün Bulunamadı";
                    productErr.SellerName = "Ürün Bulunamadı";
                    productErr.SearchName = "Ürün Bulunamadı";
                    _productList.Add(productErr);
                }

                else
                {
                    if (PageActive)
                    {
                        dokuman.LoadHtml(html);
                        List<string> listItemHtml;
                        try
                        {
                            listItemHtml = dokuman.DocumentNode.SelectNodes("//div[@class='catalogView ']//ul//li[@class='column ']").Select(li => li.OuterHtml).ToList();
                        }
                        catch (Exception)
                        {
                            return _productList;
                        }
                        int sayi = 0;

                        for (int i = 0; i < 2; i++)
                        {
                            if (listItemHtml.Count >= 2)
                            {
                                var product = new N11VM();
                                dokuman1.LoadHtml(listItemHtml[i]);
                                HtmlNode test12 = dokuman1.DocumentNode;
                                product.ProductName = test12.SelectSingleNode("//h3[contains(@class,'productName')]").InnerText;
                                product.ProductURL = test12.SelectSingleNode("//div[@class='pro']//a").Attributes["href"].Value;
                                product.SellerURL = test12.SelectSingleNode("//a[@class='sallerInfo']").Attributes["href"].Value;
                                product.SellerName = test12.SelectSingleNode("//span[@class='sallerName']").InnerText;
                                product.LPrice = test12.SelectSingleNode("//a[@class='newPrice']//ins").InnerText;
                                product.HPrice = test12.SelectSingleNode("//a[@class='oldPrice']//del") != null ? test12.SelectSingleNode("//a[@class='oldPrice']//del").InnerText : "Yüksek Fiyat Bulunamadı";
                                product.SearchName = Pname;
                                _productList.Add(product);
                                sayi++;
                                if (sayi == 2)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                var product = new N11VM();
                                dokuman1.LoadHtml(listItemHtml[i]);
                                HtmlNode test12 = dokuman1.DocumentNode;
                                product.ProductName = test12.SelectSingleNode("//h3[contains(@class,'productName')]").InnerText;
                                product.ProductURL = test12.SelectSingleNode("//div[@class='pro']//a").Attributes["href"].Value;
                                product.SellerURL = test12.SelectSingleNode("//a[@class='sallerInfo']").Attributes["href"].Value;
                                product.SellerName = test12.SelectSingleNode("//span[@class='sallerName']").InnerText;
                                product.LPrice = test12.SelectSingleNode("//a[@class='newPrice']//ins").InnerText;
                                product.HPrice = test12.SelectSingleNode("//a[@class='oldPrice']//del") != null ? test12.SelectSingleNode("//a[@class='oldPrice']//del").InnerText : "Yüksek Fiyat Bulunamadı";
                                product.SearchName = Pname;
                                _productList.Add(product);
                                sayi++;
                                if (sayi == 1)
                                {
                                    break;
                                }
                            }
                            //burda kaldım;

                        }


                    }
                }



            }

            return _productList;
        }
    }
}