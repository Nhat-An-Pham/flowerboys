using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.TypeModule.Request;
using Traibanhoa.Modules.TypeModule.Response;
using Type = Models.Models.Type;

namespace Traibanhoa.Modules.TypeModule.Interface
{
    public interface ITypeService
    {
        public Task<ICollection<Type>> GetTypesForCustomer();

        public Task<Guid?> AddNewType(CreateTypeRequest typeCreate);

        public Task UpdateType(UpdateTypeRequest typeUpdate);

        public Task DeleteType(Guid? typeDeleteId);

        public Task<ICollection<Type>> GetAll();

        public Task<Type> GetTypeByID(Guid? id);
        public Task<ICollection<TypeDropdownResponse>> GetTypeDropdown();
    }
}
