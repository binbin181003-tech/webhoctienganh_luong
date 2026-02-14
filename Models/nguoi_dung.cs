using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("nguoi_dung")]
    public class nguoi_dung
    {
        [Key]
        public string ma_nguoi_dung { get; set; } = Guid.NewGuid().ToString();
        public string email { get; set; } = "";
        public string mat_khau_hash { get; set; } = ""; // plaintext for now
        public string ho_ten { get; set; } = "";
        public string so_dien_thoai { get; set; } = "";
        public string anh_dai_dien { get; set; } = "";
        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public string trang_thai { get; set; } = "active"; // active, inactive, banned

        // Navigation
        public ICollection<nguoi_dung_vai_tro> vai_tros { get; set; } = new List<nguoi_dung_vai_tro>();
        public ICollection<dang_ky_model> dang_kys { get; set; } = new List<dang_ky_model>();
        public ICollection<lop_hoc> lop_giang_days { get; set; } = new List<lop_hoc>();
        public ICollection<ket_qua_kiem_tra> ket_quas { get; set; } = new List<ket_qua_kiem_tra>();
        public ICollection<danh_gia> danh_gias { get; set; } = new List<danh_gia>();
    }
}