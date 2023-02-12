using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.CustomerModule.Interface;
using Repository.Utils.Repository;

namespace Traibanhoa.Modules.CustomerModule
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly TraibanhoaContext _db;

        public CustomerRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<Customer>> GetCustomersBy(
            Expression<Func<Customer, bool>> filter = null,
            Func<IQueryable<Customer>, ICollection<Customer>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Customer> query = DbSet;

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
