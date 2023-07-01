using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace WebBurgelo.Controllers;

[Route("/checkout/[action]")]
public class CheckOutController : Controller
{    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;
    private readonly AccountService _accountService;
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    public CheckOutController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService, CartService cartService, OrderService orderService)
    {
        _env = env;
        _burgeloContext = burgeloContext;
        _accountService = accountService;
        _cartService = cartService;
        _orderService = orderService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (_accountService.GetAccountInfo().AccountId == 0)
        {
            return RedirectToAction(nameof(LoginAlert));
        }
        if (_cartService.GetCartItems().Count() == 0)
        {
            return RedirectToAction(nameof(Error));
        }
        else
        {
            CheckOutModel model = new CheckOutModel();
            UserModel user = await _burgeloContext.users.FindAsync(_accountService.GetAccountInfo().UserId);
            Console.WriteLine(user.UserName);
            if(user.RoleId == 1)
            {
                return RedirectToAction(nameof(EmailAlert));
            }
            model.UserName = user.UserName;
            model.Address = user.Address;
            model.PhoneNumber = user.PhoneNumber;
            model.PaymentMethod = 1;
            model.listCartItems = _cartService.GetCartItems();
            return View(model);
        }

    }
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([Bind("UserName", "Address", "PhoneNumber", "PaymentMethod")] CheckOutModel model)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("Model Oke");
            Console.WriteLine(model.UserName);
            Console.WriteLine(model.Address);
            Console.WriteLine(model.PhoneNumber);
            Console.WriteLine(model.PaymentMethod);
            UserModel user = await _burgeloContext.users.FindAsync(_accountService.GetAccountInfo().UserId);
            try
            {
                List<CartItem> cartItems = _cartService.GetCartItems();
                OrderModel order = new OrderModel();
                var random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                string code = new string(Enumerable.Repeat(chars, 9).Select(s => s[random.Next(s.Length)]).ToArray());
                order.OrderCode = code;
                order.UserId = user.UserId;
                order.CustomerName = model.UserName;
                order.Phone = model.PhoneNumber;
                order.Address = model.Address;
                order.OrderDateTime = DateTime.Now;
                order.SubTotal = 0;
                foreach (var item in cartItems)
                {
                    order.SubTotal += item.product.Price * item.quantity;
                }
                order.PaymentMethod = model.PaymentMethod;
                await _burgeloContext.orders.AddAsync(order);
                await _burgeloContext.SaveChangesAsync();
                Console.WriteLine("Order Maked");
                order = await (from o in _burgeloContext.orders where o.OrderCode == code select o).FirstOrDefaultAsync();
                foreach (var item in cartItems)
                {
                    OrderDetailModel orderDetail = new OrderDetailModel();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.ProductId = item.product.ProductId;
                    orderDetail.Quantity = item.quantity;
                    await _burgeloContext.orderDetails.AddAsync(orderDetail);
                    await _burgeloContext.SaveChangesAsync();
                }
                _cartService.ClearCart();
                var orders = await (from od in _burgeloContext.orders where od.UserId == user.UserId && od.DeliveryStatus < 2 select od).ToListAsync();
                _orderService.SaveOrderSession(orders);
                Console.WriteLine("Order Deatail Maked");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
        else
        {
            //Đọc lỗi Model
            Console.WriteLine("Model Fail");
            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(message);
            return View();
        }
        return View();
    }
    public IActionResult EmailAlert()
    {
        return View();
    }
    public IActionResult LoginAlert()
    {
        return View();
    }
    public IActionResult Error()
    {
        return View();
    }
}