using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Delivery")]
public class PaymentModel
{
    [Key]
    [Column("DeliveryId")]
    public int DeliveryId { set; get; }
    [Column("CustomerStatus")]
    public int CustomerStatus { set; get; }
}