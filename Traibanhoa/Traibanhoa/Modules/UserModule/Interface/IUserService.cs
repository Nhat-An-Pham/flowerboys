using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.UserModule.Request;
using Models.UserDTO;
using Traibanhoa.Modules.UserModule.Response;

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
        public Customer GetCustomerByUsername(string? username);
        public CurrentUserResponse GetCurrentUser(string authHeader);
        #region Authentication
        public Task<string> GenerateToken(LoginDTO login);
        public Task<string> GenerateGoolgleToken(LoginGoogleDTO loginGoogle);
        public Task Register(RegisterDTO register);
        #endregion
    }
}
