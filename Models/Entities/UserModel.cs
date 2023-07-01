using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebBurgelo.Models;
[Table("User")]
public class UserModel
{
    [Key]
    [Column("UserId")]
    public int UserId { set; get; }
    [Column("UserName", TypeName = "nvarchar")]
    [StringLength(50)]
    [Display(Name = "Name")]
    public string? UserName { set; get; }
    [Column("Gender", TypeName = "nvarchar")]
    [StringLength(10)]
    public string? Gender { set; get; }
    [Column("DateOfBirth", TypeName = "date")]
    [Display(Name = "Birth Day")]
    public DateTime DateOfBirth { set; get; }
    [Column("Email", TypeName = "nvarchar")]
    [StringLength(100)]
    public string Email { set; get; }
    [Column("RoleId")]
    public int? RoleId { set; get; }
    [Column("Address", TypeName = "nvarchar")]
    [StringLength(100)]
    public string? Address { set; get; }
    [Column("PhoneNumber", TypeName = "nvarchar")]
    [StringLength(20)]
    [Phone]
    public string? PhoneNumber { set; get; }
    [ForeignKey("RoleId")]
    public virtual RoleModel? role { set; get; }
    public virtual VerifyEmailModel verify { get; set; }
    public virtual AccountModel? account { get; set; }
}