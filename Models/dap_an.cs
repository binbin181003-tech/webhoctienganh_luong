using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("dap_an")]
    public class dap_an
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_dap_an { get; set; }
        public int ma_cau_hoi { get; set; }
        public string noi_dung { get; set; } = "";
        public bool la_dap_an_dung { get; set; } = false;

        // Navigation
        public cau_hoi? cau_hoi { get; set; }
    }
}