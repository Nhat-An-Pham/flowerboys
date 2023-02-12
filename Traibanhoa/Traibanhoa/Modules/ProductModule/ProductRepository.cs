using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.ProductModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.ProductModule
{
    public class UserRepository : Repository<Product>, IUserRepository
    {
        private readonly TraibanhoaContext _db;

        public UserRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<Product>> GetProductsBy(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, ICollection<Product>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Product> query = DbSet;

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
