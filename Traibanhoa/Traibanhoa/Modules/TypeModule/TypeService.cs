using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.TypeModule.Interface;
using Traibanhoa.Modules.TypeModule.Request;
using Type = Models.Models.Type;

namespace Traibanhoa.Modules.TypeModule
{
    public class TypeService : ITypeService
    {
        private readonly ITypeRepository _typeRepository;

        public TypeService(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<ICollection<Type>> GetAll()
        {
            return await _typeRepository.GetAll();
        }

        public Task<ICollection<Type>> GetTypesBy(
                Expression<Func<Type,
                bool>> filter = null,
                Func<IQueryable<Type>,
                ICollection<Type>> options = null,
                string includeProperties = null)
        {
            return _typeRepository.GetTypesBy(filter);
        }


        public async Task AddNewType(CreateTypeRequest typeRequest)
        {
            var newType = new Type();

            newType.TypeId = Guid.NewGuid();
            newType.Name = typeRequest.Name;
            newType.Description = typeRequest.Description;
            newType.Status = false;

            await _typeRepository.AddAsync(newType);
        }

        public async Task UpdateType(UpdateTypeRequest typeRequest)
        {
            var typeUpdate = GetTypeByID(typeRequest.TypeId).Result;

            typeUpdate.Name = typeRequest.Name;
            typeUpdate.Description = typeRequest.Description;
            typeUpdate.Status = typeRequest.Status;

            await _typeRepository.UpdateAsync(typeUpdate);
        }

        public async Task DeleteType(Type typeDelete)
        {
            typeDelete.Status = true;
            await _typeRepository.UpdateAsync(typeDelete);
        }

        public async Task<Type> GetTypeByID(Guid? id)
        {
            return await _typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == id);
        }
    }
}
