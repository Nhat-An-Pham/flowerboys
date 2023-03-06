using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.SubCateModule.Response;
using Traibanhoa.Modules.SubCateModule.Request;
using Models.Models;

namespace Traibanhoa.Modules.SubCateModule.Interface
{
    public interface ISubCateService
    {
        public Task<bool> AddNewSubCate(CreateSubCategoryRequest newSubCate);

        public Task<bool> UpdateSubCate(UpdateSubCategoryRequest subCateUpdate);
        public Task<bool> DeleteSubCate(Guid? deleteSubCateId);

        public Task<ICollection<SubCategory>> GetAll();

        public Task<ICollection<SubCategory>> GetAllForStaff();

        public Task<ICollection<SubCategory>> GetSubCatesBy(
            Expression<Func<SubCategory, bool>> filter = null,
            Func<IQueryable<SubCategory>, ICollection<SubCategory>> options = null,
            string includeProperties = null);

        public SubCategory GetSubCateByID(Guid? id);

        public SubCategory GetSubCateByIDForStaff(Guid? id);

        public Task<ICollection<SubCateResponse>> GetSubCatesByCategoryId(Guid id);
        public Task<ICollection<SubCateResponse>> GetSubCatesByCategoryIdForStaff(Guid id);
    }
}

