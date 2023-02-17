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
using FluentValidation.Results;

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


        public async Task<Guid?> AddNewProduct(CreateProductRequest productRequest)
        {
            ValidationResult result = new CreateProductRequestValidator().Validate(productRequest);
            if (!result.IsValid)
            {
                return null;
            }

            var newProduct = new Product();

            if(_typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == productRequest.TypeId) == null)
            {
                return null;
            }
            newProduct.ProductId = Guid.NewGuid();
            newProduct.Name = productRequest.Name;
            newProduct.Description = productRequest.Description;
            newProduct.TypeId = productRequest.TypeId;
            newProduct.Picture = productRequest.Picture;
            newProduct.Price = productRequest.Price;
            newProduct.Status = productRequest.Status ?? true;

            await _productRepository.AddAsync(newProduct);
            return newProduct.ProductId;
        }

        public async Task<bool> UpdateProduct(UpdateProductRequest productRequest)
        {
            var productUpdate = GetProductByID(productRequest.ProductId).Result;

            if(productUpdate == null)
            {
                return false;
            }

            ValidationResult result = new UpdateProductRequestValidator().Validate(productRequest);
            if (!result.IsValid)
            {
                return false;
            }

            if (_typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == productRequest.TypeId) == null)
            {
                return false;
            }

            productUpdate.Name = productRequest.Name ?? productUpdate.Name;
            productUpdate.Description = productRequest.Description ?? productUpdate.Description;
            productUpdate.Status = productRequest.Status ?? productUpdate.Status;
            productUpdate.TypeId = productRequest.TypeId ?? productUpdate.TypeId;
            productUpdate.Picture = productRequest.Picture ?? productUpdate.Picture ;
            productUpdate.Price = productRequest.Price ?? productUpdate.Price ;

            await _productRepository.UpdateAsync(productUpdate);
            return true;
        }

        public async Task<bool> DeleteProduct(Guid? productDeleteId)
        {
            if (productDeleteId == null)
            {
                return false;
            }

            var productDelete = _productRepository.GetFirstOrDefaultAsync(x => x.ProductId == productDeleteId).Result;

            if (productDelete == null)
            {
                return false;
            }

            productDelete.Status = false;
            await _productRepository.UpdateAsync(productDelete);

            return true;
        }

        public async Task<Product> GetProductByID(Guid? id)
        {
            return await _productRepository.GetFirstOrDefaultAsync(x => x.ProductId == id);
        }
    }
}
