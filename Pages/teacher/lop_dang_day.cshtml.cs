using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.teacher
{
    public class lop_dang_dayModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public lop_dang_dayModel(du_lieu db)
        {
            _db = db;
        }

        public List<LopView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var lops = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Where(l => l.ma_giao_vien == maGV)
                .ToList();

            DanhSach = lops.Select(l => new LopView
            {
                MaLop = l.ma_lop_hoc,
                TenKhoaHoc = l.khoa_hoc != null ? l.khoa_hoc.ten_khoa_hoc : "",
                NgayBatDau = l.ngay_bat_dau,
                NgayKetThuc = l.ngay_ket_thuc,
                SoLuongToiDa = l.so_luong_toi_da,
                SoLuongDangKy = _db.dang_ky.Count(d => d.ma_lop_hoc == l.ma_lop_hoc && d.trang_thai != "Cancelled"),
                TrangThai = l.trang_thai
            }).ToList();

            return Page();
        }

        public class LopView
        {
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public int SoLuongToiDa { get; set; }
            public int SoLuongDangKy { get; set; }
            public string TrangThai { get; set; } = "";
        }
    }
}