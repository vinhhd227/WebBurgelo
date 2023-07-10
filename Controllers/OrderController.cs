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
    [Route("/order")]
    [HttpGet]
    public async Task<IActionResult> Index(int? confirmstatus, int? deliverystatus, int? paymentstatus, int? customerconfirm, int? shipperid)
    {
        var account = _accountService.GetAccountInfo();
        if (account.AccountId == 0)
        {
            return RedirectToAction(nameof(LoginAlert));
        }
        // Get order session
        var listOrder = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.ConfirmStatus != 2 && (o.Delivery.DeliveryStatus < 2 || o.Delivery.CustomerConfirm == 0) select o).ToListAsync();
        _orderService.SaveOrderSession(listOrder);
        // To Pay
        if (confirmstatus == 0)
        {
            Console.WriteLine("To Pay");
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && (o.ConfirmStatus == 0 || (o.ConfirmStatus == 1 && (o.PaymentStatus < 2 && o.PaymentMethod == 1))) select o).ToListAsync();
            listOrder = qr;
        }
        // To Ship
        else if (confirmstatus == 1 && shipperid == 0)
        {
            Console.WriteLine("To Ship");
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.ConfirmStatus == 1 && o.Delivery.ShipperId == 0 && (o.PaymentStatus == 2 || (o.PaymentMethod == 0 && o.PaymentStatus == 0)) select o).ToListAsync();
            listOrder = qr;
        }
        // To Receive
        else if (confirmstatus == 1 && shipperid == 1 && customerconfirm == 0)
        {
            Console.WriteLine("To Receive");
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.ConfirmStatus == confirmstatus && o.Delivery.DeliveryStatus > 0 && o.Delivery.ShipperId != 0 && o.Delivery.CustomerConfirm == customerconfirm select o).ToListAsync();
            listOrder = qr;
        }
        // Completed
        else if (confirmstatus == 1 && deliverystatus == 2 && paymentstatus == 2 && customerconfirm == 1)
        {
            Console.WriteLine("Completed");
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.Delivery.DeliveryStatus == deliverystatus && o.PaymentStatus == paymentstatus && o.ConfirmStatus == confirmstatus && o.Delivery.CustomerConfirm == customerconfirm select o).ToListAsync();
            listOrder = qr;
        }
        // Cancelled
        else if (confirmstatus == 2)
        {
            Console.WriteLine("Cancelled");
            var qr = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.ConfirmStatus == confirmstatus select o).ToListAsync();
            listOrder = qr;
        }
        List<OrderViewModel> listModel = new List<OrderViewModel>();
        foreach (var order in listOrder)
        {
            var orderDetails = await (from od in _burgeloContext.orderDetails where od.OrderId == order.OrderId select od).ToListAsync();

            var deliveryModel = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            DeliveryViewModel deliveryViewModel = new DeliveryViewModel()
            {
                DeliveryId = deliveryModel.DeliveryId,
                DeliveryCode = deliveryModel.DeliveryCode,
                DeliveryStatus = deliveryModel.DeliveryStatus,
                CustomerConfirm = deliveryModel.CustomerConfirm,
                ShipperId = deliveryModel.ShipperId,
                shipper = await _burgeloContext.users.FindAsync(deliveryModel.ShipperId)
            };
            listModel.Add(new OrderViewModel()
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                UserId = order.UserId,
                DeliveryId = order.DeliveryId,
                CustomerName = order.CustomerName,
                Phone = order.Phone,
                Address = order.Address,
                OrderDateTime = order.OrderDateTime,
                SubTotal = order.SubTotal,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                ConfirmStatus = order.ConfirmStatus,
                Delivery = deliveryViewModel,
                listOrderDetail = orderDetails
            });
        }
        return View(listModel);
    }
    public async Task<IActionResult> PayNow(int orderid)
    {
        try
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
        catch (Exception e)
        {
            return RedirectToAction(nameof(LoginAlert));
        }
    }
    public async Task<IActionResult> PayCheck(int orderid)
    {
        try
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            order.PaymentStatus = 1;
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return RedirectToAction(nameof(LoginAlert));
        }
    }
    public async Task<IActionResult> ReceivedConfirm(int orderid)
    {
        var order = await _burgeloContext.orders.FindAsync(orderid);
        var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
        delivery.CustomerConfirm = 1;
        _burgeloContext.Update(delivery);
        await _burgeloContext.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());
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