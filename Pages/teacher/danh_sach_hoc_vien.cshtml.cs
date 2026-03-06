using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.teacher
{
    public class danh_sach_hoc_vienModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public danh_sach_hoc_vienModel(du_lieu db)
        {
            _db = db;
        }

        public int MaLop { get; set; }
        public string TenLop { get; set; } = "";
        public string TenKhoaHoc { get; set; } = "";
        public List<HocVienView> DanhSach { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int lop)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var lopHoc = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .FirstOrDefault(l => l.ma_lop_hoc == lop && l.ma_giao_vien == maGV);

            if (lopHoc == null)
            {
                ThongBao = "Lop khong ton tai hoac khong thuoc giao vien!";
                return Page();
            }

            MaLop = lop;
            TenLop = $"Lop {lopHoc.ma_lop_hoc}";
            TenKhoaHoc = lopHoc.khoa_hoc?.ten_khoa_hoc ?? "";

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