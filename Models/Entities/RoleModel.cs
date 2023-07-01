using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebBurgelo.Models;
[Table("Role")]
public class RoleModel
{
    [Key]
    [Column("RoleId")]
    public int RoleId { set; get; }
    [StringLength(50)]
    [Column("RoleName")]
    public string? RoleName { set; get; }
}