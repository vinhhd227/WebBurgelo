using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace WebBurgelo.Areas_Admin_Controllers
{

    [Area("Admin")]
    [Route("/admin/delivery-manage/[action]")]
    public class DeliveryManageController : Controller
    {
        // private readonly ILogger<OrderManageController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        private readonly AccountService _accountService;
        public DeliveryManageController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService)
        {
            // _logger = logger;
            _env = env;
            _burgeloContext = burgeloContext;
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int deliverystatus = 1, int shipperid = 0, int customerconfirm = 0)
        {
            var user = await (from u in _burgeloContext.users where u.UserId == _accountService.GetAccountInfo().UserId select u).FirstOrDefaultAsync();
            if (user is not null)
            {
                if (user.RoleId >= 3)
                {
                    List<OrderViewModel> listModel = new List<OrderViewModel>();
                    List<OrderModel> listOrder = new List<OrderModel>();
                    if (deliverystatus == 3)
                    {
                        var shipper = await _burgeloContext.users.FindAsync(_accountService.GetAccountInfo().UserId);
                        var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == 2 && o.Delivery.DeliveryStatus == deliverystatus && o.Delivery.ShipperId == shipper.UserId select o).ToListAsync();
                        listOrder = qr;
                        ViewData["nav"] = "Cancelled";
                    }
                    else
                    {
                        if (shipperid == 0)
                        {
                            var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == 1 && o.Delivery.DeliveryStatus == deliverystatus && o.Delivery.ShipperId == shipperid select o).ToListAsync();
                            listOrder = qr;
                            ViewData["nav"] = "Waiting";
                        }
                        else
                        {
                            var shipper = user;
                            var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == 1 && o.Delivery.DeliveryStatus == deliverystatus && o.Delivery.ShipperId == shipper.UserId && o.Delivery.CustomerConfirm == customerconfirm select o).ToListAsync();
                            listOrder = qr;
                            if (customerconfirm == 0)
                            {
                                if (deliverystatus == 2)
                                {
                                    ViewData["nav"] = "Success";
                                }
                                else
                                {
                                    ViewData["nav"] = "Delivering";
                                }
                            }
                            else
                            {
                                ViewData["nav"] = "Completed";
                            }

                        }
                    }
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
            }
            return RedirectToAction(nameof(AccessDenied));
        }
        public async Task<IActionResult> StartDelivery(int orderid)
        {
            AccountModel account = _accountService.GetAccountInfo();
            var user = await _burgeloContext.users.FindAsync(account.UserId);
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            delivery.ShipperId = user.UserId;
            order.Delivery = delivery;
            _burgeloContext.Update(delivery);
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> SuccessDelivery(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            delivery.DeliveryStatus = 2;
            _burgeloContext.Update(delivery);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> UndoDelivery(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            delivery.ShipperId = 0;
            order.Delivery = delivery;
            _burgeloContext.Update(delivery);
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> CancelDelivery(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            order.ConfirmStatus = 2;
            delivery.DeliveryStatus = 3;
            _burgeloContext.Update(delivery);
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Paid(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            order.PaymentStatus = 2;
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult AccessDenied()
        {
            ViewData["Reffer"] = Request.Headers["Referer"].ToString();
            return View();
        }
    }
}