using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class hoa_donModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public hoa_donModel(du_lieu db)
        {
            _db = db;
        }

        public List<HoaDonView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostThanhToan(int hoaDonId, string phuong_thuc_thanh_toan)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var hoaDon = _db.hoa_don
                .Include(h => h.dang_ky)
                .FirstOrDefault(h => h.ma_hoa_don == hoaDonId);

            if (hoaDon == null || hoaDon.trang_thai == "DaThanhToan")
            {
                LoadDanhSach();
                return Page();
            }

            // Tao thanh toan
            _db.thanh_toan.Add(new thanh_toan
            {
                ma_hoa_don = hoaDon.ma_hoa_don,
                ngay_thanh_toan = DateTime.Now,
                phuong_thuc_thanh_toan = phuong_thuc_thanh_toan,
                trang_thai = "completed"
            });

            hoaDon.trang_thai = "DaThanhToan";

            if (hoaDon.dang_ky != null)
            {
                hoaDon.dang_ky.trang_thai = "DaThanhToan";
            }

            _db.SaveChanges();
            LoadDanhSach();
            return Page();
        }

        private void LoadDanhSach()
        {
            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            DanhSach = _db.hoa_don
                .Include(h => h.dang_ky)
                .ThenInclude(d => d!.lop_hoc)
                .ThenInclude(l => l!.khoa_hoc)
                .Where(h => h.dang_ky != null && h.dang_ky.ma_hoc_vien == maHocVien)
                .Select(h => new HoaDonView
                {
                    MaHoaDon = h.ma_hoa_don,
                    TenKhoaHoc = h.dang_ky != null && h.dang_ky.lop_hoc != null && h.dang_ky.lop_hoc.khoa_hoc != null
                        ? h.dang_ky.lop_hoc.khoa_hoc.ten_khoa_hoc
                        : "",
                    SoTien = h.so_tien,
                    TrangThai = h.trang_thai
                })
                .ToList();
        }

        public class HoaDonView
        {
            public int MaHoaDon { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public decimal SoTien { get; set; }
            public string TrangThai { get; set; } = "";
        }
    }
}