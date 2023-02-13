using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderProductDetailModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.OrderProductDetailModule
{
    public class OrderProductDetailRepository : Repository<OrderProductDetail>, IOrderProductDetailRepository
    {
        private readonly TraibanhoaContext _db;

        public OrderProductDetailRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<OrderProductDetail>> GetOrderProductDetailsBy(
            Expression<Func<OrderProductDetail, bool>> filter = null,
            Func<IQueryable<OrderProductDetail>, ICollection<OrderProductDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<OrderProductDetail> query = DbSet;

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
