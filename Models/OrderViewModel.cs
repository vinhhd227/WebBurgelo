using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;
// public class OrderManageModel
// {
//     public OrderViewModel order { set; get; }
//     public List<OrderDetailModel> listOrderDetail { set; get; }
//     public UserModel user { set; get; }
// }
public class OrderViewModel
{
    public int OrderId { set; get; }
    public string OrderCode { set; get; }
    public int UserId { set; get; }
    public int DeliveryId { set; get; }
    public string CustomerName { set; get; }
    public string Phone { set; get; }
    public string Address { set; get; }
    public DateTime OrderDateTime { set; get; }
    public int SubTotal { set; get; }
    // 0 - COD, 1 - Momo 
    [Display(Name = "PaymentMethod")]
    public int PaymentMethod { set; get; } = 0;
    // 0 - Waiting, 1 - đã xác nhận , 2 - Đã hủy
    [Display(Name = "PaymentStatus")]
    public int PaymentStatus { set; get; } = 0;
    // 0 - Waiting, 1 - Confirmed
    [Display(Name = "ConfirmStatus")]
    public int ConfirmStatus { set; get; } = 0;
    public DeliveryViewModel Delivery { set; get; }
    public List<OrderDetailModel> listOrderDetail { set; get; }
}



