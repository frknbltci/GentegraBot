using Genbot.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genbot.BLL.Repository.Entity
{
    public class UsersRepository : Base.BaseRepository<Users>
    {
        public Users FindUserName(string username)
        {
            return table.FirstOrDefault(x => x.UserName == username);
        }

        public bool login(string username, string password)
        {
            bool rtn = table.Any(x => x.UserName == username && x.Password == password);

            if (rtn == false)
            {
                return false;
            }
            else return true;
        }


        public void Update(Users gelen)
        {
            var bul = Find(gelen.ID);
            bul.IsActive = (bool)gelen.IsActive;
            bul.Role = (bool)gelen.Role;
            bul.UserName = gelen.UserName;
            bul.Password = gelen.Password;
            Save();
        }
    }
}
