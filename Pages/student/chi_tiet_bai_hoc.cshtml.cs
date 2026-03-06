using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class chi_tiet_bai_hocModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public chi_tiet_bai_hocModel(du_lieu db)
        {
            _db = db;
        }

        public bai_hoc? BaiHoc { get; set; }
        public List<tai_lieu> DsTaiLieu { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            // === FIX #4: Kiem tra quyen truy cap bai hoc ===
            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var bai = _db.bai_hoc.Include(b => b.khoa_hoc).FirstOrDefault(b => b.ma_bai_hoc == id);
            if (bai == null)
            {
                ThongBao = "Bai hoc khong ton tai!";
                return Page();
            }

            var khoaHocIds = _db.dang_ky
                .Where(d => d.ma_hoc_vien == maHocVien && d.trang_thai == "DaThanhToan")
                .Include(d => d.lop_hoc)
                .Select(d => d.lop_hoc!.ma_khoa_hoc)
                .Distinct()
                .ToList();

            if (!khoaHocIds.Contains(bai.ma_khoa_hoc))
            {
                ThongBao = "Ban khong co quyen truy cap bai hoc nay!";
                return Page();
            }

            BaiHoc = bai;
            DsTaiLieu = _db.tai_lieu.Where(t => t.ma_bai_hoc == id).ToList();

            return Page();
        }
    }
}