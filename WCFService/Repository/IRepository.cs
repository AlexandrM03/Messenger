using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.Repository
{
    interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(int id);
        void Add(T item);
        void Remove(int id);
        void Update(T item);
    }
}
