using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Utils.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.CartModule.Interface;

namespace Traibanhoa.Modules.CartModule
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly TraibanhoaContext _db;

        public CartRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<Cart>> GetCartsBy(
            Expression<Func<Cart, bool>> filter = null,
            Func<IQueryable<Cart>, ICollection<Cart>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Cart> query = DbSet;

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
