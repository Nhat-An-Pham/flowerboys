using FluentValidation;
using FluentValidation.Results;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.TypeModule.Interface;
using Traibanhoa.Modules.TypeModule.Request;
using Traibanhoa.Modules.TypeModule.Response;
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


        public async Task<ResponseReturnEnum> AddNewType(CreateTypeRequest typeRequest)
        {
            ValidationResult result = new CreateTypeRequestValidator().Validate(typeRequest);
            if (!result.IsValid)
            {
                return ResponseReturnEnum.BAD_REQUEST;
            }

            var newType = new Type();

            newType.TypeId = Guid.NewGuid();
            newType.Name = typeRequest.Name;
            newType.Description = typeRequest.Description;
            newType.Status = true;

            await _typeRepository.AddAsync(newType);
            return ResponseReturnEnum.OK;
        }

        public async Task<ResponseReturnEnum> UpdateType(UpdateTypeRequest typeRequest)
        {
            var typeUpdate = GetTypeByID(typeRequest.TypeId).Result;

            if (typeUpdate == null)
            {
                return ResponseReturnEnum.NOT_FOUND;
            }

            ValidationResult result = new UpdateTypeRequestValidator().Validate(typeRequest);
            if (!result.IsValid)
            {
                return ResponseReturnEnum.BAD_REQUEST;
            }

            typeUpdate.Name = typeRequest.Name;
            typeUpdate.Description = typeRequest.Description;
            typeUpdate.Status = typeRequest.Status;

            await _typeRepository.UpdateAsync(typeUpdate);
            return ResponseReturnEnum.OK;
        }

        public async Task<ResponseReturnEnum> DeleteType(Guid? typeDeleteId)
        {
            if (typeDeleteId == null)
            {
                return ResponseReturnEnum.BAD_REQUEST;
            }

            Type typeDelete = _typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == typeDeleteId).Result;

            if (typeDelete == null)
            {
                return ResponseReturnEnum.NOT_FOUND;
            }

            typeDelete.Status = false;
            await _typeRepository.UpdateAsync(typeDelete);
            
            return ResponseReturnEnum.OK;
        }

        public async Task<Type> GetTypeByID(Guid? id)
        {
            return await _typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == id);
        }

        public async Task<ICollection<TypeDropdownResponse>> GetTypeDropdown()
        {
            var result = await _typeRepository.GetTypesBy(x => x.Status == true);
            return result.Select(x => new TypeDropdownResponse
            {
                TypeId = x.TypeId,
                TypeName = x.Name
            }).ToList();
        }
    }
}
