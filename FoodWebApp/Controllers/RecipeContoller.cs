using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Models.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public RecipesController(IUnitOfWork uow) => _uow = uow;

        // GET api/recipes/types
        [HttpGet("types")]
        public async Task<ActionResult<List<RecipeTypeDto>>> GetTypes()
        {
            var types = await _uow.MenuType
                .Query()
                .Select(mt => new RecipeTypeDto
                {
                    Id = mt.Id,
                    Name = mt.Name
                })
                .ToListAsync();

            return Ok(types);
        }

        // GET api/recipes?typeId=3
        [HttpGet]
        public async Task<ActionResult<List<RecipeMealDto>>> GetMeals([FromQuery] int typeId)
        {
            // if no typeId provided, pick first
            if (typeId == 0)
            {
                var first = await _uow.MenuType.Query().Select(mt => mt.Id).FirstOrDefaultAsync();
                typeId = first;
            }

            var meals = await _uow.MenuType
                .Query()
                .Where(mt => mt.Id == typeId)
                .Include(mt => mt.Meals)
                .SelectMany(mt => mt.Meals)
                .Select(mi => new RecipeMealDto
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    Description = mi.Description,
                    Price = mi.Price,
                    ImageUrl = mi.ImageUrl
                })
                .ToListAsync();

            return Ok(meals);
        }

        // GET api/recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeMealDto>> GetMeal(int id)
        {
            var mi = await _uow.MenuItem.GetByIdAsync(id);
            if (mi == null) return NotFound();

            var dto = new RecipeMealDto
            {
                Id = mi.Id,
                Name = mi.Name,
                Description = mi.Description,
                Price = mi.Price,
                ImageUrl = mi.ImageUrl
            };
            return Ok(dto);
        }
    }
}
