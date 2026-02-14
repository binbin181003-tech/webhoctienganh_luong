using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.admin
{
    public class danh_sach_hoc_vienModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public danh_sach_hoc_vienModel(du_lieu db)
        {
            _db = db;
        }

        public int MaLop { get; set; }
        public string TenLop { get; set; } = "";
        public string TenKhoaHoc { get; set; } = "";
        public string TenGiaoVien { get; set; } = "";
        public List<HocVienView> DanhSach { get; set; } = new();

        public IActionResult OnGet(int lop)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            MaLop = lop;

            // Lay thong tin lop
            var lopHoc = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Include(l => l.giao_vien)
                .FirstOrDefault(l => l.ma_lop_hoc == lop);

            if (lopHoc != null)
            {
                TenLop = $"Lop {lopHoc.ma_lop_hoc}";
                TenKhoaHoc = lopHoc.khoa_hoc?.ten_khoa_hoc ?? "";
                TenGiaoVien = lopHoc.giao_vien?.ho_ten ?? "";
            }

            // JOIN: dang_ky -> nguoi_dung
            DanhSach = _db.dang_ky
                .Where(d => d.ma_lop_hoc == lop)
                .Include(d => d.hoc_vien)
                .Select(d => new HocVienView
                {
                    HoTen = d.hoc_vien != null ? d.hoc_vien.ho_ten : "",
                    Email = d.hoc_vien != null ? d.hoc_vien.email : "",
                    SoDienThoai = d.hoc_vien != null ? d.hoc_vien.so_dien_thoai : "",
                    NgayDangKy = d.ngay_dang_ky,
                    TrangThai = d.trang_thai
                })
                .ToList();

            return Page();
        }

        public class HocVienView
        {
            public string HoTen { get; set; } = "";
            public string Email { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
            public DateTime NgayDangKy { get; set; }
            public string TrangThai { get; set; } = "";
        }
    }
}