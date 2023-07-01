// using System.ComponentModel.DataAnnotations.Schema;
// using System.ComponentModel.DataAnnotations;

// namespace MVC.Models;

// [Table("Payment")]
// public class PaymentModel
// {
//     [Key]
//     [Column("PaymentId")]
//     [Display(Name = "")]
//     public int PaymentId { set; get; }
//     [Column("CustomerId")]
//     [Display(Name = "mã kh")]
//     public int CustomerId { set; get; }
//     [Column("CustomerId")]
//     [Display(Name = "mã kh")]
//     public int CustomerId { set; get; }
//     [Column("OrderDateTime")]
//     public DateTime OrderDateTime { set; get; }
//     public decimal Total { set; get; }
//     public int? CouponId { set; get; } = 0;
//     // 0 - Trả sau, 1 - Trả trước
//     [Column("PaymentMethod")]
//     [Display(Name = "PaymentMethod")]
//     public int PaymentMethod { set; get; }
//     // 0 - chờ xác nhận, 1 - đã xác nhận , 2 - Đã hủy
//     [Column("ConfirmStatus")]
//     [Display(Name = "ConfirmStatus")]
//     public int ConfirmStatus { set; get; }
//     // 0 - Chờ giao hàng, 1 - Đang giao hàng, 2 - Giao hàng thành công, 3 - Giao hàng thất bại
//     [Column("DeliveryStatus")]
//     [Display(Name = "DeliveryStatus")]
//     public int DeliveryStatus { set; get; }
// }