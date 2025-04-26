using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Models;       // for view-models
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
public class MenuTypeController : Controller
{
    private readonly IUnitOfWork _uow;
    public MenuTypeController(IUnitOfWork uow) => _uow = uow;

    // GET: /Admin/MenuType
    public async Task<IActionResult> Index()
    {
        // Eagerly load Meals
        var allTypes = await _uow
            .MenuType
            .Query()
            .Include(mt => mt.Meals)
            .ToListAsync();

        return View(allTypes);
    }

    // GET: /Admin/MenuType/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MenuTypeViewModel
        {
            AvailableMeals = (await _uow.MenuItem.GetAllAsync())
                .Select(mi => new SelectListItem(mi.Name, mi.Id.ToString()))
                .ToList()
        };
        return View(vm);
    }

    // POST: /Admin/MenuType/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuTypeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.AvailableMeals = (await _uow.MenuItem.GetAllAsync())
              .Select(mi => new SelectListItem(mi.Name, mi.Id.ToString()))
              .ToList();
            return View(vm);
        }

        var entity = new MenuType
        {
            Name = vm.Name,
            Meals = (await _uow.MenuItem.Query()
                      .Where(mi => vm.SelectedMealIds.Contains(mi.Id))
                      .ToListAsync())
        };
        await _uow.MenuType.AddAsync(entity);
        await _uow.CompleteAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Admin/MenuType/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var e = await _uow.MenuType.GetByIdAsync(id);
        if (e == null) return NotFound();

        var allItems = await _uow.MenuItem.GetAllAsync();
        var vm = new MenuTypeViewModel
        {
            Id = e.Id,
            Name = e.Name,
            SelectedMealIds = e.Meals.Select(m => m.Id).ToList(),
            AvailableMeals = allItems
              .Select(mi => new SelectListItem(mi.Name, mi.Id.ToString())).ToList()
        };
        return View("create",vm);
    }

    // POST: /Admin/MenuType/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MenuTypeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            // If invalid, refill the AvailableMeals for the view
            vm.AvailableMeals = (await _uow.MenuItem.GetAllAsync())
                .Select(mi => new SelectListItem(mi.Name, mi.Id.ToString()))
                .ToList();
            return View("Create", vm);
        }

        // 1) Load the MenuType *with* its existing Meals
        var entity = await _uow
            .MenuType
            .Query()
            .Include(mt => mt.Meals)
            .FirstOrDefaultAsync(mt => mt.Id == vm.Id);

        if (entity == null) return NotFound();

        // 2) Update simple props
        entity.Name = vm.Name;

        // 3) Remove any meal no longer selected
        foreach (var m in entity.Meals.ToList())
        {
            if (!vm.SelectedMealIds.Contains(m.Id))
                entity.Meals.Remove(m);
        }

        // 4) Add only newly selected meals
        var toAdd = await _uow.MenuItem
            .Query()
            .Where(mi => vm.SelectedMealIds.Contains(mi.Id))
            .ToListAsync();

        foreach (var m in toAdd)
        {
            if (!entity.Meals.Any(existing => existing.Id == m.Id))
                entity.Meals.Add(m);
        }

        // 5) Save changes (UnitOfWork.CompleteAsync will call SaveChangesAsync)
        await _uow.CompleteAsync();

        return RedirectToAction(nameof(Index));
    }

    // POST: /Admin/MenuType/Delete/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _uow.MenuType.DeleteAsync(id);
        await _uow.CompleteAsync();
        return RedirectToAction(nameof(Index));
    }
}
