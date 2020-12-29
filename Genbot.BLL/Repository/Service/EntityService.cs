using Genbot.BLL.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genbot.BLL.Repository.Service
{
    public class EntityService
    {
        public EntityService()
        {
            _usersService = new UsersRepository();
        }

        private UsersRepository _usersService;

        public UsersRepository UsersService
        {
            get { return _usersService; }
            set { _usersService = value; }
        }


    }
}
