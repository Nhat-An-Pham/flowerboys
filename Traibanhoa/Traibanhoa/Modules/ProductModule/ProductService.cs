﻿using System.Collections.Generic;
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
using Models.Constant;
using Traibanhoa.Modules.ProductModule.Response;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;

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

        public async Task<ICollection<GetProductResponse>> GetAll(bool? forSelling)
        {
            var result = _productRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList(), includeProperties: "Type").Result.Select(x => new GetProductResponse
            {
                ProductId = x.ProductId,
                Name = x.Name,
                Description = x.Description,
                Picture = x.Picture,
                CreatedDate = x.CreatedDate,
                Price = x.Price,
                TypeId = x.Type.TypeId,
                TypeName = x.Type.Name,
                ForSelling = x.ForSelling
            }).ToList();
            if (result.Count() == 0)
            {
                throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
            }
            if (forSelling == true)
            {
                result = result.Where(x => x.ForSelling == forSelling).ToList();
            } else if (forSelling == false)
            {
                result = result.Where(x => x.ForSelling == forSelling).ToList();
            }
            return result;
        }

        public async Task<ICollection<GetProductResponse>> GetProductsForCustomer(bool? forSelling)
        {
            var result = _productRepository.GetProductsBy(x => x.Status == true, includeProperties: "Type").Result.Select(x => new GetProductResponse
            {
                ProductId = x.ProductId,
                Name = x.Name,
                Description = x.Description,
                Picture = x.Picture,
                CreatedDate = x.CreatedDate,
                Price = x.Price,
                TypeId = x.Type.TypeId,
                TypeName = x.Type.Name,
                ForSelling = x.ForSelling
            }).ToList();
            if (result.Count() == 0)
            {
                throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
            }
            if (forSelling == true)
            {
                result = result.Where(x => x.ForSelling == forSelling).ToList();
            }
            else if (forSelling == false)
            {
                result = result.Where(x => x.ForSelling == forSelling).ToList();
            }
            return result;
        }

        public async Task<Guid?> AddNewProduct(CreateProductRequest productRequest)
        {
            ValidationResult result = new CreateProductRequestValidator().Validate(productRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newProduct = new Product();

            newProduct.ProductId = Guid.NewGuid();
            newProduct.Name = productRequest.Name;
            newProduct.Description = productRequest.Description;
            newProduct.CreatedDate = DateTime.Now;
            newProduct.UpdatedDate = DateTime.Now;
            newProduct.TypeId = productRequest.TypeId;
            newProduct.Picture = productRequest.Picture;
            newProduct.Price = productRequest.Price;
            newProduct.Status = productRequest.Status ?? true;
            newProduct.ForSelling = productRequest.ForSelling;

            await _productRepository.AddAsync(newProduct);
            return newProduct.ProductId;
        }

        public async Task UpdateProduct(UpdateProductRequest productRequest)
        {
            var productUpdate = _productRepository.GetByIdAsync(productRequest.ProductId).Result;

            if(productUpdate == null)
            {
                throw new Exception(ErrorMessage.ProductError.PRODUCT_NOT_FOUND);
            }

            ValidationResult result = new UpdateProductRequestValidator().Validate(productRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            if (_typeRepository.GetFirstOrDefaultAsync(x => x.TypeId == productRequest.TypeId) == null)
            {
                throw new Exception(ErrorMessage.TypeError.TYPE_NOT_FOUND);
            }

            productUpdate.Name = productRequest.Name ?? productUpdate.Name;
            productUpdate.Description = productRequest.Description ?? productUpdate.Description;
            productUpdate.Status = productRequest.Status ?? productUpdate.Status;
            productUpdate.UpdatedDate = DateTime.Now;
            productUpdate.TypeId = productRequest.TypeId ?? productUpdate.TypeId;
            productUpdate.Picture = productRequest.Picture ?? productUpdate.Picture ;
            productUpdate.Price = productRequest.Price ?? productUpdate.Price ;
            productUpdate.ForSelling = productRequest.ForSelling ?? productUpdate.ForSelling ;

            await _productRepository.UpdateAsync(productUpdate);
        }

        public async Task DeleteProduct(Guid? productDeleteId)
        {
            if (productDeleteId == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }

            var productDelete = _productRepository.GetFirstOrDefaultAsync(x => x.ProductId == productDeleteId).Result;

            if (productDelete == null)
            {
                throw new Exception(ErrorMessage.ProductError.PRODUCT_NOT_FOUND);
            }

            productDelete.Status = false;
            await _productRepository.UpdateAsync(productDelete);
        }

        public async Task<GetProductResponse> GetProductByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var product = await _productRepository.GetFirstOrDefaultAsync(x => x.ProductId == id && x.Status == true, includeProperties: "Type");
            if (product == null)
            {
                throw new Exception(ErrorMessage.ProductError.PRODUCT_NOT_FOUND);
            }
            var result = new GetProductResponse()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Picture = product.Picture,
                CreatedDate = product.CreatedDate,
                Price = product.Price,
                TypeId = product.Type.TypeId,
                TypeName = product.Type.Name
            };
            return result;
        }
        public async Task<ICollection<SearchProductResponse>> GetProductByName(String name)
        {

            var products = await _productRepository.GetProductsBy(x => x.Status == true);

            return products.Where(x => ConvertToUnSign(x.Name).Contains(name, StringComparison.CurrentCultureIgnoreCase) || x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList().Select(ToSearchResponse).ToList();
        }

        public SearchProductResponse ToSearchResponse(Product product)
        {
            if (product != null)
            {
                return new SearchProductResponse()
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    ProductPrice = (decimal)product.Price,
                };
            }
            else
            {
                return null;
            }
        }
        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }
    }
}
