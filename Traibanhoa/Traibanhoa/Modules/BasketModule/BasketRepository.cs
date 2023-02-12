using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.BasketModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.BasketModule
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        private readonly TraibanhoaContext _db;

        public BasketRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<Basket>> GetBasketsBy(
            Expression<Func<Basket, bool>> filter = null,
            Func<IQueryable<Basket>, ICollection<Basket>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Basket> query = DbSet;

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
