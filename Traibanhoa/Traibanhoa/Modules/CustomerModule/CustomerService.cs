using FluentValidation.Results;
using Models.Constant;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traibanhoa.Modules.CustomerModule.Interface;
using Traibanhoa.Modules.CustomerModule.Request;

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
            return await _CustomerRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }

        //public Task<ICollection<Customer>> GetCustomersByUser()
        //{
        //    return _CustomerRepository.GetCustomersBy(x => x.IsBlocked == false, options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        //}


        public async Task<Guid?> AddNewCustomer(CreateCustomerRequest CustomerRequest)
        {
            ValidationResult result = new CreateCustomerRequestValidator().Validate(CustomerRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newCustomer = new Customer();
            
            newCustomer.CustomerId= Guid.NewGuid();
            newCustomer.Username = CustomerRequest.Username;
            newCustomer.Name = CustomerRequest.Name;
            newCustomer.Email = CustomerRequest.Email;
            newCustomer.Password = CustomerRequest.Password;
            newCustomer.Phonenumber = CustomerRequest.Phonenumber;
            newCustomer.Gender = CustomerRequest.Gender;
            newCustomer.Avatar = CustomerRequest.Avatar;
            newCustomer.CreatedDate = DateTime.Now;
            newCustomer.UpdatedDate = DateTime.Now;
            newCustomer.IsGoogle = CustomerRequest.IsGoogle;
            newCustomer.IsBlocked = false;

            await _CustomerRepository.AddAsync(newCustomer);
            return newCustomer.CustomerId;
        }

        public async Task UpdateCustomer(UpdateCustomerRequest CustomerRequest)
        {
            try
            {
                var CustomerUpdate = GetCustomerByID(CustomerRequest.CustomerId).Result;

                if (CustomerUpdate == null)
                {
                    throw new Exception(ErrorMessage.CustomerError.CUSTOMER_NOT_FOUND);
                }

                ValidationResult result = new UpdateCustomerRequestValidator().Validate(CustomerRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                CustomerUpdate.Username = CustomerRequest.Username;
                CustomerUpdate.Name = CustomerRequest.Name;
                CustomerUpdate.Password = CustomerRequest.Password;
                CustomerUpdate.Phonenumber = CustomerRequest.Phonenumber;
                CustomerUpdate.Avatar = CustomerRequest.Avatar;
                CustomerUpdate.IsBlocked = CustomerRequest.IsBlocked;
                CustomerUpdate.UpdatedDate = DateTime.Now;

                await _CustomerRepository.UpdateAsync(CustomerUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteCustomer(Guid? customerDeleteID)
        {
            try
            {
                if (customerDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Customer customerDelete = _CustomerRepository.GetFirstOrDefaultAsync(x => x.CustomerId == customerDeleteID && x.IsBlocked == false).Result;

                if (customerDelete == null)
                {
                    throw new Exception(ErrorMessage.CustomerError.CUSTOMER_NOT_FOUND);
                }

                customerDelete.IsBlocked = true;
                await _CustomerRepository.UpdateAsync(customerDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Customer> GetCustomerByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var customer = await _CustomerRepository.GetFirstOrDefaultAsync(x => x.CustomerId == id);
            if (customer == null)
            {
                throw new Exception(ErrorMessage.CustomerError.CUSTOMER_NOT_FOUND);
            }
            return customer;
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
