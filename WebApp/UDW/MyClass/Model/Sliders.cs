using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Sliders")]
    public class Sliders
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên Slider không được để trống")]
        [Display(Name = "Tên Slider")]
        public string Name { get; set; }

        [Display(Name = "Liên kết")]
        public string URL { get; set; }

        //[Required(ErrorMessage = "Hình ảnh không được để trống")]
        [Display(Name = "Hình")]
        public string Image { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Required(ErrorMessage = "Vị trí không được để trống")]
        [Display(Name = "Vị trí")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage = "Từ khóa không được để trống")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Cập nhật bởi")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Thời gian cập nhật")]
        public DateTime? UpdateAt { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
    }
}
