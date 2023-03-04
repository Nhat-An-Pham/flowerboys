using FluentValidation.Results;
using Models.Constant;
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
            return await _typeRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }

        public Task<ICollection<Type>> GetTypesForCustomer()
        {
            return _typeRepository.GetTypesBy(x => x.Status == true, options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }


        public async Task<Guid?> AddNewType(CreateTypeRequest typeRequest)
        {
            try
            {
                ValidationResult result = new CreateTypeRequestValidator().Validate(typeRequest);
                if (!result.IsValid)
                {
                    return null;
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                var newType = new Type();

                newType.TypeId = Guid.NewGuid();
                newType.Name = typeRequest.Name;
                newType.Description = typeRequest.Description;
                newType.CreatedDate = DateTime.Now;
                newType.UpdatedDate = DateTime.Now;
                newType.Status = true;

                await _typeRepository.AddAsync(newType);
                return newType.TypeId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateType(UpdateTypeRequest typeRequest)
        {
            try
            {
                var typeUpdate = GetTypeByID(typeRequest.TypeId).Result;

                if (typeUpdate == null)
                {
                    throw new Exception(ErrorMessage.TypeError.TYPE_NOT_FOUND);
                }

                ValidationResult result = new UpdateTypeRequestValidator().Validate(typeRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                typeUpdate.Name = typeRequest.Name;
                typeUpdate.Description = typeRequest.Description;
                typeUpdate.UpdatedDate = DateTime.Now;
                typeUpdate.Status = typeRequest.Status;

                await _typeRepository.UpdateAsync(typeUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task DeleteType(Guid? typeDeleteId)
        {
            try
            {
                if (typeDeleteId == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Type typeDelete = _typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == typeDeleteId && x.Status == true).Result;

                if (typeDelete == null)
                {
                    throw new Exception(ErrorMessage.TypeError.TYPE_NOT_FOUND);
                }

                typeDelete.Status = false;
                await _typeRepository.UpdateAsync(typeDelete);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Type> GetTypeByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var type = await _typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == id);
            if (type == null)
            {
                throw new Exception(ErrorMessage.TypeError.TYPE_NOT_FOUND);
            }
            return type;
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
