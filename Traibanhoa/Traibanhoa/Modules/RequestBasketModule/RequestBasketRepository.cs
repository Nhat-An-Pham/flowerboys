using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.RequestBasketModule
{
    public class RequestBasketRepository : Repository<RequestBasket>, IRequestBasketRepository
    {
        private readonly TraibanhoaContext _db;

        public RequestBasketRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<RequestBasket>> GetRequestBasketsBy(
            Expression<Func<RequestBasket, bool>> filter = null,
            Func<IQueryable<RequestBasket>, ICollection<RequestBasket>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<RequestBasket> query = DbSet;

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
