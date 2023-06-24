using ExpenseTracker.BLL.DTO;
using ExpenseTracker.BLL.Services.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers;


[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var categories = await _categoryService.GetAllCategoriesAsync(loggedInUserId);
        return View(categories.Data);
    }


    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryDto categoryDto)
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _categoryService.CreateCategoryAsync(categoryDto, loggedInUserId);

        if (response.Success)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", string.Join(", ", response.Errors));
        return View(categoryDto);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _categoryService.GetCategoryByIdAsync(id, loggedInUserId);

        if (!response.Success)
        {
            return NotFound();
        }

        return View(response.Data);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryDto categoryDto)
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _categoryService.UpdateCategoryAsync(categoryDto, loggedInUserId);

        if (response.Success)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", string.Join(", ", response.Errors));
        return View(categoryDto);
    }

    // GET: Category/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _categoryService.GetCategoryByIdAsync(id, loggedInUserId);

        if (!response.Success)
        {
            return NotFound();
        }

        return View(response.Data);
    }

    // POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _categoryService.DeleteCategoryAsync(id, loggedInUserId);

        if (response.Success)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", string.Join(", ", response.Errors));
        return View();
    }
}