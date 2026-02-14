using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("vai_tro")]
    public class vai_tro
    {
        [Key]
        public string ma_vai_tro { get; set; } = "";
        public string ten_vai_tro { get; set; } = ""; // user, teacher, admin

        // Navigation
        public ICollection<nguoi_dung_vai_tro> nguoi_dungs { get; set; } = new List<nguoi_dung_vai_tro>();
    }
}