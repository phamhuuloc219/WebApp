using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Họ tên người dùng")]
        public string Fullname { get; set; }
        [Required]
        [Display(Name = "Thư điện tử")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Hình")]
        public string Img { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Quyền truy cập")]
        public string Role { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreateAt { get; set; }
        [Display(Name = "Người tạo")]
        public int? CreateBy { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }
        [Display(Name = "Người cập nhật")]
        public int? UpdateBy { get; set; }
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
    }
}
