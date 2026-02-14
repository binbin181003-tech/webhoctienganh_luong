using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class danh_sach_bai_kiem_traModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public danh_sach_bai_kiem_traModel(du_lieu db)
        {
            _db = db;
        }

        public List<bai_kiem_tra> DanhSach { get; set; } = new();

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

            DanhSach = _db.bai_kiem_tra
                .Include(b => b.khoa_hoc)
                .Where(b => khoaHocIds.Contains(b.ma_khoa_hoc))
                .ToList();

            return Page();
        }
    }
}