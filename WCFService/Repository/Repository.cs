using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCFService.DataBase;

namespace WCFService.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private Context context;
        private DbSet<T> dbset;

        public Repository(Context context)
        {
            this.context = context;
            this.dbset = context.Set<T>();
        }
        
        public void Add(T item)
        {
            dbset.Add(item);
        }

        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public List<T> GetAll()
        {
            return dbset.ToList();
        }

        public void Remove(int id)
        {
            dbset.Remove(Get(id));
        }

        public void Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
