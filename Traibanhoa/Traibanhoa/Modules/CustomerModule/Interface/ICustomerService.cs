using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.CustomerModule.Request;

namespace Traibanhoa.Modules.CustomerModule.Interface
{
    public interface ICustomerService
    {
        public Task<ICollection<Customer>> GetCustomersBy(
            Expression<Func<Customer, bool>> filter = null,
            Func<IQueryable<Customer>, ICollection<Customer>> options = null,
            string includeProperties = null);

        public Task<Boolean> AddNewCustomer(CreateCustomerRequest CustomerCreate);

        public Task<Boolean> UpdateCustomer(UpdateCustomerRequest CustomerUpdate);

        public Task<Boolean> DeleteCustomer(Customer CustomerDelete);

        public Task<ICollection<Customer>> GetAll();

        public Task<Customer> GetCustomerByID(Guid? id);
    }
}
