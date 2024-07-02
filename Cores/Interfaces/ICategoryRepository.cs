using Cores.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cores.Interfaces
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        void Update (Category category);
       IReadOnlyList<Category> Search (string SearchValue="");
        void AddActiveOrInActive(Category category);
    }
}
