using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.TypeModule.Interface;
using Traibanhoa.Modules.TypeModule.Request;
using Traibanhoa.Modules.TypeModule.Response;
using Traibanhoa.Modules.ProductModule.Interface;
using Models.Models;
using Traibanhoa.Modules.ProductModule.Request;
using Type = Models.Models.Type;
using Traibanhoa.Modules.TypeModule;

namespace Traibanhoa.Modules.ProductModule
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITypeRepository _typeRepository;
        public ProductService(IProductRepository productRepository, ITypeRepository typeRepository)
        {
            _productRepository = productRepository;
            _typeRepository = typeRepository;
        }

        public async Task<ICollection<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public Task<ICollection<Product>> GetProductsBy(
                Expression<Func<Product,
                bool>> filter = null,
                Func<IQueryable<Product>,
                ICollection<Product>> options = null,
                string includeProperties = null)
        {
            return _productRepository.GetProductsBy(filter);
        }


        public async Task<Boolean> AddNewProduct(CreateProductRequest productRequest)
        {
            var newProduct = new Product();

            if(_typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == productRequest.TypeId) == null)
            {
                return false;
            }
            newProduct.TypeId = Guid.NewGuid();
            newProduct.Name = productRequest.Name;
            newProduct.Description = productRequest.Description;
            newProduct.Status = true;

            await _productRepository.AddAsync(newProduct);
            return true;
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
            typeDelete.Status = false;
            await _typeRepository.UpdateAsync(typeDelete);
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
