using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.UserModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.UserModule
{
    public class BasketRepository : Repository<User>, IBasketRepository
    {
        private readonly TraibanhoaContext _db;

        public BasketRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<User>> GetUsersBy(
            Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, ICollection<User>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<User> query = DbSet;

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
