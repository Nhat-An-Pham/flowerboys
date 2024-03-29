﻿using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Utils.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.CategoryModule.Interface;

namespace Traibanhoa.Modules.CategoryModule
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly TraibanhoaContext _db;

        public CategoryRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<Category>> GetCategoriesBy(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, ICollection<Category>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Category> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
