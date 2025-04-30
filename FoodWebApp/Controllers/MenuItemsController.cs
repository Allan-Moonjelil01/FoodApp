using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAccess;
using Models.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public MenuItemsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET api/menuitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAll()
        {
            var items = await _uow.MenuItem.GetAllAsync();
            var dtos = items.Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.Available == "Yes"
            });
            return Ok(dtos);
        }

        // GET api/menuitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemDto>> Get(int id)
        {
            var m = await _uow.MenuItem.GetByIdAsync(id);
            if (m == null) return NotFound();

            var dto = new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.Available == "Yes"
            };
            return Ok(dto);
        }

        // POST api/menuitems
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] MenuItemCreateDto dto)
        {
            var entity = new DataAccess.MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Available = dto.IsAvailable ? "Yes" : "No"
            };

            await _uow.MenuItem.AddAsync(entity);
            await _uow.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, null);
        }

        // PUT api/menuitems/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] MenuItemUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _uow.MenuItem.GetByIdAsync(id);
            if (entity == null) return NotFound();

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.ImageUrl = dto.ImageUrl;
            entity.Available = dto.IsAvailable ? "Yes" : "No";

            await _uow.MenuItem.UpdateAsync(entity);
            await _uow.CompleteAsync();

            return NoContent();
        }

        // DELETE api/menuitems/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _uow.MenuItem.GetByIdAsync(id);
            if (entity == null) return NotFound();

            await _uow.MenuItem.DeleteAsync(id);
            await _uow.CompleteAsync();
            return NoContent();
        }
    }
}
