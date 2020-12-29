using Genbot.DAL.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Genbot.BLL.Repository.Base
{
    public class BaseRepository<T> where T : class
    {
        private GenbotDBEntities db;
        protected DbSet<T> table;

        public BaseRepository()
        {
            db = new GenbotDBEntities();
            table = db.Set<T>();
        }

        public int Save()
        {
            return db.SaveChanges();
        }
        public virtual void Insert(T entity)
        {
            table.Add(entity);
            Save();
        }
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return table.Any(predicate);
        }
        public List<T> Queryable(Expression<Func<T, bool>> predicate)
        {
            return table.Where(predicate).ToList();
        }

        public List<T> GetAll()
        {
            return table.ToList();
        }

        public T Find(long ID)
        {
            return table.Find(ID);
        }

    }
}
