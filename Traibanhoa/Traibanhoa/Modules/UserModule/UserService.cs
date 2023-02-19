using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.UserModule.Interface;
using Models.Models;
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
            return await _UserRepository.GetAll();
        }

        public Task<ICollection<User>> GetUsersBy(
                Expression<Func<User,
                bool>> filter = null,
                Func<IQueryable<User>,
                ICollection<User>> options = null,
                string includeProperties = null)
        {
            return _UserRepository.GetUsersBy(filter);
        }


        public async Task<Boolean> AddNewUser(CreateUserRequest UserRequest)
        {
            var newUser = new User();

            if(_UserRepository.GetFirstOrDefaultAsync(x => x.UserId == UserRequest.UserId) == null)
            {
                return false;
            }
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
            newUser.UpdatedDate = null;
            newUser.IsGoogle = UserRequest.IsGoogle;
            newUser.IsBlocked = UserRequest.IsBlocked;

            await _UserRepository.AddAsync(newUser);
            return true;
        }

        public async Task<Boolean> UpdateUser(UpdateUserRequest userRequest)
        {
            var userUpdate = GetUserByID(userRequest.UserId).Result;

            userUpdate.Username = userRequest.Username;
            userUpdate.Name = userRequest.Name;
            userUpdate.Password = userRequest.Password;
            userUpdate.Phonenumber = userRequest.Phonenumber;
            userUpdate.Avatar = userRequest.Avatar;
            userUpdate.IsBlocked = userRequest.IsBlocked;
            userUpdate.UpdatedDate = DateTime.Now;

            await _UserRepository.UpdateAsync(userUpdate);
            return true;
        }

        public async Task<Boolean> DeleteUser(User userDelete)
        {
            userDelete.IsBlocked = true;
            await _UserRepository.UpdateAsync(userDelete);
            return true;
        }

        public async Task<User> GetUserByID(Guid? id)
        {
            return await _UserRepository.GetFirstOrDefaultAsync(x => x.UserId == id);
        }
    }
}
