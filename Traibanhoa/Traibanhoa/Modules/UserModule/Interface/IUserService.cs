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
        public Task<ICollection<User>> GetUsersBy(
            Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, ICollection<User>> options = null,
            string includeProperties = null);

        public Task<Boolean> AddNewUser(CreateUserRequest UserCreate);

        public Task<Boolean> UpdateUser(UpdateUserRequest UserUpdate);

        public Task<Boolean> DeleteUser(User UserDelete);

        public Task<ICollection<User>> GetAll();

        public Task<User> GetUserByID(Guid? id);
    }
}
