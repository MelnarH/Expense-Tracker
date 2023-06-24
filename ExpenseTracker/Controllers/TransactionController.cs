using ExpenseTracker.BLL.DTO;
using ExpenseTracker.BLL.Services.API;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers;

[Authorize]
public class TransactionController : Controller
{
    private readonly ITransactionsService _transactionsService;
    private readonly ICategoryService _categoryService;
    private readonly IUsersService _usersService;

    public TransactionController(ITransactionsService transactionsService, ICategoryService categoryService, IUsersService usersService)
    {
        _transactionsService = transactionsService;
        _categoryService = categoryService;
        _usersService = usersService;
    }
    // GET: TransactionController
    // GET: Transaction
    public async Task<IActionResult> Index()
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var transactionsResponse = await _transactionsService.GetAllTransactionsAsync(loggedInUserId);
        
        if (transactionsResponse.Success)
        {
            var transactions = transactionsResponse.Data;
            return View(transactions);
        }

        ModelState.AddModelError("", string.Join(", ", transactionsResponse.Errors));
        return View(new List<TransactionDto>());
    }

    // GET: TransactionController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: TransactionController/Create
    public async Task<IActionResult> Create()
    {
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var categories = await _categoryService.GetAllCategoriesAsync(loggedInUserId);
        ViewBag.Categories = categories.Data;

        var transactionDto = new TransactionDto();
        return View(transactionDto);
    }

    // POST: TransactionController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TransactionDto transactionDto)
    {
        try
        {
            int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                transactionDto.UserId = loggedInUserId;

                var response = await _transactionsService.AddTransactionAsync(transactionDto, loggedInUserId);

                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", string.Join(", ", response.Errors));
            }

            var categories = await _categoryService.GetAllCategoriesAsync(loggedInUserId);
            ViewBag.Categories = categories.Data;
            return View(transactionDto);
        }
        catch
        {
            return View();
        }
    }

    


    // GET: TransactionController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: TransactionController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: TransactionController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: TransactionController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
