using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("dang_ky")]
    public class dang_ky_model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_dang_ky { get; set; }
        public string ma_hoc_vien { get; set; } = ""; // FK -> nguoi_dung
        public int ma_lop_hoc { get; set; }
        public DateTime ngay_dang_ky { get; set; } = DateTime.Now;
        public string trang_thai { get; set; } = "ChoThanhToan"; // ChoThanhToan, DaThanhToan, Cancelled

        // Navigation
        public nguoi_dung? hoc_vien { get; set; }
        public lop_hoc? lop_hoc { get; set; }
        public hoa_don? hoa_don { get; set; }
    }
}