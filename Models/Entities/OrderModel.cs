using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Order")]
public class OrderModel
{
    [Key]
    [Column("OrderId")]
    [Display(Name = "")]
    public int OrderId { set; get; }
    [Column("OrderCode")]
    [Display(Name = "OrderCode")]
    public string OrderCode { set; get; }
    [Column("UserId")]
    [Display(Name = "User Id")]
    public int UserId { set; get; }
    [Column("CustomerName")]
    [Display(Name = "Customer Name")]
    public string CustomerName { set; get; }
    [Column("Phone")]
    [Display(Name = "Phone")]
    public string Phone { set; get; }
    [Column("Address")]
    [Display(Name = "Address")]
    public string Address { set; get; }
    [Column("OrderDateTime")]
    public DateTime OrderDateTime { set; get; }
    [Column("SubTotal")]
    public int SubTotal { set; get; }
    // 0 - Waiting, 1 - Online, 2 - COD
    [Column("PaymentMethod")]
    [Display(Name = "PaymentMethod")]
    public int PaymentMethod { set; get; }
    // 0 - Waiting, 1 - Done
    [Column("PaymentStatus")]
    [Display(Name = "PaymentStatus")]
    public int PaymentStatus { set; get; } = 0;
    // 0 - Waiting, 1 - đã xác nhận , 2 - Đã hủy
    [Column("ConfirmStatus")]
    [Display(Name = "ConfirmStatus")]
    public int ConfirmStatus { set; get; } = 0;
    // 0 - Chờ giao hàng, 1 - Đang giao hàng, 2 - Giao hàng thành công, 3 - Giao hàng thất bại
    [Column("DeliveryStatus")]
    [Display(Name = "DeliveryStatus")]
    public int DeliveryStatus { set; get; } = 0;
}
