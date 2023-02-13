using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.RequestBasketDetailModule
{
    public class RequestBasketDetailRepository : Repository<RequestBasketDetail>, IRequestBasketDetailRepository
    {
        private readonly TraibanhoaContext _db;

        public RequestBasketDetailRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<RequestBasketDetail>> GetRequestBasketDetailsBy(
            Expression<Func<RequestBasketDetail, bool>> filter = null,
            Func<IQueryable<RequestBasketDetail>, ICollection<RequestBasketDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<RequestBasketDetail> query = DbSet;

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
