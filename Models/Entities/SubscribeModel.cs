using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebBurgelo.Models;

[Table("Subscribe")]
public class SubscribeModel
{
    [Key]
    public int SubscribeId { set; get; }
    [StringLength(50)]
    [Column("Email")]
    public string Email { set; get; }
}