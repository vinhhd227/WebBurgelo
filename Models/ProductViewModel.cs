using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;
public class InputProductModel
        {
             public int ProductId { set; get; }
            [Display(Name = "Tên món")]
            [Required(ErrorMessage = "Phải nhập tên món ăn")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} phải có từ {2} đến {1} kí tự")]
            public string? ProductName { set; get; }
            [StringLength(1000)]
            [Display(Name = "Mô tả")]
            public string? Description { set; get; }
            [Display(Name = "Giá")]
            [Required(ErrorMessage = "Phải nhập {0}")]
            [Range(0, 9999, ErrorMessage = "{0} phải là số dương")]
            public int Price { get; set; }
            [DataType(DataType.Upload)]
            [Display(Name = "File Upload")]
            public IFormFile? FileUpload { set; get; }
            [Display(Name = "Danh mục")]
            public int CategoryId { get; set; }
        }