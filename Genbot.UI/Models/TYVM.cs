using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genbot.UI.Models
{
    public class TYVM
    {
        public string PBarcode { get; set; }
        public string ProductName { get; set; }
        public string ProductURL { get; set; }
        public List<TYSELLER> tyseller { get; set; }
    }

    public class TYSELLER
    {
        public string SellerName { get; set; }
        public string SellerURL { get; set; }
        public string KPrice { get; set; }
        public string HPrice { get; set; }
        public string LPrice { get; set; }

    }
}