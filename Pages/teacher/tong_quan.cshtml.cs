using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.teacher
{
    public class tong_quanModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public tong_quanModel(du_lieu db)
        {
            _db = db;
        }

        public int TongBaiHoc { get; set; }
        public int TongHocVien { get; set; }
        public int TongBaiKiemTra { get; set; }
        public int TongLop { get; set; }
        public string Email { get; set; } = "";
        public string AnhDaiDien { get; set; } = "";
        public List<LopGanDayItem> LopGanDay { get; set; } = new();
        public List<BaiKiemTraGanDayItem> BaiKiemTraGanDay { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            Email = HttpContext.Session.GetString("email") ?? "";
            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var teacher = _db.nguoi_dung.Find(maGV);
            AnhDaiDien = teacher?.anh_dai_dien ?? "";

            var lopIds = _db.lop_hoc.Where(l => l.ma_giao_vien == maGV).Select(l => l.ma_lop_hoc).ToList();
            var khoaHocIds = _db.lop_hoc.Where(l => l.ma_giao_vien == maGV).Select(l => l.ma_khoa_hoc).Distinct().ToList();

            TongLop = lopIds.Count;
            TongBaiHoc = _db.bai_hoc.Count(b => khoaHocIds.Contains(b.ma_khoa_hoc));
            TongBaiKiemTra = _db.bai_kiem_tra.Count(b => khoaHocIds.Contains(b.ma_khoa_hoc));
            TongHocVien = _db.dang_ky.Count(d => lopIds.Contains(d.ma_lop_hoc) && d.trang_thai == "DaThanhToan");

            LopGanDay = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Where(l => l.ma_giao_vien == maGV)
                .OrderBy(l => l.ngay_bat_dau)
                .Take(3)
                .Select(l => new LopGanDayItem
                {
                    MaLop = l.ma_lop_hoc,
                    TenKhoaHoc = l.khoa_hoc != null ? l.khoa_hoc.ten_khoa_hoc : "",
                    NgayBatDau = l.ngay_bat_dau,
                    NgayKetThuc = l.ngay_ket_thuc,
                    TrangThai = l.trang_thai
                }).ToList();

            BaiKiemTraGanDay = _db.bai_kiem_tra
                .Include(b => b.khoa_hoc)
                .Where(b => khoaHocIds.Contains(b.ma_khoa_hoc))
                .OrderByDescending(b => b.ma_bai_kiem_tra)
                .Take(3)
                .Select(b => new BaiKiemTraGanDayItem
                {
                    MaBai = b.ma_bai_kiem_tra,
                    TieuDe = b.tieu_de,
                    KhoaHoc = b.khoa_hoc != null ? b.khoa_hoc.ten_khoa_hoc : "",
                    ThoiLuong = b.thoi_luong_phut
                }).ToList();

            return Page();
        }

        public class LopGanDayItem
        {
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public string TrangThai { get; set; } = "";
        }

        public class BaiKiemTraGanDayItem
        {
            public int MaBai { get; set; }
            public string TieuDe { get; set; } = "";
            public string KhoaHoc { get; set; } = "";
            public int ThoiLuong { get; set; }
        }
    }
}