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
        //public Task<ICollection<Customer>> GetCustomersByUser();

        public Task<Guid?> AddNewCustomer(CreateCustomerRequest CustomerCreate);

        public Task UpdateCustomer(UpdateCustomerRequest CustomerUpdate);

        public Task DeleteCustomer(Guid? customerDeleteID);

        public Task<ICollection<Customer>> GetAll();

        public Task<Customer> GetCustomerByID(Guid? id);
    }
}
