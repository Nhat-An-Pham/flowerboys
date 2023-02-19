using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.UserModule.Request;

namespace Traibanhoa.Modules.UserModule.Interface
{
    public interface IUserService
    {
        public Task<ICollection<User>> GetUsersBy();

        public Task<Guid?> AddNewUser(CreateUserRequest UserCreate);

        public Task UpdateUser(UpdateUserRequest UserUpdate);

        public Task DeleteUser(Guid? userDeleteID);

        public Task<ICollection<User>> GetAll();

        public Task<User> GetUserByID(Guid? id);
    }
}
