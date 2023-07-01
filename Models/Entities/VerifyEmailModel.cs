using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebBurgelo.Models;

[Table("VerifyEmail")]
public class VerifyEmailModel
{
    [Key]
    [Column("VerifyEmailId")]
    public int VerifyEmailId { set; get; }
    [Column("UserId")]
    public int UserId { set; get; }
    [StringLength(6)]
    [Column("Code")]
    public string? Code { set; get; }
    [Column("ExpirationTime")]
    public DateTime ExpirationTime { set; get; }
    [ForeignKey("UserId")]
    [BindNever]
    public virtual UserModel? user { set; get; }
}