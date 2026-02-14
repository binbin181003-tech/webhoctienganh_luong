using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("bai_hoc")]
    public class bai_hoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_bai_hoc { get; set; }
        public int ma_khoa_hoc { get; set; }
        public string tieu_de { get; set; } = "";
        public string noi_dung { get; set; } = "";
        public string nguoi_tao { get; set; } = ""; // FK -> nguoi_dung
        public DateTime ngay_tao { get; set; } = DateTime.Now;

        // Navigation
        public khoa_hoc? khoa_hoc { get; set; }
        public nguoi_dung? nguoi_tao_nav { get; set; }
        public ICollection<tai_lieu> tai_lieus { get; set; } = new List<tai_lieu>();
    }
}