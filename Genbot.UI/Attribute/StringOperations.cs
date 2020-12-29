using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Genbot.UI.Attribute
{
    public class StringOperations
    {
        public static string ToSlug(string incomingText)
        {

            incomingText = incomingText.Replace("Parf&#252;m", "parfum");
            incomingText = incomingText.Replace("ş", "s");
            incomingText = incomingText.Replace("Ş", "s");
            incomingText = incomingText.Replace("İ", "i");
            incomingText = incomingText.Replace("I", "i");
            incomingText = incomingText.Replace("ı", "i");
            incomingText = incomingText.Replace("ö", "o");
            incomingText = incomingText.Replace("Ö", "o");
            incomingText = incomingText.Replace("ü", "u");
            incomingText = incomingText.Replace("Ü", "u");
            incomingText = incomingText.Replace("Ç", "c");
            incomingText = incomingText.Replace("ç", "c");
            incomingText = incomingText.Replace("ğ", "g");
            incomingText = incomingText.Replace("Ğ", "g");
            incomingText = incomingText.Replace(" ", "-");
            incomingText = incomingText.Replace("---", "-");
            incomingText = incomingText.Replace("?", "");
            incomingText = incomingText.Replace("/", "");
            incomingText = incomingText.Replace(".", "");
            incomingText = incomingText.Replace("'", "");
            incomingText = incomingText.Replace("#", "");
            incomingText = incomingText.Replace("%", "");
            incomingText = incomingText.Replace("&", "");
            incomingText = incomingText.Replace("*", "");
            incomingText = incomingText.Replace("!", "");
            incomingText = incomingText.Replace("@", "");
            incomingText = incomingText.Replace("+", "");
            incomingText = incomingText.ToLower();
            incomingText = incomingText.Trim();

            // tüm harfleri küçült
            string encodedUrl = (incomingText ?? "").ToLower();

            // & ile " " yer değiştirme
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // " " karakterlerini silme
            encodedUrl = encodedUrl.Replace("'", "");

            // geçersiz karakterleri sil
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

            // tekrar edenleri sil
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // karakterlerin arasına tire koy
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }

        public static string slugnbsp(string incoming)
        {
            incoming = incoming.Replace("&nbsp;", " ");
            incoming = incoming.Replace("Ürün Adı :", "");
            incoming = incoming.Replace("amp;", "");
            incoming = incoming.Replace(" ", "-");


            return incoming;
        }

        public static string delnumb(string incoming)
        {
            incoming = incoming.Replace("1", "");
            incoming = incoming.Replace("2", "");
            incoming = incoming.Replace("3", "");
            incoming = incoming.Replace("4", "");
            incoming = incoming.Replace("5", "");
            incoming = incoming.Replace("6", "");
            incoming = incoming.Replace("7", "");
            incoming = incoming.Replace("8", "");
            incoming = incoming.Replace("9", "");
            incoming = incoming.Replace("10", "");
            incoming = incoming.Replace("0", "");
            incoming = incoming.Replace(".", "");
            return incoming;

        }

    }
}