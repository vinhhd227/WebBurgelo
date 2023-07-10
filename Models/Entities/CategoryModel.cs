using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Category")]
public class CategoryModel
{
    [Key]
    [Column("CategoryId")]
    [Display(Name = "Category Id")]
    public int CategoryId { set; get; }
    [Column("CategoryName")]
    [Display(Name = "Category Name")]
    public string? CategoryName { set; get; }
}