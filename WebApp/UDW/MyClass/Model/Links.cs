using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Links")]
    public class Links
    {
        [Key]
        public int Id { get; set; }        

        [Display(Name = "URL")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "ID bảng liên kết không được để trống")]
        [Display(Name = "Bảng liên kết")]
        public int TableId { get; set; }

        [Display(Name = "Loại liên kết")]
        public string Type { get; set; }
    }
}
