using FluentValidation.Results;
using Models.Constant;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traibanhoa.Modules.UserModule.Interface;
using Traibanhoa.Modules.UserModule.Request;

namespace Traibanhoa.Modules.UserModule
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        public UserService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await _UserRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }

        public Task<ICollection<User>> GetUsersBy()
        {
            return _UserRepository.GetUsersBy(x => x.IsBlocked == false, options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }


        public async Task<Guid?> AddNewUser(CreateUserRequest UserRequest)
        {

            ValidationResult result = new CreateUserRequestValidator().Validate(UserRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }
            var newUser = new User();

            newUser.UserId= Guid.NewGuid();
            newUser.Username = UserRequest.Username;
            newUser.Name = UserRequest.Name;
            newUser.Email = UserRequest.Email;
            newUser.Password = UserRequest.Password;
            newUser.Phonenumber = UserRequest.Phonenumber;
            newUser.Gender = UserRequest.Gender;
            newUser.Avatar = UserRequest.Avatar;
            newUser.Role = UserRequest.Role;
            newUser.CreatedDate = DateTime.Now;
            newUser.UpdatedDate = DateTime.Now;
            newUser.IsGoogle = UserRequest.IsGoogle;
            newUser.IsBlocked = false;

            await _UserRepository.AddAsync(newUser);
            return newUser.UserId;
        }

        public async Task UpdateUser(UpdateUserRequest userRequest)
        {
            try
            {
                var userUpdate = GetUserByID(userRequest.UserId).Result;

                if (userUpdate == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                ValidationResult result = new UpdateUserRequestValidator().Validate(userRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                userUpdate.Username = userRequest.Username;
                userUpdate.Name = userRequest.Name;
                userUpdate.Password = userRequest.Password;
                userUpdate.Phonenumber = userRequest.Phonenumber;
                userUpdate.Avatar = userRequest.Avatar;
                userUpdate.IsBlocked = userRequest.IsBlocked;
                userUpdate.UpdatedDate = DateTime.Now;

                await _UserRepository.UpdateAsync(userUpdate);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        public async Task DeleteUser(Guid? userDeleteID)
        {
            try
            {
                if (userDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                User userDelete = _UserRepository.GetFirstOrDefaultAsync(x => x.UserId == userDeleteID && x.IsBlocked == false).Result;

                if (userDelete == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                userDelete.IsBlocked = true;
                await _UserRepository.UpdateAsync(userDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<User> GetUserByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return user;
        }
    }
}
