using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genbot.UI.Attribute
{
    public class SessionContext
    {
        public string UserName { get; set; }
        public bool Role { get; set; }
        public bool IsActive { get; set; }
        public string FileURL { get; set; }

    }
}