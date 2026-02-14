using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.admin
{
    public class quan_ly_thanh_toanModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_thanh_toanModel(du_lieu db)
        {
            _db = db;
        }

        public List<ThanhToanView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            DanhSach = _db.thanh_toan
                .Include(t => t.hoa_don)
                .ThenInclude(h => h!.dang_ky)
                .ThenInclude(d => d!.hoc_vien)
                .Include(t => t.hoa_don)
                .ThenInclude(h => h!.dang_ky)
                .ThenInclude(d => d!.lop_hoc)
                .ThenInclude(l => l!.khoa_hoc)
                .Select(t => new ThanhToanView
                {
                    HoTen = t.hoa_don != null && t.hoa_don.dang_ky != null && t.hoa_don.dang_ky.hoc_vien != null
                        ? t.hoa_don.dang_ky.hoc_vien.ho_ten : "",
                    TenKhoaHoc = t.hoa_don != null && t.hoa_don.dang_ky != null && t.hoa_don.dang_ky.lop_hoc != null && t.hoa_don.dang_ky.lop_hoc.khoa_hoc != null
                        ? t.hoa_don.dang_ky.lop_hoc.khoa_hoc.ten_khoa_hoc : "",
                    SoTien = t.hoa_don != null ? t.hoa_don.so_tien : 0,
                    NgayThanhToan = t.ngay_thanh_toan,
                    PhuongThuc = t.phuong_thuc_thanh_toan
                })
                .ToList();

            return Page();
        }

        public class ThanhToanView
        {
            public string HoTen { get; set; } = "";
            public string TenKhoaHoc { get; set; } = "";
            public decimal SoTien { get; set; }
            public DateTime NgayThanhToan { get; set; }
            public string PhuongThuc { get; set; } = "";
        }
    }
}