using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.TypeModule.Request;
using Traibanhoa.Modules.TypeModule.Response;
using Models.Models;
using Traibanhoa.Modules.ProductModule.Request;
using Traibanhoa.Modules.ProductModule.Response;
using Microsoft.AspNetCore.Mvc;

namespace Traibanhoa.Modules.ProductModule.Interface
{
    public interface IProductService
    {
        public Task<ICollection<GetProductResponse>> GetProductsForCustomer(bool? forSelling);

        public Task<Guid?> AddNewProduct(CreateProductRequest productCreate);

        public Task UpdateProduct(UpdateProductRequest productUpdate);

        public Task DeleteProduct(Guid? productDeleteId);

        public Task<ICollection<GetProductResponse>> GetAll(bool? forSelling);

        public Task<GetProductResponse> GetProductByID(Guid? id);

        public Task<ICollection<SearchProductResponse>> GetProductByName(string name);

    }
}
