﻿using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.CategoryModule.Interface;
using Traibanhoa.Modules.CategoryModule.Request;
using Traibanhoa.Modules.CategoryModule.Response;
using Traibanhoa.Modules.SubCateModule.Interface;

namespace Traibanhoa.Modules.CategoryModule
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCateRepository _subCateRepository;

        public CategoryService(ICategoryRepository categoryRepository, ISubCateRepository subCateRepository)
        {
            _categoryRepository = categoryRepository;
            _subCateRepository = subCateRepository;
        }
        public async Task<ICollection<Category>> GetAll()
        {
            return await _categoryRepository.GetCategoriesBy(x => x.Status == true, includeProperties: "SubCategories");
        }
        public async Task<ICollection<Category>> GetAllForStaff()
        {
            return await _categoryRepository.GetAll(includeProperties: "SubCategories");
        }
        public Task<ICollection<Category>> GetCategoriesBy(Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, ICollection<Category>> options = null,
            string includeProperties = null)
        {
            return _categoryRepository.GetCategoriesBy(filter);
        }
        public async Task<Guid> AddNewCategory(CreateCategoryRequest categoryRequest)
        {
            var newCategory = new Category();
            newCategory.CategoryId = Guid.NewGuid();
            newCategory.Name = categoryRequest.Name;
            newCategory.Description = categoryRequest.Description;
            newCategory.Status = true;
            newCategory.CreatedDate = DateTime.Now;
            await _categoryRepository.AddAsync(newCategory);

            return newCategory.CategoryId;
        }
        public async Task<bool> UpdateCategory(UpdateCategoryRequest categoryRequest)
        {
            var categoryUpdate = _categoryRepository.GetFirstOrDefaultAsync(x => x.CategoryId == categoryRequest.CategoryId).Result;
            if (categoryUpdate == null) return false;

            categoryUpdate.Name = categoryRequest.Name ?? categoryUpdate.Name;
            categoryUpdate.Description = categoryRequest.Description ?? categoryUpdate.Description;
            categoryUpdate.Status = categoryRequest.Status ?? categoryUpdate.Status;

            await _categoryRepository.UpdateAsync(categoryUpdate);
            return true;
        }
        public async Task DeleteCategory(Guid? id)
        {
            Category categoryDelete = _categoryRepository.GetFirstOrDefaultAsync(x => x.CategoryId.Equals(id) && x.Status == true).Result;
            if (categoryDelete == null) return;
            categoryDelete.Status = false;

            var listSubCateFollowByCate = _subCateRepository.GetSubCatesBy(x => x.CategoryId == categoryDelete.CategoryId).Result;
            foreach (var item in listSubCateFollowByCate)
            {
                item.Status = false;
            }

            await _subCateRepository.UpdateRangeAsync(listSubCateFollowByCate);
            await _categoryRepository.UpdateAsync(categoryDelete);
        }
        public Category GetCategoryByID(Guid? cateID)
        {
            return _categoryRepository.GetFirstOrDefaultAsync(x => x.CategoryId.Equals(cateID)).Result;
        }

        public async Task<ICollection<DropdownCategory>> GetDropdownCategory()
        {
            List<DropdownCategory> result = new List<DropdownCategory>();
            try
            {
                var categories = await _categoryRepository.GetCategoriesBy(c => c.Status.Value);
                if (categories.Count > 0)
                {
                    result = categories.Select(cate => new DropdownCategory()
                    {
                        CategoryId = cate.CategoryId,
                        Name = cate.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetAllAvailable: " + ex.Message);
                throw;
            }
            return result;
        }
    }
}
