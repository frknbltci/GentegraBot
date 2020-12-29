using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genbot.UI.Models
{
    public class UserVM
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool Role { get; set; }

    }
}