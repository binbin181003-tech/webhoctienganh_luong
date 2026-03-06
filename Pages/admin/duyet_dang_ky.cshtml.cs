using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.admin
{
    public class duyet_dang_kyModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public duyet_dang_kyModel(du_lieu db)
        {
            _db = db;
        }

        public string ThongBao { get; set; } = "";
        public List<DangKyView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostDuyet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var dk = _db.dang_ky
                .Include(d => d.lop_hoc)
                .ThenInclude(l => l!.khoa_hoc)
                .FirstOrDefault(d => d.ma_dang_ky == id);

            if (dk == null)
            {
                ThongBao = "Dang ky khong ton tai!";
                LoadDanhSach();
                return Page();
            }

            if (dk.trang_thai == "Pending")
            {
                dk.trang_thai = "ChoThanhToan";

                // === FIX #1 + #2: Tao hoa don khi admin duyet, dung entity da track ===
                var hoaDon = new Models.hoa_don
                {
                    ma_dang_ky = dk.ma_dang_ky, // dung truc tiep, khong query lai
                    so_tien = dk.lop_hoc?.khoa_hoc?.hoc_phi ?? 0,
                    ngay_tao = DateTime.Now,
                    trang_thai = "ChuaThanhToan"
                };
                _db.hoa_don.Add(hoaDon);

                _db.SaveChanges();
                ThongBao = "Da duyet dang ky va tao hoa don (Cho thanh toan)!";
            }
            else
            {
                ThongBao = $"Khong the duyet: trang thai hien tai la '{dk.trang_thai}'";
            }

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostHuy(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var dk = _db.dang_ky.FirstOrDefault(d => d.ma_dang_ky == id);
            if (dk != null)
            {
                dk.trang_thai = "Cancelled";

                // Huy hoa don lien quan (neu co)
                var hoaDon = _db.hoa_don.FirstOrDefault(h => h.ma_dang_ky == dk.ma_dang_ky);
                if (hoaDon != null && hoaDon.trang_thai == "ChuaThanhToan")
                {
                    hoaDon.trang_thai = "Huy";
                }

                _db.SaveChanges();
                ThongBao = "Da huy dang ky!";
            }

            LoadDanhSach();
            return Page();
        }

        private void LoadDanhSach()
        {
            DanhSach = _db.dang_ky
                .Include(d => d.hoc_vien)
                .Include(d => d.lop_hoc)
                .ThenInclude(l => l!.khoa_hoc)
                .Select(d => new DangKyView
                {
                    MaDangKy = d.ma_dang_ky,
                    HoTen = d.hoc_vien != null ? d.hoc_vien.ho_ten : "",
                    Email = d.hoc_vien != null ? d.hoc_vien.email : "",
                    MaLop = d.ma_lop_hoc,
                    TenKhoaHoc = d.lop_hoc != null && d.lop_hoc.khoa_hoc != null ? d.lop_hoc.khoa_hoc.ten_khoa_hoc : "",
                    NgayDangKy = d.ngay_dang_ky,
                    TrangThai = d.trang_thai
                })
                .ToList();
        }

        public class DangKyView
        {
            public int MaDangKy { get; set; }
            public string HoTen { get; set; } = "";
            public string Email { get; set; } = "";
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public DateTime NgayDangKy { get; set; }
            public string TrangThai { get; set; } = "";
        }
    }
}