using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace WebBurgelo.Controllers;
[Route("/menu/[action]")]
public class MenuController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;
    private readonly CartService _cartService;

    public MenuController(ILogger<HomeController> logger, IWebHostEnvironment env, BurgeloContext burgeloContext, CartService cartService)
    {
        _logger = logger;
        _env = env;
        _burgeloContext = burgeloContext;
        _cartService = cartService;
    }
    [Route("/menu")]
    public async Task<IActionResult> Index(string? categoryName = "")
    {

        if (categoryName == "")
        {
            ViewData["category"] = "Menu";
            MenuViewModel model = new MenuViewModel()
            {
                products = await _burgeloContext.products.ToListAsync()
            };
            return View(model);
        }
        else
        {
            ViewData["category"] = categoryName;
            MenuViewModel model = new MenuViewModel()
            {
                products = await (from p in _burgeloContext.products where p.category.CategoryName.ToLower() == categoryName select p).ToListAsync()
            };
            return View(model);
        }
    }


}

// [HttpGet]
// public IActionResult Product(int id)
// {
//     var product = _burgeloContext.products.Find(id);
//     return View(product);
// }
// [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
// public IActionResult Error()
// {
//     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
// }
// }
