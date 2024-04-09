using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Menus")]
    public class Menus
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên menu không được để trống")]
        [Display(Name = "Tên menu")]
        public string Name { get; set; }

        [Display(Name = "Bảng dữ liệu")]
        public int? TableId { get; set; }

        [Display(Name = "Kiểu menu")]
        public string TypeMenu { get; set; }

        [Display(Name = "Vị trí")]
        public string Position { get; set; }

        [Display(Name = "URL")]
        public string Link { get; set; }

        [Display(Name = "Cấp cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Người cập nhật")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
    }
}
