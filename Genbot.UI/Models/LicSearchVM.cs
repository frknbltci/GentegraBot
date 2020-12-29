using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genbot.UI.Models
{
    public class LicSearchVM
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }


    }
}