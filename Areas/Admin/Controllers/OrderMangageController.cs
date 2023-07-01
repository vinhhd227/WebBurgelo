using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MVC.Areas_Admin_Controllers
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
        [HttpGet]
        public async Task<IActionResult> Index(int? paymentstatus, int? confirmstatus, int? deliverystatus)
        {
            Console.WriteLine(confirmstatus.GetValueOrDefault());
            List<OrderManageModel> listModel = new List<OrderManageModel>();
            List<OrderModel> listOrder = new List<OrderModel>();
            if (paymentstatus == null && confirmstatus == null && deliverystatus == null)
            {
                var qr = await _burgeloContext.orders.ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "ALL ORDERS";
            }
            else if (paymentstatus < 2 && confirmstatus == 0 && deliverystatus == 0)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.PaymentStatus <2 && o.DeliveryStatus == deliverystatus orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "NEW ORDERS";
            }
            else if (confirmstatus == 1 && deliverystatus == 1)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.DeliveryStatus == deliverystatus orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "DELIVERING ORDERS";
            }
            else if (confirmstatus == 1 && paymentstatus == 2  && deliverystatus == 2)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.PaymentStatus == paymentstatus && o.DeliveryStatus == deliverystatus orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "FINISH ORDERS";
            }
            else if (confirmstatus == 2)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "CANCEL ORDERS";
            }
            else if (confirmstatus == 1 && deliverystatus == 0)
            {
                var qr = await (from o in _burgeloContext.orders where o.ConfirmStatus == confirmstatus && o.DeliveryStatus == deliverystatus orderby o.OrderDateTime select o).ToListAsync();
                listOrder = qr;
                ViewData["Title"] = "WAITING ORDERS";
            }
            foreach (var order in listOrder)
            {
                OrderManageModel model = new OrderManageModel();
                model.order = order;
                var user = await _burgeloContext.users.FindAsync(order.UserId);
                model.user = user;
                var listOrderDetail = await (from c in _burgeloContext.orderDetails where c.OrderId == order.OrderId select c).ToListAsync();
                model.listOrderDetail = listOrderDetail;
                listModel.Add(model);
            }
            return View(listModel);
            // if (confirmstatus == 0)
            // {
            //     var listOrder = await (from o in _burgeloContext.orders where o.ConfirmStatus == 0 orderby o.OrderDateTime select o).ToListAsync();
            //     foreach (var order in listOrder)
            //     {
            //         OrderManageModel model = new OrderManageModel();
            //         model.order = order;
            //         var user = await _burgeloContext.users.FindAsync(order.UserId);
            //         model.user = user;
            //         var listOrderDetail = await (from c in _burgeloContext.orderDetails where c.OrderId == order.OrderId select c).ToListAsync();
            //         model.listOrderDetail = listOrderDetail;
            //         listModel.Add(model);
            //     }
            //     return View(listModel);
            // }
        }
        [HttpPost]
        public IActionResult UpdateOrder(int orderid, int paymentstatus, int confirmstatus, int deliverystatus)
        {
            Console.WriteLine("OrderId:" + orderid);
            var orders = _burgeloContext.orders.ToList();
            var order = orders.Find(o => o.OrderId == orderid);
            Console.WriteLine(order.OrderCode);
            order.PaymentStatus = paymentstatus;
            order.ConfirmStatus = confirmstatus;
            order.DeliveryStatus = deliverystatus;
            if (confirmstatus == 2)
            {
                order.DeliveryStatus = 3;
            }
            if (deliverystatus == 3)
            {
                order.ConfirmStatus = 2;
            }
            _burgeloContext.orders.Update(order);
            _burgeloContext.SaveChanges();
            ViewData["Reffer"] = Request.Headers["Referer"].ToString();
            return Ok();
        }
    }
}