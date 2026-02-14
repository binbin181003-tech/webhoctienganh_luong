using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("lop_hoc")]
    public class lop_hoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_lop_hoc { get; set; }
        public int ma_khoa_hoc { get; set; }
        public string ma_giao_vien { get; set; } = ""; // FK -> nguoi_dung
        public DateTime ngay_bat_dau { get; set; }
        public DateTime ngay_ket_thuc { get; set; }
        public int so_luong_toi_da { get; set; }
        public string trang_thai { get; set; } = "open"; // open, closed, full

        // Navigation
        public khoa_hoc? khoa_hoc { get; set; }
        public nguoi_dung? giao_vien { get; set; }
        public ICollection<dang_ky_model> dang_kys { get; set; } = new List<dang_ky_model>();
        public ICollection<lich_hoc> lich_hocs { get; set; } = new List<lich_hoc>();
    }
}