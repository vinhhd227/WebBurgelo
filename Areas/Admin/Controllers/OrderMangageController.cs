using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace WebBurgelo.Areas_Admin_Controllers
{

    [Area("Admin")]
    [Route("/admin/order-manage/[action]")]
    public class OrderManageController : Controller
    {
        // private readonly ILogger<OrderManageController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        public OrderManageController(IWebHostEnvironment env, BurgeloContext burgeloContext)
        {
            // _logger = logger;
            _env = env;
            _burgeloContext = burgeloContext;
        }



        public async Task<IActionResult> Manage(DateTime date)
        {
            List<OrderViewModel> listModel = new List<OrderViewModel>();
            List<OrderModel> listOrder = new List<OrderModel>();
            if (date < DateTime.Now.AddYears(-10))
            {
                listOrder = await _burgeloContext.orders.ToListAsync();
            }
            else
            {
                listOrder = await (from o in _burgeloContext.orders where o.OrderDateTime.Day == date.Day select o).ToListAsync();
            }
            ViewData["Title"] = "ALL";
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
        [HttpGet]
        public async Task<IActionResult> Index(int? paymentstatus = 0, int? confirmstatus = 0, int? deliverystatus = 0)
        {
            //Auto Clear
            {
                var orders = await (from o in _burgeloContext.orders where o.OrderDateTime < DateTime.Today.AddMonths(-1) orderby o.OrderDateTime select o).ToListAsync();
                _burgeloContext.RemoveRange(orders);
                await _burgeloContext.SaveChangesAsync();
            }
            List<OrderViewModel> listModel = new List<OrderViewModel>();
            List<OrderModel> listOrder = new List<OrderModel>();

            if (paymentstatus < 2 && confirmstatus == 0 && deliverystatus == 0)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.PaymentStatus < 2 && o.Delivery.DeliveryStatus == deliverystatus && o.OrderDateTime >= DateTime.Today orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "NEW ORDER";
            }
            else if (confirmstatus == 1 && deliverystatus == 1)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.Delivery.DeliveryStatus == deliverystatus && o.OrderDateTime >= DateTime.Today orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "DELIVERING ORDER";
            }
            else if (confirmstatus == 1 && paymentstatus == 2 && deliverystatus == 2)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.PaymentStatus == paymentstatus && o.Delivery.DeliveryStatus == deliverystatus && o.OrderDateTime >= DateTime.Today orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "COMPLETED ORDER";
            }
            else if (confirmstatus == 2)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.OrderDateTime >= DateTime.Today orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "CANCELLED ORDER";
            }
            else if (confirmstatus == 1 && deliverystatus == 0)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.Delivery.DeliveryStatus == deliverystatus && o.OrderDateTime >= DateTime.Today orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "PROCESSING ORDER";
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
        [HttpPost]
        public IActionResult UpdateOrder(int orderid, int paymentstatus, int confirmstatus, int deliverystatus)
        {
            Console.WriteLine("OrderId:" + orderid);
            var orders = _burgeloContext.orders.ToList();
            var order = orders.Find(o => o.OrderId == orderid);
            var deliveries = _burgeloContext.deliveries.ToList();
            var delivery = deliveries.Find(d => d.DeliveryId == order.DeliveryId);
            delivery.DeliveryStatus = deliverystatus;
            Console.WriteLine(order.OrderCode);
            order.PaymentStatus = paymentstatus;
            order.ConfirmStatus = confirmstatus;
            if (confirmstatus == 2)
            {
                delivery.DeliveryStatus = 3;
            }
            if (deliverystatus == 3)
            {
                order.ConfirmStatus = 2;
            }
            _burgeloContext.deliveries.Update(delivery);
            _burgeloContext.orders.Update(order);
            _burgeloContext.SaveChanges();
            ViewData["Reffer"] = Request.Headers["Referer"].ToString();
            return Ok();
        }
        public async Task<IActionResult> ConfirmPay(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            order.PaymentStatus = 2;
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> UndoPay(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            order.PaymentStatus = 1;
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Refund(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            order.PaymentStatus = 3;
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> ConfirmOrder(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            order.ConfirmStatus = 1;
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> UndoOrder(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            order.ConfirmStatus = 0;
            delivery.DeliveryStatus = 0;
            _burgeloContext.Update(delivery);
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> DeliveryOrder(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            delivery.DeliveryStatus = 1;
            _burgeloContext.Update(delivery);
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> UndoDelivery(int orderid)
        {
            var order = await _burgeloContext.orders.FindAsync(orderid);
            var delivery = await _burgeloContext.deliveries.FindAsync(order.DeliveryId);
            delivery.DeliveryStatus = 0;
            _burgeloContext.Update(delivery);
            _burgeloContext.Update(order);
            await _burgeloContext.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> CancelOrder(int orderid)
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
        public async Task<IActionResult> Clear()
        {
            var orders = await (from o in _burgeloContext.orders where o.OrderDateTime < DateTime.Today.AddDays(-7) orderby o.OrderDateTime select o).ToListAsync();
            _burgeloContext.RemoveRange(orders);
            await _burgeloContext.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }
    }
}