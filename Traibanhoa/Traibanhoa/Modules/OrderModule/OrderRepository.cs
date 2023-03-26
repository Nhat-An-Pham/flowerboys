using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.OrderModule
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly TraibanhoaContext _db;

        public OrderRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<Order>> GetOrdersBy(
            Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, ICollection<Order>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Order> query = DbSet;

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

        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction Transaction()
        {
            var transaction = _db.Database.BeginTransaction();
            return transaction;
        }
    }
}
