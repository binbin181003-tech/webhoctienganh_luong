using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("lich_hoc")]
    public class lich_hoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_lich_hoc { get; set; }
        public int ma_lop_hoc { get; set; }
        public string thu_trong_tuan { get; set; } = ""; // Mon, Tue, Wed...
        public string gio_bat_dau { get; set; } = "";
        public string gio_ket_thuc { get; set; } = "";
        public string phong_hoc { get; set; } = "";

        // Navigation
        public lop_hoc? lop_hoc { get; set; }
    }
}