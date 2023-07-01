using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Newtonsoft.Json;

namespace WebBurgelo.Controllers;

[Route("/cart/[action]")]
public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;
    private readonly CartService _cartService;

    public CartController(ILogger<CartController> logger, IWebHostEnvironment env, BurgeloContext burgeloContext, CartService cartService)
    {
        _logger = logger;
        _env = env;
        _burgeloContext = burgeloContext;
        _cartService = cartService;
    }
    // Hiện thị giỏ hàng
    [Route("/cart")]
    public IActionResult Index()
    {
        return View(_cartService.GetCartItems());
    }
    public IActionResult AddToCart(int productid, int quantity = 1)
    {
        Console.WriteLine(productid);
        var product = (from p in _burgeloContext.products where p.ProductId == productid select p).FirstOrDefault();
        // Console.WriteLine(product.ProId);
        if (product == null)
            return NotFound("Không có sản phẩm");

        // Xử lý đưa vào Cart ...
        var cart = _cartService.GetCartItems();
        var cartitem = cart.Find(p => p.product.ProductId == productid);
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cartitem.quantity = cartitem.quantity + quantity;
        }
        else
        {
            //  Thêm mới
            cart.Add(new CartItem() { quantity = quantity, product = product });
        }

        // Lưu cart vào Session
        _cartService.SaveCartSession(cart);
        // Chuyển đến trang hiện thị Cart
        return RedirectToAction(nameof(Index));
    }
    /// Cập nhật
    // [Route("/updatecart", Name = "updatecart")]
    [HttpPost]
    public IActionResult UpdateCart( int productid,  int quantity)
    {
        // Cập nhật Cart thay đổi số lượng quantity ...
        var cart = _cartService.GetCartItems();
        var cartitem = cart.Find(p => p.product.ProductId == productid);
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cartitem.quantity = quantity;
        }
        _cartService.SaveCartSession(cart);
        // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
        return Ok();
    }
    public IActionResult RemoveCart(int productid)
    {
        var cart = _cartService.GetCartItems();
        var cartitem = (from p in cart where p.product.ProductId == productid select p).FirstOrDefault();
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cart.Remove(cartitem);
        }
        _cartService.SaveCartSession(cart);
        return RedirectToAction(nameof(Index));
    }
}