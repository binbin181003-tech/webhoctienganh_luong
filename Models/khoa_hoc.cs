using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("khoa_hoc")]
    public class khoa_hoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_khoa_hoc { get; set; }
        public string ten_khoa_hoc { get; set; } = "";
        public string mo_ta { get; set; } = "";
        public decimal hoc_phi { get; set; }
        public string trinh_do { get; set; } = ""; // beginner, intermediate, advanced
        public int thoi_luong_tuan { get; set; }
        public string nguoi_tao { get; set; } = ""; // FK -> nguoi_dung.ma_nguoi_dung

        // Navigation
        public nguoi_dung? nguoi_tao_nav { get; set; }
        public ICollection<lop_hoc> lop_hocs { get; set; } = new List<lop_hoc>();
        public ICollection<bai_hoc> bai_hocs { get; set; } = new List<bai_hoc>();
        public ICollection<bai_kiem_tra> bai_kiem_tras { get; set; } = new List<bai_kiem_tra>();
        public ICollection<danh_gia> danh_gias { get; set; } = new List<danh_gia>();
    }
}