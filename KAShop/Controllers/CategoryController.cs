using API_Task1.Data;
using KAShop.DTO.Request;
using KAShop.DTO.Responses;
using KAShop.Model.Category;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("all")]
        public IActionResult GetAll([FromQuery] string lang = "en")
        {
            try
            {
                var cat = context.Categories
                                 .Include(c => c.Translations)
                                 .ToList();

                if (!cat.Any())
                {
                    return NotFound(new
                    {
                        message = _localizer["NoCategories"].Value
                    });
                }

                var result = cat.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Translations.FirstOrDefault(t => t.Language == lang)?.Name
                           ?? c.Translations.FirstOrDefault(t => t.Language == "en")?.Name
                           ?? string.Empty
                });

                return Ok(new
                {
                    message = _localizer["GetAllCategoriesDone"].Value,
                    cats = result
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


        [HttpGet("{id}")]
        public IActionResult GetCategoryById([FromRoute] int id)
        {
            try
            {
                var cat = context.Categories
                                 .Include(c => c.Translations)
                                 .FirstOrDefault(c => c.Id == id);

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

        [HttpPost]
        public IActionResult Create([FromBody] CategoryRequestDTO request)
        {
            try
            {
                var categoryDb = new Category
                {
                    Status = KAShop.Model.Status.Active,
                    CreatedDate = DateTime.Now,
                    Translations = request.CategoryTranslations.Select(t => new CategoryTranslation
                    {
                        Name = t.Name,
                        Language = t.Language
                    }).ToList()
                };

                context.Categories.Add(categoryDb);
                context.SaveChanges();

                var catDTO = categoryDb.Adapt<CategoryResponseDTO>();
                return Ok(new
                {
                    message = _localizer["CreateCategoryDone"].Value,
                    category = catDTO
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

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var cat = context.Categories
                                 .Include(c => c.Translations)
                                 .FirstOrDefault(c => c.Id == id);

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

        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CategoryRequestDTO request)
        {
            try
            {
                var cat = context.Categories
                                 .Include(c => c.Translations)
                                 .FirstOrDefault(c => c.Id == id);

                if (cat == null)
                {
                    return NotFound(new
                    {
                        message = _localizer["CategoryNotFound"].Value
                    });
                }

                cat.Translations.Clear();
                cat.Translations = request.CategoryTranslations.Select(t => new CategoryTranslation
                {
                    Name = t.Name,
                    Language = t.Language,
                    CategoryId = cat.Id
                }).ToList();

                context.SaveChanges();

                var catDTO = cat.Adapt<CategoryResponseDTO>();
                return Ok(new
                {
                    message = _localizer["UpdateCategoryDone"].Value,
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

        [HttpDelete("all")]
        public IActionResult RemoveAll()
        {
            try
            {
                var cat = context.Categories
                                 .Include(c => c.Translations)
                                 .ToList();

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
