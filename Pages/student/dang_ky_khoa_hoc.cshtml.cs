using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.student
{
    public class dang_ky_khoa_hocModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public dang_ky_khoa_hocModel(du_lieu db)
        {
            _db = db;
        }

        public string ThongBao { get; set; } = "";
        public List<LopHocView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostDangKy(int lopId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung");
            if (string.IsNullOrEmpty(maHocVien))
            {
                return RedirectToPage("/dang_nhap");
            }

            var lop = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .FirstOrDefault(l => l.ma_lop_hoc == lopId && l.trang_thai == "open");

            if (lop == null)
            {
                ThongBao = "Lop hoc khong ton tai hoac da dong!";
                LoadDanhSach();
                return Page();
            }

            var daDangKy = _db.dang_ky.Any(d => d.ma_lop_hoc == lopId && d.ma_hoc_vien == maHocVien);
            if (daDangKy)
            {
                ThongBao = "Ban da dang ky lop nay!";
                LoadDanhSach();
                return Page();
            }

            _db.dang_ky.Add(new Models.dang_ky_model
            {
                ma_hoc_vien = maHocVien,
                ma_lop_hoc = lopId,
                ngay_dang_ky = DateTime.Now,
                trang_thai = "ChoThanhToan"
            });
            _db.SaveChanges();

            // Tạo hóa đơn tự động
            var hd = new Models.hoa_don
            {
                ma_dang_ky = _db.dang_ky
                    .OrderByDescending(d => d.ma_dang_ky)
                    .First().ma_dang_ky,
                so_tien = lop.khoa_hoc?.hoc_phi ?? 0,
                ngay_tao = DateTime.Now,
                trang_thai = "ChuaThanhToan"
            };

            _db.hoa_don.Add(hd);
            _db.SaveChanges();

            ThongBao = "Dang ky thanh cong! Vui long cho duyet.";
            LoadDanhSach();
            return Page();
        }

        
        private void LoadDanhSach()
        {
            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var openLops = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Include(l => l.giao_vien)
                .Where(l => l.trang_thai == "open")
                .ToList();

            var dangKys = _db.dang_ky
                .Where(d => d.trang_thai != "cancelled")
                .ToList();

            DanhSach = openLops.Select(l => new LopHocView
            {
                MaLop = l.ma_lop_hoc,
                TenKhoaHoc = l.khoa_hoc != null ? l.khoa_hoc.ten_khoa_hoc : "",
                TrinhDo = l.khoa_hoc != null ? l.khoa_hoc.trinh_do : "",
                HocPhi = l.khoa_hoc != null ? l.khoa_hoc.hoc_phi : 0,
                TenGiaoVien = l.giao_vien != null ? l.giao_vien.ho_ten : "",
                NgayBatDau = l.ngay_bat_dau,
                NgayKetThuc = l.ngay_ket_thuc,
                SoLuongDaDangKy = dangKys.Count(d => d.ma_lop_hoc == l.ma_lop_hoc),
                DaDangKy = _db.dang_ky.Any(d => d.ma_lop_hoc == l.ma_lop_hoc && d.ma_hoc_vien == maHocVien)
            }).ToList();
        }

        public class LopHocView
        {
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public string TrinhDo { get; set; } = "";
            public string TenGiaoVien { get; set; } = "";
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public decimal HocPhi { get; set; }
            public int SoLuongDaDangKy { get; set; }
            public bool DaDangKy { get; set; }
        }
    }
}