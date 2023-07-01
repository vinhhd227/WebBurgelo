using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebBurgelo.Models;

[Table("Account")]
public class AccountModel
{
    [Key]
    public int AccountId { set; get; }
    [StringLength(50)]
    [Column("AccountName")]
    public string? AccountName { set; get; }
    [StringLength(50)]
    [Column("Password")]
    public string? Password { set; get; }
    [Column("UserId")]
    public int UserId { set; get; }
    [ForeignKey("UserId")]
    [JsonIgnore]
    public virtual UserModel user { set; get; }
}