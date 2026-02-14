using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("hoa_don")]
    public class hoa_don
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_hoa_don { get; set; }

        public int ma_dang_ky { get; set; }
        public decimal so_tien { get; set; }
        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public string trang_thai { get; set; } = "ChuaThanhToan"; // ChuaThanhToan, DaThanhToan, Huy

        // Navigation
        public dang_ky_model? dang_ky { get; set; }
        public thanh_toan? thanh_toan { get; set; }
    }
}