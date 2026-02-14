using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class bai_hocModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public bai_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<bai_hoc> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var khoaHocIds = _db.dang_ky
                .Where(d => d.ma_hoc_vien == maHocVien && d.trang_thai == "DaThanhToan")
                .Include(d => d.lop_hoc)
                .Select(d => d.lop_hoc!.ma_khoa_hoc)
                .Distinct()
                .ToList();

            DanhSach = _db.bai_hoc
                .Include(b => b.khoa_hoc)
                .Where(b => khoaHocIds.Contains(b.ma_khoa_hoc))
                .ToList();

            return Page();
        }
    }
}