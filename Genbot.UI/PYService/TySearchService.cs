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
    public class TySearchService
    {

        Uri urlty = new Uri("https://www.trendyol.com/tum--urunler?q=");
        WebClient client = new WebClient();

        public TYVM TYPull(string barcode)
        {
            try
            {
            //backHead:
            //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(urlty + barcode);
            //    req.Timeout = 100000;
            //    req.Method = "HEAD";
            //    HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            //    if (res.StatusCode == HttpStatusCode.OK)
            //    {
            var headPurl = "https://www.trendyol.com";

            bool PageActive = true;
            client.Encoding = Encoding.UTF8;
            //client.Proxy = new WebProxy("103.227.254.105", 8080);
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

            //mozilla ve chrome arasında dinamik olarak denenecek
            string html;
            if (PageActive)
            {
                try
                {
                    html = client.DownloadString(urlty + barcode);
                }
                catch (Exception)
                {
                    Thread.Sleep(2000);
                   
                    html = client.DownloadString(urlty + barcode);
                }

                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument dokuman1 = new HtmlAgilityPack.HtmlDocument();
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

                string _tyPurl = "";

                //Bulunan Ürünün linklerinin belirlendiği yer

                if (dokuman.DocumentNode.SelectSingleNode("//div[@class='p-card-wrppr add-to-bs-card']/div[1]/a") != null)
                {
                    _tyPurl = dokuman.DocumentNode.SelectSingleNode("//div[@class='p-card-wrppr add-to-bs-card']/div[1]/a").Attributes["href"].Value;
                }
                else if (dokuman.DocumentNode.SelectSingleNode("//*[@id='search-app']/div/div/div[2]/div[2]/div/div/div[1]/a") != null)
                {
                    _tyPurl = dokuman.DocumentNode.SelectSingleNode("//*[@id='search-app']/div/div/div[2]/div[2]/div/div/div[1]/a").Attributes["href"].Value;
                }
                else
                {
                    sonuc = "urunleryok";
                }

                var productErr = new TYVM();

                //linki bulamazsa ürün bulunamadı olarak giriş yapıyoruz
                productErr.tyseller = new List<TYSELLER>();
                if (sonuc == "urunleryok")
                {
                    productErr.ProductName = "Ürün Bulunamadı";
                    productErr.tyseller.Add(new TYSELLER
                    {
                        SellerURL = "Ürün Bulunamadı",
                        SellerName = "Ürün Bulunamadı",
                        LPrice = "Ürün Bulunamadı",
                        HPrice = "Ürün Bulunamadı"
                    });

                    productErr.ProductURL = "Ürün Bulunamadı";

                    productErr.PBarcode = barcode;

                    return productErr;
                }
                else
                {
                    //Bu aşama artık ürünün bilgileri ile bulunduğu sayfa url linkinden yönlendik

                    client.Headers.Add("user-agent", "Mozilla/5.0(Windows NT 10.0; Win64 x64)AppleWebKit/537.36(KTHML,like Gecko)");
                    string html2;
                    try
                    {
                         html2 = client.DownloadString(headPurl + _tyPurl.Trim());
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(2000);
                         html2 = client.DownloadString(headPurl + _tyPurl.Trim());
                    }

                    dokuman1.LoadHtml(html2);
                    HtmlNode test1 = dokuman1.DocumentNode;

                    var product = new TYVM();
                    product.tyseller = new List<TYSELLER>();

                    //Ürünün diğer satıcısı olup olmadığı kontrol etmek için
                    var otherSell = test1.SelectNodes("//div[@class='pr-omc']");

                    //Kampanyalı fiyat var mı durumu 
                    var varMi = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div/div/div[3]/div[2]/span");
                    if (otherSell == null)
                    {
                        if (varMi != null)
                        {
                            product.tyseller.Add(new TYSELLER
                            {
                                SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value),
                                SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText,
                                LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[2]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[2]").InnerText : "Düşük Fiyat Bulunamadı",
                                HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[1]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[1]/div/span[1]").InnerText : "Yüksek Fiyat Bulunamadı",
                                KPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/span") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/span").InnerText : "Kampanyalı Fiyat Bulunamadı",
                            });
                            //product.ProductName = test1.SelectSingleNode("//span[contains(@class,'pr-nm')]") != null ? test1.SelectSingleNode("//span[contains(@class,'pr-nm')]").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/h1/span").InnerText;
                            product.ProductName = test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]") != null ? test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]/span").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/h1/span").InnerText;
                            product.ProductURL = headPurl + _tyPurl.Trim();
                            product.PBarcode = barcode;
                            return product;
                        }
                        else
                        {
                            product.tyseller.Add(new TYSELLER
                            {
                                SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value),
                                SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText,
                                LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[2]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[2]").InnerText : test1.SelectSingleNode("//span[@class='prc-slg']") ==null ? "Düşük Fiyat Bulunamadı": test1.SelectSingleNode("//span[@class='prc-slg']").InnerText,
                                HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[1]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[1]").InnerText : test1.SelectSingleNode("//span[@class='prc-org']") == null ? "Yüksek Fiyat Bulunamadı" : test1.SelectSingleNode("//span[@class='prc-org']").InnerText,
                                KPrice = "Kampanyalı Fiyat Bulunamadı",
                            });
                            //LPrice = test1.SelectSingleNode("//span[@class='prc-org']") != null ? test1.SelectSingleNode("//span[@class='prc-org']").InnerText : "Yüksek Fiyat Bulunamadı",

                            //Son yaptığın sıkıntı çıkarırsa buraya try catch atarak yolu değişebilirsin

                            product.ProductName = test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]") != null ? test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]/span").InnerText:test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/h1/span").InnerText;
                            product.ProductURL = headPurl + _tyPurl.Trim();
                            product.PBarcode = barcode;
                            return product;
                        }
                        

                    }
                    else
                    {
                            //Diğer satıcı var kampanya yok 
                        if (varMi == null)
                        {
                            try
                            {
                                product.tyseller.Add(new TYSELLER
                                {
                                    SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value),
                                    SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText,


                                    LPrice = test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-slg']") != null ? test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-slg']").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div/div/span[2]") == null ? "Düşük Fiyat Bulunamadı" : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div/div/span[2]").InnerText,
                                    HPrice = test1.SelectSingleNode("//div[@class='pr-in-w']//span[@class='prc-org']") != null ? test1.SelectSingleNode("//div[@class='pr-in-w']//span[@class='prc-org']").InnerText : test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-org']") == null ? "Yüksek Fiyat Bulunamadı" : test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-org']").InnerText,

                                    //LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[2]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[2]").InnerText : "Düşük Fiyat Bulunamadı",

                                    //HPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[1]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div/div/span[1]").InnerText : "Yüksek Fiyat Bulunamadı",

                                    KPrice = "Kampanya Bulunamadı"

                                });

                                //product.ProductName = test1.SelectSingleNode("//span[contains(@class,'pr-nm')]").InnerText;
                                //product.ProductName = test1.SelectSingleNode("//span[contains(@class,'pr-nm')]") != null ? test1.SelectSingleNode("//span[contains(@class,'pr-nm')]").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/h1/span").InnerText;
                                product.ProductName = test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]") != null ? test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]/span").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/h1/span").InnerText;
                                product.ProductURL = headPurl + _tyPurl.Trim();
                                product.PBarcode = barcode;

                                var otherSellerCamp = test1.SelectSingleNode("//*[@class='pr-mc-w gnr-cnt-br']/div/div/div[3]/div/span");

                                if (otherSellerCamp == null)
                                {
                                    product.tyseller.Add(new TYSELLER
                                    {
                                        SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]").Attributes["href"].Value),
                                        SellerName = StringOperations.delnumb(test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]/div[2]//div[@class='pr-mb-mn']").InnerText),
                                        LPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-slg')]").InnerText : "Düşük Fiyat Bulunamadı",
                                        HPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]").InnerText : test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[@class='prc-slg']").InnerText,
                                        KPrice = "Kampanya Bulunamadı"

                                    });
                                }
                                else
                                {
                                    //diğer satıcılarda yüksek price(üzeri çizili) yoksa
                                    var hpriceIfExist = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]");

                                    if (hpriceIfExist != null)
                                    {
                                        product.tyseller.Add(new TYSELLER
                                        {
                                            SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]").Attributes["href"].Value),
                                            SellerName = StringOperations.delnumb(test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]/div[2]//div[@class='pr-mb-mn']").InnerText),
                                            LPrice = otherSellerCamp.InnerText,
                                            HPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]").InnerText : test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[@class='prc-slg']").InnerText,
                                            KPrice = "Kampanya Bulunamadı"

                                        });
                                    }
                                    else
                                    {

                                        product.tyseller.Add(new TYSELLER
                                        {
                                            SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]").Attributes["href"].Value),
                                            SellerName = StringOperations.delnumb(test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]/div[2]//div[@class='pr-mb-mn']").InnerText),
                                            LPrice = otherSellerCamp.InnerText,
                                            HPrice = test1.SelectSingleNode("//*[@class='pr-mc-w gnr-cnt-br']/div/div/div/div/span") != null ? test1.SelectSingleNode("//*[@class='pr-mc-w gnr-cnt-br']/div/div/div/div/span").InnerText : "Yüksek Fiyat Bulunamadı",
                                        KPrice = "Kampanya Bulunamadı"

                                        });
                                    }

                                   
                                }

                            }
                            catch (Exception)
                            {
                                product.tyseller.Add(new TYSELLER
                                {
                                    SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value),
                                    SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText,


                                    LPrice = "Düşük Fiyat Bulunamadı",

                                    HPrice = "Yüksek Fiyat Bulunamadı",

                                    KPrice = "Kampanya Bulunamadı"

                                });

                                //product.ProductName = test1.SelectSingleNode("//span[contains(@class,'pr-nm')]") != null ? test1.SelectSingleNode("//span[contains(@class,'pr-nm')]").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/h1/span").InnerText;
                                product.ProductName = test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]") != null ? test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]/span").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/h1/span").InnerText;
                                product.ProductURL = headPurl + _tyPurl.Trim();
                                product.PBarcode = barcode;

                                product.tyseller.Add(new TYSELLER
                                {
                                    SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]").Attributes["href"].Value),
                                    SellerName = StringOperations.delnumb(test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]/div[2]//div[@class='pr-mb-mn']").InnerText),
                                    LPrice = "Düşük Fiyat Bulunamadı",

                                    HPrice = "Yüksek Fiyat Bulunamadı",

                                    KPrice = "Kampanya Bulunamadı"
                                });

                            }

                        }
                        else
                        {   //Kamp varsa birinci satıcıdan çekilen veriler
                            product.tyseller.Add(new TYSELLER
                            {
                                SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//div[@class='sl-nm']/a").Attributes["href"].Value),
                                SellerName = test1.SelectSingleNode("//div[@class='sl-nm']/a").InnerText,

                                LPrice = test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div/div/div[1]/div/span[2]") != null ? test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[3]/div/div/div[1]/div/span[2]").InnerText : test1.SelectSingleNode("//div[@class='pr-bx-nm-dsc']//span[@class='prc-slg prc-slg-w-dsc']") == null ? "Düşük Fiyat Bulunamadı" : test1.SelectSingleNode("//div[@class='pr-bx-nm-dsc']//span[@class='prc-slg prc-slg-w-dsc']").InnerText,
                                HPrice = test1.SelectSingleNode("//div[@class='pr-bx-nm-dsc']//span[@class='prc-org']") != null ? test1.SelectSingleNode("//div[@class='pr-bx-nm-dsc']//span[@class='prc-org']").InnerText : test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-org']") == null ? "Yüksek Fiyat Bulunamadı" : test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-org']").InnerText,


                                KPrice = test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-dsc']") != null ? test1.SelectSingleNode("//div[@class='pr-in-cn']//span[@class='prc-dsc']").InnerText : "Kampanyalı Fiyat Bulunamadı",

                            });

                            //product.ProductName = test1.SelectSingleNode("//span[contains(@class,'pr-nm')]") != null ? test1.SelectSingleNode("//span[contains(@class,'pr-nm')]").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/h1/span").InnerText;
                            product.ProductName = test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]") != null ? test1.SelectSingleNode("//h1[contains(@class,'pr-new-br')]/span").InnerText : test1.SelectSingleNode("//*[@id='product-detail-app']/div/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/h1/span").InnerText;
                            product.ProductURL = headPurl + _tyPurl.Trim();
                            product.PBarcode = barcode;

                            //(üstü çizili) Yüksek fiyat yoksa 
                            var hpriceifExcept = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]");

                            if (hpriceifExcept!=null)
                            {
                                product.tyseller.Add(new TYSELLER
                                {
                                    SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]").Attributes["href"].Value),
                                    SellerName = StringOperations.delnumb(test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]/div[2]//div[@class='pr-mb-mn']").InnerText),
                                    LPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-slg')]").InnerText : "Düşük Fiyat Bulunamadı",
                                    HPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]").InnerText : test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[@class='prc-slg']").InnerText,
                                    KPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-pr-dsc']/div/span") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-pr-dsc']/div/span").InnerText : "Kampanyalı Fiyat Bulunamadı",
                                });
                            }
                            else
                            {
                              
                                //Sonrasında bir hata ile karşılaşırlırsa LPrice 'ın null 'a eşit durumunu != değiş
                                product.tyseller.Add(new TYSELLER
                                {
                                    SellerURL = StringOperations.slugnbsp(headPurl + test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]").Attributes["href"].Value),
                                    SellerName = StringOperations.delnumb(test1.SelectSingleNode("//a[contains(@class,'pr-mc-w')]/div[2]//div[@class='pr-mb-mn']").InnerText),
                                    LPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-org')]") == null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-w']//div//span[contains(@class,'prc-slg')]").InnerText : "Düşük Fiyat Bulunamadı",
                                    HPrice = "Yüksek fiyat bulunamadı",
                                    KPrice = test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-pr-dsc']/div/span") != null ? test1.SelectSingleNode("//div[@class='pr-omc']//div[@class='pr-bx-pr-dsc']/div/span").InnerText : "Kampanyalı Fiyat Bulunamadı",
                                });
                            }


                            
                        }


                        return product;
                    }

                }

            }



            return default(TYVM);
            }
            catch (Exception)
            {

                var tySeller = new List<TYSELLER>();
                TYSELLER errData = new TYSELLER()
                {
                    HPrice = "Server Err",
                    KPrice = "Server Err",
                    LPrice = "Server Err",
                    SellerName = "Server Err",
                    SellerURL = "Server Err"
                };
                tySeller.Add(errData);
                
                return new TYVM() { PBarcode = barcode, ProductName = "Err", ProductURL = "err",tyseller= tySeller };
            }
        }
    }
}