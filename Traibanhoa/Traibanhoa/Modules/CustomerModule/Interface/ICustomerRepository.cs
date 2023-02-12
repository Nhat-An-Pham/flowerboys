using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.CustomerModule.Interface
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Task<ICollection<Customer>> GetCustomersBy(
               Expression<Func<Customer, bool>> filter = null,
               Func<IQueryable<Customer>, ICollection<Customer>> options = null,
               string includeProperties = null
           );
    }
}
