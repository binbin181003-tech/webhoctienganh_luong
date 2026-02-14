using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhoctienganh.Models
{
    [Table("tai_lieu")]
    public class tai_lieu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ma_tai_lieu { get; set; }
        public int ma_bai_hoc { get; set; }
        public string duong_dan_file { get; set; } = "";
        public string loai_tai_lieu { get; set; } = ""; // pdf, video, audio, image

        // Navigation
        public bai_hoc? bai_hoc { get; set; }
    }
}