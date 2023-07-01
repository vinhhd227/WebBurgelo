using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Order Detail")]
public class OrderDetailModel
{
    [Key]
    [Column("OrderDetailId")]
    [Display(Name = "MÃ ct")]
    public int OrderDetailId { set; get; }
    [Column("OrderId")]
    [Display(Name = "mã hóa đơn")]
    public int OrderId { set; get; }
    [Column("ProductId")]
    public int ProductId { set; get; }
    public int Quantity { set; get; }
    [ForeignKey("ProductId")]
    //[Required]
    public virtual ProductModel? product { set; get; }
    [ForeignKey("OrderId")]
    //[Required]
    public virtual OrderModel? order { set; get; }
}