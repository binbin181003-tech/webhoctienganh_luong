using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class sua_lich_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public sua_lich_hocModel(du_lieu db)
        {
            _db = db;
        }

        public lich_hoc? LichHoc { get; set; }
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LichHoc = _db.lich_hoc.Find(id);
            return Page();
        }

        public IActionResult OnPost(int id, string thu_trong_tuan, string gio_bat_dau, string gio_ket_thuc, string phong_hoc)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var lich = _db.lich_hoc.Find(id);
            if (lich == null)
            {
                ThongBao = "Lich hoc khong ton tai!";
                return Page();
            }

            lich.thu_trong_tuan = thu_trong_tuan.Trim();
            lich.gio_bat_dau = gio_bat_dau.Trim();
            lich.gio_ket_thuc = gio_ket_thuc.Trim();
            lich.phong_hoc = phong_hoc.Trim();

            _db.SaveChanges();
            return RedirectToPage("/admin/quan_ly_lich_hoc");
        }
    }
}