using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Products")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã loại SP không được để trống")]
        [Display(Name = "Mã loại SP")]
        public int CatID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mã nhà CC không được để trống")]
        [Display(Name = "Mã NCC")]
        public int SupplierId { get; set; }

        [Display(Name = "Liên kết")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Chi tiết không được để trống")]
        [Display(Name = "Chi tiết SP")]
        public string Detail { get; set; }

        [Display(Name = "Ảnh SP")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Giá bán không được để trống")]
        [Display(Name = "Đơn giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Giá giảm không được để trống")]
        [Display(Name = "Giá giảm")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Display(Name = "Số lượng")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Tên loại SP")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage = "Từ khóa không được để trống")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Tên loại SP")]
        public int CreateBy { get; set; }

        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Cập nhật bởi")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
    }
}
