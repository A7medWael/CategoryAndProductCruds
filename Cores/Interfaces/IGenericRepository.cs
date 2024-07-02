using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cores.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? Perdicate=null,string? IncludeWord=null);
        T GetFirstOrDefault(Expression<Func<T,bool>>? Perdicate,string? IncludeWord=null);
        void Add(T entity);
        void Remove(T entity);
    }
}
