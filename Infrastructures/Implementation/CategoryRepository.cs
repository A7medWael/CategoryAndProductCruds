using Cores.Entities;
using Cores.Interfaces;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        

        public void Update(Category category)
        {
            var categorybyid=_context.Categories.FirstOrDefault(c=>c.Id==category.Id);
            if (categorybyid!=null)
            {
                categorybyid.Name= category.Name;
                categorybyid.Status= category.Status;
            }
        }

       public IReadOnlyList<Category>Search(string SearchValue="")
        {
            IReadOnlyList<Category> categories;

            if (SearchValue.ToLower() == "active")
            {
                categories = _context.Categories.Where(x => x.Status == true).ToList();


            }
            else if (SearchValue.ToLower() == "inactive")
            {
                categories = _context.Categories.Where(x => x.Status == false).ToList();

            }
            else
            {
                categories = _context.Categories.Where(c => c.Name.Contains(SearchValue)).ToList();

            }
            return categories;
        }

        public void AddActiveOrInActive(Category category)
        {
            var categorybyid = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (categorybyid != null)
            {
                categorybyid.Name = category.Name;
                categorybyid.Status = category.Status;
            }
           
        }
    }
}
