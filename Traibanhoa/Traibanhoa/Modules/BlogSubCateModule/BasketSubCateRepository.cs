using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Repository.Utils.Repository;
using Traibanhoa.Modules.BlogSubCateModule.Interface;

namespace Traibanhoa.Modules.BlogSubCateModule
{
    public class BasketSubCateRepository : Repository<BasketSubCate>, IBasketSubCateRepository
    {
        private readonly TraibanhoaContext _db;

        public BasketSubCateRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<BasketSubCate>> GetBlogSubCatesBy(
            Expression<Func<BasketSubCate, bool>> filter = null,
            Func<IQueryable<BasketSubCate>, ICollection<BasketSubCate>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<BasketSubCate> query = DbSet;

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
