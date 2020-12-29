using Genbot.UI.Assets.Custom;
using Genbot.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace Genbot.UI.Attribute
{
    public class LicenceCheck
    {
        public bool LicenceChe(string urlcheck)
        {
            List<LicSearchVM> vm = new List<LicSearchVM>();

            SqlCommand cmd = new SqlCommand("select * from Lisans", Connectbag.bagi);
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                LicSearchVM vm2 = new LicSearchVM()
                {
                    ID = Convert.ToInt32(rdr["ID"].ToString()),
                    URL = rdr["URL"].ToString(),
                    EndDate = (DateTime)rdr["EndDate"],
                    StartDate = (DateTime)rdr["StartDate"],
                    IsActive = (bool)rdr["IsActive"]
                };
                vm.Add(vm2);
            }
            rdr.Close();

            for (int i = 0; i < vm.Count; i++)
            {
                if (vm[i].URL.Contains(urlcheck))
                {
                    if (vm[i].EndDate >= DateTime.UtcNow && vm[i].IsActive == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}