using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace FoodWebApp.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealController : ControllerBase
    {
        private readonly IMealRepository _mealRepository;

        public MealController(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meal>>> GetAll()
        {
            var meals = _mealRepository.GetAll();
            var mealsData = meals.Select(m => new Meal
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl
            });
            return Ok(mealsData);
        }

        // GET api/menuitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meal>> Get(int id)
        {
            var mealFromDb = _mealRepository.Get(u => u.Id == id);

            if (mealFromDb == null)
            {
                return NotFound();
            }
            var meal = new Meal
            {
                Id = mealFromDb.Id,
                Name = mealFromDb.Name,
                Description = mealFromDb.Description,
                Price = mealFromDb.Price,
                ImageUrl = mealFromDb.ImageUrl
            };
            return Ok(meal);
        }


        // POST api/menuitems
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] Meal obj)
        {
            var meal = new Meal
            {
                Name = obj.Name,
                Description = obj.Description,
                Price = obj.Price,
                ImageUrl = obj.ImageUrl
            };
            _mealRepository.Add(meal);
            _mealRepository.Save();
            return CreatedAtAction(nameof(Get), new { id = meal.Id }, null);

        }


        // PUT api/menuitems/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Meal obj)
        {
            if (id != obj.Id) return BadRequest();
            var meal = _mealRepository.Get(m => m.Id == id);
            if (meal == null) return NotFound();
            meal.Name = obj.Name;
            meal.Description = obj.Description;
            meal.Price = obj.Price;
            meal.ImageUrl = obj.ImageUrl;

            _mealRepository.Update(meal);
            _mealRepository.Save();

            return NoContent();

        }

        // DELETE api/menuitems/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var meal = _mealRepository.Get(m => m.Id == id);
            if (meal == null) return NotFound();
            _mealRepository.Remove(meal);
            _mealRepository.Save();
            return NoContent();
        }
    }
}
