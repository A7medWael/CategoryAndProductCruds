﻿using Cores.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cores.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        void Update(Product product);
        //IReadOnlyList<Product> Search(string SearchValue = "");

    }
}
