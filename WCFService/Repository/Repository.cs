using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFService.DataBase;

namespace WCFService.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private Context context;

        public Repository(Context context)
        {
            this.context = context;
        }
        
        public void Add(T item)
        {
            context.Set<T>().Add(item);
        }

        public T Get(int id)
        {
            return context.Set<T>().Find(id);
        }

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public void Remove(int id)
        {
            context.Set<T>().Remove(Get(id));
        }

        public void Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
