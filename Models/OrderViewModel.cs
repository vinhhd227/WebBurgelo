using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;
public class OrderManageModel
{
    public OrderModel order { set; get; }
    public List<OrderDetailModel> listOrderDetail { set; get; }
    public UserModel user { set; get; }
}
public class OrderViewModel
{
    public int OrderId { set; get; }
    public string OrderCode { set; get; }
    public int UserId { set; get; }
    public string CustomerName { set; get; }
    public string Phone { set; get; }
    public string Address { set; get; }
    public DateTime OrderDateTime { set; get; }
    public int SubTotal { set; get; }
    // 0 - Waiting, 1 - Online, 2 - COD
    public int PaymentMethod { set; get; }
    // 0 - Waiting, 1 - Done
    public int PaymentStatus { set; get; } = 0;
    // 0 - Waiting, 1 - đã xác nhận , 2 - Đã hủy
    public int ConfirmStatus { set; get; } = 0;
    // 0 - Chờ giao hàng, 1 - Đang giao hàng, 2 - Giao hàng thành công, 3 - Giao hàng thất bại
    public int DeliveryStatus { set; get; } = 0;
    public List<OrderDetailModel> listOrderDetail { set; get; }
}