using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace WebBurgelo.Controllers;
[Route("/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, BurgeloContext burgeloContext)
    {
        _logger = logger;
        _env = env;
        _burgeloContext = burgeloContext;
    }
    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        HomeModel model = new HomeModel();
        model.categories = await _burgeloContext.categories.ToListAsync();
        model.products = await _burgeloContext.products.ToListAsync();
        if (model.categories != null && model.products != null)
        {
            return View(model);
        }
        return RedirectToAction(nameof(Error));
    }
    [Route("/search")]
    [HttpGet]
    public IActionResult Search(string? SearchString)
    {
        if (SearchString == null)
        {
            ViewBag.status = 0;
            return View();
        }
        else
        {
            // Console.WriteLine(SearchString);
            var qr = from p in _burgeloContext.products where p.ProductName.ToLower().Contains(SearchString.ToLower()) select p;
            if (qr == null)
            {
                ViewBag.status = 1;
                return View();
            }
            else
            {
                foreach (var p in qr)
                    // Console.WriteLine(p.ProId + " " + p.Name);
                    ViewBag.status = 2;
                ViewData["SearchString"] = SearchString;
                List<ProductModel> products = qr.ToList();
                return View(products);
            }
        }
    }
    [HttpPost]
    public IActionResult Subscribe(string email)
    {
        var qr = (from e in _burgeloContext.subscribes where e.Email == email select e).FirstOrDefault();
        if (qr == null)
        {
            _burgeloContext.subscribes.Add(new SubscribeModel()
            {
                Email = email
            });
            _burgeloContext.SaveChanges();
        }
        return View("SubscribeSuccess");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
