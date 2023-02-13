using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.BasketDetailModule
{
    public class BasketDetailRepository : Repository<BasketDetail>, IBasketDetailRepository
    {
        private readonly TraibanhoaContext _db;

        public BasketDetailRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<BasketDetail>> GetBasketDetailsBy(
            Expression<Func<BasketDetail, bool>> filter = null,
            Func<IQueryable<BasketDetail>, ICollection<BasketDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<BasketDetail> query = DbSet;

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
