// Controllers/Api/MenuTypesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Api;

namespace FoodWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuTypesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public MenuTypesController(IUnitOfWork uow) => _uow = uow;

        // GET api/menutypes
        [HttpGet]
        public async Task<ActionResult<List<MenuTypeDto>>> GetAll()
        {
            var types = await _uow.MenuType
                .Query()
                .Include(mt => mt.Meals)
                .ToListAsync();

            var dtos = types.Select(mt => new MenuTypeDto
            {
                Id = mt.Id,
                Name = mt.Name,
                Meals = mt.Meals
                           .Select(m => new IdNameDto { Id = m.Id, Name = m.Name })
                           .ToList()
            }).ToList();

            return Ok(dtos);
        }

        // GET api/menutypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuTypeDto>> Get(int id)
        {
            var mt = await _uow.MenuType
                .Query()
                .Include(mt => mt.Meals)
                .FirstOrDefaultAsync(mt => mt.Id == id);
            if (mt == null) return NotFound();

            var dto = new MenuTypeDto
            {
                Id = mt.Id,
                Name = mt.Name,
                Meals = mt.Meals
                           .Select(m => new IdNameDto { Id = m.Id, Name = m.Name })
                           .ToList()
            };
            return Ok(dto);
        }

        // POST api/menutypes
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] MenuTypeCreateDto dto)
        {
            var items = await _uow.MenuItem.Query()
                .Where(mi => dto.SelectedMealIds.Contains(mi.Id))
                .ToListAsync();

            var mt = new MenuType
            {
                Name = dto.Name,
                Meals = items
            };

            await _uow.MenuType.AddAsync(mt);
            await _uow.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = mt.Id }, null);
        }

        // PUT api/menutypes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] MenuTypeUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var mt = await _uow.MenuType
                .Query()
                .Include(x => x.Meals)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (mt == null)
                return NotFound();

            // 1) Update name
            mt.Name = dto.Name;

            // 2) Remove any meals no longer selected
            var toRemove = mt.Meals
                .Where(m => !dto.SelectedMealIds.Contains(m.Id))
                .ToList();
            foreach (var m in toRemove)
            {
                mt.Meals.Remove(m);
            }

            // 3) Add only newly selected meals
            var toAdd = await _uow.MenuItem.Query()
                .Where(mi => dto.SelectedMealIds.Contains(mi.Id))
                .ToListAsync();
            foreach (var m in toAdd)
            {
                if (!mt.Meals.Any(existing => existing.Id == m.Id))
                    mt.Meals.Add(m);
            }

            // 4) Persist
            await _uow.CompleteAsync();
            return NoContent();
        }

        // DELETE api/menutypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var mt = await _uow.MenuType.GetByIdAsync(id);
            if (mt == null) return NotFound();

            await _uow.MenuType.DeleteAsync(id);
            await _uow.CompleteAsync();
            return NoContent();
        }
    }
}
