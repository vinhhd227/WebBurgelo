// using System.ComponentModel.DataAnnotations.Schema;
// using System.ComponentModel.DataAnnotations;

// namespace WebBurgelo.Models;

// [Table("Payment")]
// public class PaymentModel
// {
//     [Key]
//     [Column("PaymentId")]
//     [Display(Name = "PaymentId")]
//     public int PaymentId { set; get; }
//     [Column("PaymentType")]
//     [Display(Name = "PaymentType")]
//     public string PaymentType { set; get; }
//     // 0 - New, 1 - Waiting, 2 - Paid
//     [Column("PaymentStatus")]
//     [Display(Name = "PaymentStatus")]
//     public int PaymentStatus { set; get; } = 0;
// }