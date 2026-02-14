using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("nguoi_dung_vai_tro")]
    public class nguoi_dung_vai_tro
    {
        public string ma_nguoi_dung { get; set; } = "";
        public string ma_vai_tro { get; set; } = "";

        // Navigation
        public nguoi_dung? nguoi_dung { get; set; }
        public vai_tro? vai_tro { get; set; }
    }
}