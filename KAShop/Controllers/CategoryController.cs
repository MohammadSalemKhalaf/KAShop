using API_Task1.Data;
using KAShop.DTO;
using KAShop.DTO.Request;
using KAShop.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KAShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoryController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        // ✅ Get All
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var cat = context.Categories.ToList();
                var catDTO = cat.Adapt<List<CategoryResponseDTO>>();
                if(!cat.Any())
                {
                    return NotFound(new
                    {
                        message = _localizer["NoCategories"].Value
                    });
                }
                return Ok(new
                {
                    message = _localizer["GetAllCategoriesDone"].Value,
                    cats = catDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ✅ Get By Id
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var cat = context.Categories.Find(id);
                if (cat == null)
                {
                    return NotFound(new
                    {
                        message = _localizer["CategoryNotFound"].Value
                    });
                }

                var catDTO = cat.Adapt<CategoryResponseDTO>();
                return Ok(new
                {
                    message = _localizer["GetCategoryDone"].Value,
                    category = catDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ✅ Create
        [HttpPost]
        public IActionResult Create(CategoryRequestDTO request)
        {
            try
            {
                var categoryDb = request.Adapt<Category>();
                context.Categories.Add(categoryDb);
                context.SaveChanges();

                return Ok(new
                {
                    message = _localizer["CreateCategoryDone"].Value,
                    category = categoryDb
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // ✅ Delete
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var cat = context.Categories.Find(id);
                if (cat == null)
                {
                    return NotFound(new
                    {
                        message = _localizer["CategoryNotFound"].Value
                    });
                }

                context.Categories.Remove(cat);
                context.SaveChanges();

                return Ok(new
                {
                    message = _localizer["RemoveCategoryDone"].Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ✅ Update
        [HttpPatch("{id}")]
        public IActionResult Update(int id, CategoryRequestDTO request)
        {
            try
            {
                var cat = context.Categories.Find(id);
                if (cat == null)
                {
                    return NotFound(new
                    {
                        message = _localizer["CategoryNotFound"].Value
                    });
                }

                request.Adapt(cat);
                context.SaveChanges();

                return Ok(new
                {
                    message = _localizer["UpdateCategoryDone"].Value,
                    category = cat
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ✅ Remove All
        [HttpDelete("all")]
        public IActionResult RemoveAll()
        {
            try
            {
                var cat = context.Categories.ToList();
                if (!cat.Any())
                {
                    return NotFound(new
                    {
                        message = _localizer["NoCategories"].Value
                    });
                }

                context.Categories.RemoveRange(cat);
                context.SaveChanges();

                return Ok(new
                {
                    message = _localizer["RemoveAllCategoriesDone"].Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
