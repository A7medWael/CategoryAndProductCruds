﻿using Cores.Interfaces;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet=_context.Set<T>();

        }
        public void Add(T entity)
        {
           _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Perdicate = null, string? IncludeWord = null)
        {
           IQueryable<T> query = _dbSet;
            if(Perdicate != null)
            {
                query = query.Where(Perdicate);
            }
            if(IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? Perdicate, string? IncludeWord = null)
        {
            IQueryable<T> query = _dbSet;
            if (Perdicate != null)
            {
                query = query.Where(Perdicate);
            }
            if(IncludeWord != null)
            {
                foreach(var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
