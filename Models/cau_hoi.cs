using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace webhoctienganh.Models
{
    [Table("cau_hoi")]
    public class cau_hoi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_cau_hoi { get; set; }

        public int ma_bai_kiem_tra { get; set; }
        public string noi_dung { get; set; } = "";

        // Navigation
        public bai_kiem_tra? bai_kiem_tra { get; set; }
        public ICollection<dap_an> dap_ans { get; set; } = new List<dap_an>();
    }
}