using Repository.Utils.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Type = Models.Models.Type;

namespace Traibanhoa.Modules.TypeModule.Interface
{
    public interface ITypeRepository : IRepository<Type>
    {
        public Task<ICollection<Type>> GetTypesBy(
               Expression<Func<Type, bool>> filter = null,
               Func<IQueryable<Type>, ICollection<Type>> options = null,
               string includeProperties = null
           );
    }
}
