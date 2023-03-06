using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.BlogSubCateModule.Interface;
using Traibanhoa.Modules.CategoryModule.Interface;
using Traibanhoa.Modules.SubCateModule.Interface;
using Traibanhoa.Modules.SubCateModule.Request;
using Traibanhoa.Modules.SubCateModule.Response;

namespace Traibanhoa.Modules.SubCateModule
{
    public class SubCateService : ISubCateService
    {
        private readonly ISubCateRepository _subCateRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBasketSubCateRepository _basketSubCateRepository;

        public SubCateService(ISubCateRepository subCateRepository, ICategoryRepository categoryRepository, IBasketSubCateRepository basketSubCateRepository)
        {
            _subCateRepository = subCateRepository;
            _categoryRepository = categoryRepository;
            _basketSubCateRepository = basketSubCateRepository;
        }

        public async Task<ICollection<SubCategory>> GetAll()
        {
            return await _subCateRepository.GetSubCatesBy(x => x.Status == true);
        }

        public async Task<ICollection<SubCategory>> GetAllForStaff()
        {
            return await _subCateRepository.GetAll();
        }

        public Task<ICollection<SubCategory>> GetSubCatesBy(
                Expression<Func<SubCategory,
                bool>> filter = null,
                Func<IQueryable<SubCategory>,
                ICollection<SubCategory>> options = null,
                string includeProperties = null)
        {
            return _subCateRepository.GetSubCatesBy(filter);
        }

        public async Task<bool> AddNewSubCate(CreateSubCategoryRequest newSubCateReq)
        {
            var category = _categoryRepository.GetByIdAsync(newSubCateReq.CategoryId).Result;
            if (category == null)
            {
                return false;
            }

            var newSubCate = new SubCategory();
            newSubCate.SubCategoryId = Guid.NewGuid();
            newSubCate.Name = newSubCateReq.Name;
            newSubCate.Description = newSubCateReq.Description;
            newSubCate.CreatedDate = DateTime.Now;
            newSubCate.Status = true;
            newSubCate.CategoryId = newSubCateReq.CategoryId;

            await _subCateRepository.AddAsync(newSubCate);
            return true;
        }

        public async Task<bool> UpdateSubCate(UpdateSubCategoryRequest subCateUpdateReq)
        {
            var subCateUpdate = _subCateRepository.GetByIdAsync(subCateUpdateReq.SubCategoryId).Result;
            if (subCateUpdate == null)
            {
                return false;
            }
            if (subCateUpdateReq.CategoryId != null && _categoryRepository.GetByIdAsync((Guid)subCateUpdateReq.CategoryId).Result == null)
            {
                return false;
            }
            var category = _categoryRepository.GetByIdAsync((Guid)subCateUpdate.CategoryId);
            if (category.Result.Status == false && subCateUpdateReq.Status == true && subCateUpdate.Status == false)
            {
                return false;
            }

            subCateUpdate.Name = subCateUpdateReq.Name ?? subCateUpdate.Name;
            subCateUpdate.Description = subCateUpdateReq.Description ?? subCateUpdate.Description;
            if (subCateUpdate.Status == false && subCateUpdateReq.Status == true)
            {
                var listBlogSubCate = _basketSubCateRepository.GetBlogSubCatesBy(x => x.SubCateId == subCateUpdate.SubCategoryId).Result;
                foreach (var item in listBlogSubCate)
                {
                    item.Status = true;

                    await _basketSubCateRepository.UpdateRangeAsync(listBlogSubCate);
                }
            }
            subCateUpdate.Status = subCateUpdateReq.Status ?? subCateUpdate.Status;
            subCateUpdate.CategoryId = subCateUpdateReq.CategoryId ?? subCateUpdate.CategoryId;

            await _subCateRepository.UpdateAsync(subCateUpdate);

            return true;
        }

        public async Task<bool> DeleteSubCate(Guid? deleteSubCateId)
        {
            if (deleteSubCateId == null)
            {
                return false;
            }
            var _deleteSubCate = _subCateRepository.GetByIdAsync((Guid)deleteSubCateId).Result;
            if (_deleteSubCate == null || _deleteSubCate.Status == false)
            {
                return false;
            }

            var listBlogSubCateDelete = _basketSubCateRepository.GetBlogSubCatesBy(x => x.SubCateId == _deleteSubCate.SubCategoryId).Result.ToList();
            foreach (var item in listBlogSubCateDelete)
            {
                item.Status = false;
            }

            await _basketSubCateRepository.UpdateRangeAsync(listBlogSubCateDelete);

            _deleteSubCate.Status = false;
            await _subCateRepository.UpdateAsync(_deleteSubCate);

            return true;
        }

        public SubCategory GetSubCateByID(Guid? id)
        {
            return _subCateRepository.GetFirstOrDefaultAsync(x => x.SubCategoryId.Equals(id) && x.Status.Value).Result;
        }

        public SubCategory GetSubCateByIDForStaff(Guid? id)
        {
            return _subCateRepository.GetFirstOrDefaultAsync(x => x.SubCategoryId.Equals(id), includeProperties: "Category").Result;
        }

        public async Task<ICollection<SubCateResponse>> GetSubCatesByCategoryId(Guid id)
        {
            try
            {
                List<SubCateResponse> result = new List<SubCateResponse>();
                ICollection<SubCategory> list = await _subCateRepository.GetSubCatesBy(x => x.CategoryId.Equals(id) && x.Status.Value);
                foreach (var subCate in list)
                {
                    result.Add(new SubCateResponse()
                    {
                        SubCateId = subCate.SubCategoryId,
                        Name = subCate.Name
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetSubCatesByCategoryId: " + ex.Message);
                throw;
            }
        }

        public async Task<ICollection<SubCateResponse>> GetSubCatesByCategoryIdForStaff(Guid id)
        {
            try
            {
                List<SubCateResponse> result = new List<SubCateResponse>();
                ICollection<SubCategory> list = await _subCateRepository.GetSubCatesBy(x => x.CategoryId.Equals(id));
                foreach (var subCate in list)
                {
                    result.Add(new SubCateResponse()
                    {
                        SubCateId = subCate.SubCategoryId,
                        Name = subCate.Name
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetSubCatesByCategoryId: " + ex.Message);
                throw;
            }
        }
    }
}

