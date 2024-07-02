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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        //public IReadOnlyList<Product> Search(string SearchValue = "")
        //{
        //    IReadOnlyList<Product> products;

        //    if (SearchValue.ToLower() == "active")
        //    {
        //        products = _context.Products.Where(x => x.Status == true).Select(x => x.Category.Name).ToList();


        //    }
        //    else if (SearchValue.ToLower() == "inactive")
        //    {
        //        products = _context.Products.Where(x => x.Status == false).Include(x => x.Category).Select(x => x.Category.Name).ToList();
        //    }
        //    else
        //    {
        //        products = _context.Products.Where(c => c.Name.Contains(SearchValue)).ToList();

        //    }
        //    return products;
        //}

        public void Update(Product product)
        {
            var Productbyid=_context.Products.FirstOrDefault(pro=>pro.Id==product.Id);
            if (Productbyid!=null)
            {
                Productbyid.Name = product.Name;
                Productbyid.Status = product.Status;
                Productbyid.CategoryId = product.CategoryId;
                Productbyid.ImageUrl = product.ImageUrl;
            }
        }
    }
}
