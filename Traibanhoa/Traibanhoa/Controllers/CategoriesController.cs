using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.CategoryModule.Interface;
using Traibanhoa.Modules.CategoryModule.Request;
using Traibanhoa.Modules.CategoryModule.Response;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _categoryService.GetAll());
        }

        // GET: api/Categories
        [HttpGet("staff-managing")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesForStaff()
        {
            return Ok(await _categoryService.GetAllForStaff());
        }

        // GET: api/Categories/available
        [HttpGet("dropdown-category")]
        public async Task<ActionResult<IEnumerable<DropdownCategory>>> GetCategoriesAvailable()
        {
            var result = await _categoryService.GetDropdownCategory();
            return new JsonResult(new
            {
                total = result.Count,
                result,
            });
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = _categoryService.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCategory(UpdateCategoryRequest category)
        {
            ValidationResult result = new UpdateCategoryRequestValidator().Validate(category);
            if (!result.IsValid)
            {
                return BadRequest();
            }

            if (await _categoryService.UpdateCategory(category) == false) return NotFound();

            return Ok();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CreateCategoryRequest reqCategory)
        {
            ValidationResult result = new CreateCategoryRequestValidator().Validate(reqCategory);
            if (!result.IsValid)
            {
                return BadRequest();
            }

            return Ok(await _categoryService.AddNewCategory(reqCategory));
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}
