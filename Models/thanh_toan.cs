using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("thanh_toan")]
    public class thanh_toan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_thanh_toan { get; set; }

        public int ma_hoa_don { get; set; }
        public DateTime ngay_thanh_toan { get; set; } = DateTime.Now;
        public string phuong_thuc_thanh_toan { get; set; } = "TienMat"; // TienMat, ChuyenKhoan
        public string trang_thai { get; set; } = "completed"; // completed, failed

        // Navigation
        public hoa_don? hoa_don { get; set; }
    }
}