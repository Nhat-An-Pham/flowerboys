using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.TypeModule.Request;
using Type = Models.Models.Type;

namespace Traibanhoa.Modules.TypeModule.Interface
{
    public interface ITypeService
    {
        public Task<ICollection<Type>> GetTypesBy(
            Expression<Func<Type, bool>> filter = null,
            Func<IQueryable<Type>, ICollection<Type>> options = null,
            string includeProperties = null);

        public Task AddNewType(CreateTypeRequest typeRequest);

        public Task UpdateType(UpdateTypeRequest typeUpdate);

        public Task DeleteType(Type typeDelete);

        public Task<ICollection<Type>> GetAll();

        public Task<Type> GetTypeByID(Guid? id);
    }
}
