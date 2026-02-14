using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("bai_kiem_tra")]
    public class bai_kiem_tra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_bai_kiem_tra { get; set; }
        public int ma_khoa_hoc { get; set; }
        public string tieu_de { get; set; } = "";
        public int thoi_luong_phut { get; set; }
        public string nguoi_tao { get; set; } = ""; // FK -> nguoi_dung

        // Navigation
        public khoa_hoc? khoa_hoc { get; set; }
        public nguoi_dung? nguoi_tao_nav { get; set; }
        public ICollection<cau_hoi> cau_hois { get; set; } = new List<cau_hoi>();
        public ICollection<ket_qua_kiem_tra> ket_quas { get; set; } = new List<ket_qua_kiem_tra>();
    }
}