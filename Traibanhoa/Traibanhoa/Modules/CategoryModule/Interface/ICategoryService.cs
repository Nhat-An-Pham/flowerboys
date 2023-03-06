using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.CategoryModule.Request;
using Traibanhoa.Modules.CategoryModule.Response;

namespace Traibanhoa.Modules.CategoryModule.Interface
{
    public interface ICategoryService
    {
        public Task<Guid> AddNewCategory(CreateCategoryRequest newCategory);
        public Task<bool> UpdateCategory(UpdateCategoryRequest categoryUpdate);
        public Task DeleteCategory(Guid? ID);
        public Task<ICollection<Category>> GetAll();
        public Task<ICollection<DropdownCategory>> GetDropdownCategory();
        public Task<ICollection<Category>> GetCategoriesBy(Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, ICollection<Category>> options = null,
            string includeProperties = null);
        public Category GetCategoryByID(Guid? cateID);
    }
}
