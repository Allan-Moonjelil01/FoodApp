using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Models;
using System.Linq;

[Area("Admin")]
public class MenuController : Controller
{
    private readonly IUnitOfWork _uow;
    public MenuController(IUnitOfWork uow) => _uow = uow;

    // GET: /Admin/Menu
    public async Task<IActionResult> Index()
    {
        var items = await _uow.MenuItem.GetAllAsync();
        return View(items);
    }

    // GET: /Admin/Menu/Create
    public IActionResult CreateItem()
    {
        return View("CreateItem", new DataAccess.MenuItem());
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateItem(DataAccess.MenuItem vm)
    {
        if (!ModelState.IsValid) return View("CreateItem", vm);

        var entity = new DataAccess.MenuItem
        {
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            ImageUrl = vm.ImageUrl,
            Available = vm.Available == "Yes" ? "Yes" : "No",
        };    
        
        await _uow.MenuItem.AddAsync(entity);
        await _uow.CompleteAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Admin/Menu/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var e = await _uow.MenuItem.GetByIdAsync(id);
        if (e == null) return NotFound();

        var vm = new DataAccess.MenuItem
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Price = e.Price,
            ImageUrl = e.ImageUrl,
            Available = e.Available
        };
        return View("CreateItem", vm);
    }

    // POST: /Admin/Menu/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DataAccess.MenuItem vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var e = await _uow.MenuItem.GetByIdAsync(vm.Id);
        if (e == null) return NotFound();

        e.Name = vm.Name;
        e.Description = vm.Description;
        e.Price = vm.Price;
        e.ImageUrl = vm.ImageUrl;
        e.Available = vm.Available;

        await _uow.MenuItem.UpdateAsync(e);
        await _uow.CompleteAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: /Admin/Menu/Delete/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _uow.MenuItem.DeleteAsync(id);
        await _uow.CompleteAsync();
        return RedirectToAction(nameof(Index));
    }
}
