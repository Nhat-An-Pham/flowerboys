using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.UserModule.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<ICollection<User>> GetUsersBy(
               Expression<Func<User, bool>> filter = null,
               Func<IQueryable<User>, ICollection<User>> options = null,
               string includeProperties = null
           );
    }
}
