using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Genbot.UI.Assets.Custom
{
    public class Connectbag
    {
        public static SqlConnection bagi = new SqlConnection("Server=185.115.242.12\\MSSQLSERVER2014;Database=veritaban1284;User Id = veritaban1284; Password=yE3Dz3adkudN");
    }
}
