using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("ket_qua_kiem_tra")]
    public class ket_qua_kiem_tra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_ket_qua { get; set; }
        public int ma_bai_kiem_tra { get; set; }
        public string ma_hoc_vien { get; set; } = ""; // FK -> nguoi_dung
        public decimal diem_so { get; set; }
        public DateTime ngay_lam_bai { get; set; } = DateTime.Now;

        // Navigation
        public bai_kiem_tra? bai_kiem_tra { get; set; }
        public nguoi_dung? hoc_vien { get; set; }
    }
}