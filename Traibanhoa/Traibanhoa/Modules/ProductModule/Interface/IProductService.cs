using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.TypeModule.Request;
using Traibanhoa.Modules.TypeModule.Response;
using Models.Models;
using Traibanhoa.Modules.ProductModule.Request;

namespace Traibanhoa.Modules.ProductModule.Interface
{
    public interface IProductService
    {
        public Task<ICollection<Product>> GetProductsBy(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, ICollection<Product>> options = null,
            string includeProperties = null);

        public Task<Boolean> AddNewProduct(CreateProductRequest productCreate);

        public Task<Boolean> UpdateProduct(UpdateProductRequest productUpdate);

        public Task<Boolean> DeleteProduct(Product productDelete);

        public Task<ICollection<Product>> GetAll();

        public Task<Product> GetProductByID(Guid? id);
    }
}
