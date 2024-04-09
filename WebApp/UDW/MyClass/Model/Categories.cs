using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Categories")]
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Tên loại sản phẩm không được để trống")]
        [Display(Name="Tên loại SP")]
        public string Name { get; set; }

        [Display(Name = "Liên kết")]
        public string Slug { get; set; }

        [Display(Name = "Cấp cha")]
        public int? ParentID { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Required(ErrorMessage = "Phần mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage = "Phần từ khóa không được để trống")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateAt { get; set; }

        [Display(Name = "Người tạo")]
        public int? CreateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }

        [Display(Name = "Người cập nhật")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
    }
}
