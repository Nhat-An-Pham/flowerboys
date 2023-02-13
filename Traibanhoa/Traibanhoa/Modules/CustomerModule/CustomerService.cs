using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.CustomerModule.Request;
using Traibanhoa.Modules.CustomerModule.Interface;

namespace Traibanhoa.Modules.CustomerModule
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _CustomerRepository;
        public CustomerService(ICustomerRepository CustomerRepository)
        {
            _CustomerRepository = CustomerRepository;
        }

        public async Task<ICollection<Customer>> GetAll()
        {
            return await _CustomerRepository.GetAll();
        }

        public Task<ICollection<Customer>> GetCustomersBy(
                Expression<Func<Customer,
                bool>> filter = null,
                Func<IQueryable<Customer>,
                ICollection<Customer>> options = null,
                string includeProperties = null)
        {
            return _CustomerRepository.GetCustomersBy(filter);
        }


        public async Task<Boolean> AddNewCustomer(CreateCustomerRequest CustomerRequest)
        {
            var newCustomer = new Customer();

            if(_CustomerRepository.GetFirstOrDefaultAsync(x => x.CustomerId == CustomerRequest.CustomerId) == null)
            {
                return false;
            }
            newCustomer.CustomerId= Guid.NewGuid();
            newCustomer.Username = CustomerRequest.Username;
            newCustomer.Name = CustomerRequest.Name;
            newCustomer.Email = CustomerRequest.Email;
            newCustomer.Password = CustomerRequest.Password;
            newCustomer.Phonenumber = CustomerRequest.Phonenumber;
            newCustomer.Gender = CustomerRequest.Gender;
            newCustomer.Avatar = CustomerRequest.Avatar;
            newCustomer.CreatedDate = DateTime.Now;
            newCustomer.UpdatedDate = null;
            newCustomer.IsGoogle = CustomerRequest.IsGoogle;
            newCustomer.IsBlocked = CustomerRequest.IsBlocked;

            await _CustomerRepository.AddAsync(newCustomer);
            return true;
        }

        public async Task<Boolean> UpdateCustomer(UpdateCustomerRequest CustomerRequest)
        {
            var CustomerUpdate = GetCustomerByID(CustomerRequest.CustomerId).Result;

            CustomerUpdate.Username = CustomerRequest.Username;
            CustomerUpdate.Name = CustomerRequest.Name;
            CustomerUpdate.Password = CustomerRequest.Password;
            CustomerUpdate.Phonenumber = CustomerRequest.Phonenumber;
            CustomerUpdate.Avatar = CustomerRequest.Avatar;
            CustomerUpdate.IsBlocked = CustomerRequest.IsBlocked;
            CustomerUpdate.UpdatedDate = DateTime.Now;

            await _CustomerRepository.UpdateAsync(CustomerUpdate);
            return true;
        }

        public async Task<Boolean> DeleteCustomer(Customer CustomerDelete)
        {
            CustomerDelete.IsBlocked = true;
            await _CustomerRepository.UpdateAsync(CustomerDelete);
            return true;
        }

        public async Task<Customer> GetCustomerByID(Guid? id)
        {
            return await _CustomerRepository.GetFirstOrDefaultAsync(x => x.CustomerId == id);
        }

        //public async Task<ICollection<TypeDropdownResponse>> GetTypeDropdown()
        //{
        //    var result = await _typeRepository.GetTypesBy(x => x.Status == true);
        //    return result.Select(x => new TypeDropdownResponse
        //    {
        //        TypeId = x.TypeId,
        //        TypeName = x.Name
        //    }).ToList();
        //}
    }
}
