using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genbot.UI.Models
{
    public class HBVM
    {
        public string PBarcode { get; set; }
        public string ProductName { get; set; }
        public string ProductURL { get; set; }
        public List<HBSELLER> hbseller { get; set; }
    }
    public class HBSELLER
    {
        public string SellerName { get; set; }
        public string SellerURL { get; set; }
        public string KPrice { get; set; }
        public string HPrice { get; set; }
        public string LPrice { get; set; }

    }
}