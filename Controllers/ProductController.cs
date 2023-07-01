using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace WebBurgelo.Controllers;
[Route("/product/[action]")]
public class ProductController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;
    private readonly CartService _cartService;

    public ProductController(IWebHostEnvironment env, BurgeloContext burgeloContext, CartService cartService)
    {
        _env = env;
        _burgeloContext = burgeloContext;
        _cartService = cartService;
    }
    [Route("/product")]
    public async Task<IActionResult> Index(int productId)
    {
        var product = _burgeloContext.products.Find(productId);
        return View(product);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}



