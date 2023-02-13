using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderBasketDetailModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.OrderBasketDetailModule
{
    public class OrderBasketDetailRepository : Repository<OrderBasketDetail>, IOrderBasketDetailRepository
    {
        private readonly TraibanhoaContext _db;

        public OrderBasketDetailRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<OrderBasketDetail>> GetOrderBasketDetailsBy(
            Expression<Func<OrderBasketDetail, bool>> filter = null,
            Func<IQueryable<OrderBasketDetail>, ICollection<OrderBasketDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<OrderBasketDetail> query = DbSet;

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
