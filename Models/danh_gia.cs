using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("danh_gia")]
    public class danh_gia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_danh_gia { get; set; }
        public string ma_hoc_vien { get; set; } = ""; // FK -> nguoi_dung
        public int ma_khoa_hoc { get; set; }
        public int so_sao { get; set; } // 1-5
        public string noi_dung { get; set; } = "";
        public DateTime ngay_danh_gia { get; set; } = DateTime.Now;

        // Navigation
        public nguoi_dung? hoc_vien { get; set; }
        public khoa_hoc? khoa_hoc { get; set; }
    }
}