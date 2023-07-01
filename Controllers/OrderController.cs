using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace WebBurgelo.Controllers;
[Route("/order/[action]")]
public class OrderController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;
    private readonly AccountService _accountService;
    private readonly OrderService _orderService;
    public OrderController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService, OrderService orderService)
    {
        _env = env;
        _burgeloContext = burgeloContext;
        _accountService = accountService;
        _orderService = orderService;
    }
    [HttpGet]
    public async Task<IActionResult> Index(int? confirmstatus, int? deliverystatus, int? paymentstatus)
    {
        var account = _accountService.GetAccountInfo();
        var listOrder = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.DeliveryStatus < 2 select o).ToListAsync();
        _orderService.SaveOrderSession(listOrder);
        if (confirmstatus == 1 && deliverystatus == 2 && paymentstatus == 2)
        {
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.DeliveryStatus == 2 && o.PaymentStatus == paymentstatus && o.ConfirmStatus == 1 select o).ToListAsync();
            listOrder = qr;
        }
        if (confirmstatus == 2 && deliverystatus == 3)
        {
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.DeliveryStatus == deliverystatus && o.ConfirmStatus == confirmstatus select o).ToListAsync();
            listOrder = qr;
        }
        List<OrderViewModel> listModel = new List<OrderViewModel>();
        foreach (var order in listOrder)
        {
            var orderDetails = await (from od in _burgeloContext.orderDetails where od.OrderId == order.OrderId select od).ToListAsync();
            listModel.Add(new OrderViewModel()
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                Phone = order.Phone,
                Address = order.Address,
                OrderDateTime = order.OrderDateTime,
                SubTotal = order.SubTotal,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                ConfirmStatus = order.ConfirmStatus,
                DeliveryStatus = order.DeliveryStatus,
                listOrderDetail = orderDetails
            });
        }
        return View(listModel);
    }
    public async Task<IActionResult> PayNow(int orderid)
    {
        var order = await _burgeloContext.orders.FindAsync(orderid);
        var orderDetails = await (from od in _burgeloContext.orderDetails where od.OrderId == orderid select od).ToListAsync();
        OrderViewModel model = new OrderViewModel()
        {
            OrderId = order.OrderId,
            OrderCode = order.OrderCode,
            OrderDateTime = order.OrderDateTime,
            listOrderDetail = orderDetails
        };
        return View(model);
    }
    public async Task<IActionResult> PayCheck(int orderid)
    {
         Console.WriteLine("a");
        var order = await _burgeloContext.orders.FindAsync(orderid);
       
        order.PaymentStatus = 1;
        _burgeloContext.Update(order);
        
        await _burgeloContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View();
    }
}