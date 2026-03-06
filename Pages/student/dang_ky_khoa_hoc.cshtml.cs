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
        public List<webhoctienganh.Models.khoa_hoc> DsKhoaHoc { get; set; } = new();
        public int? KhoaHocIdFilter { get; set; }

        public IActionResult OnGet(int? khoaHocId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            KhoaHocIdFilter = khoaHocId;
            LoadDanhSach(khoaHocId);
            return Page();
        }

        public IActionResult OnPostDangKy(int lopId, int? khoaHocId)
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
                LoadDanhSach(khoaHocId);
                return Page();
            }

            var soLuongHienTai = _db.dang_ky
                .Count(d => d.ma_lop_hoc == lopId && d.trang_thai != "Cancelled");
            if (soLuongHienTai >= lop.so_luong_toi_da)
            {
                ThongBao = "Lop hoc da day, khong the dang ky!";
                LoadDanhSach(khoaHocId);
                return Page();
            }

            var existing = _db.dang_ky.FirstOrDefault(d => d.ma_lop_hoc == lopId && d.ma_hoc_vien == maHocVien);
            if (existing != null)
            {
                if (existing.trang_thai == "Cancelled")
                {
                    // Cho phep dang ky lai
                    existing.trang_thai = "Pending";
                    existing.ngay_dang_ky = DateTime.Now;
                    _db.SaveChanges();

                    ThongBao = "Dang ky lai thanh cong! Vui long cho admin duyet.";
                    LoadDanhSach(khoaHocId);
                    return Page();
                }

                ThongBao = "Ban da dang ky lop nay!";
                LoadDanhSach(khoaHocId);
                return Page();
            }

            var dk = new Models.dang_ky_model
            {
                ma_hoc_vien = maHocVien,
                ma_lop_hoc = lopId,
                ngay_dang_ky = DateTime.Now,
                trang_thai = "Pending"
            };

            _db.dang_ky.Add(dk);
            _db.SaveChanges();

            ThongBao = "Dang ky thanh cong! Vui long cho admin duyet.";
            LoadDanhSach(khoaHocId);
            return Page();
        }

        public IActionResult OnPostHuy(int lopId, int? khoaHocId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var dk = _db.dang_ky.FirstOrDefault(d => d.ma_lop_hoc == lopId && d.ma_hoc_vien == maHocVien);
            if (dk == null)
            {
                ThongBao = "Dang ky khong ton tai!";
                LoadDanhSach(khoaHocId);
                return Page();
            }

            if (dk.trang_thai == "DaThanhToan")
            {
                ThongBao = "Khong the huy vi da thanh toan!";
                LoadDanhSach(khoaHocId);
                return Page();
            }

            // Idempotent: neu da Cancelled thi chi giu nguyen, neu Pending/ChoThanhToan thi huy
            if (dk.trang_thai != "Cancelled")
            {
                dk.trang_thai = "Cancelled";

                var hoaDon = _db.hoa_don.FirstOrDefault(h => h.ma_dang_ky == dk.ma_dang_ky);
                if (hoaDon != null && hoaDon.trang_thai == "ChuaThanhToan")
                {
                    hoaDon.trang_thai = "Huy";
                }

                _db.SaveChanges();
                ThongBao = "Da huy dang ky!";
            }
            else
            {
                ThongBao = "Dang ky da bi huy truoc do.";
            }

            LoadDanhSach(khoaHocId);
            return Page();
        }

        private void LoadDanhSach(int? khoaHocId)
        {
            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            DsKhoaHoc = _db.khoa_hoc.ToList();

            var openLops = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Include(l => l.giao_vien)
                .Where(l => l.trang_thai == "open");

            if (khoaHocId.HasValue)
            {
                openLops = openLops.Where(l => l.ma_khoa_hoc == khoaHocId.Value);
            }

            var openLopsList = openLops.ToList();

            var dangKys = _db.dang_ky
                .Where(d => d.ma_hoc_vien == maHocVien)
                .ToList();

            DanhSach = openLopsList.Select(l =>
            {
                var dk = dangKys.FirstOrDefault(d => d.ma_lop_hoc == l.ma_lop_hoc);
                var trangThai = dk == null ? "Chua dang ky" : dk.trang_thai;

                return new LopHocView
                {
                    MaLop = l.ma_lop_hoc,
                    TenKhoaHoc = l.khoa_hoc != null ? l.khoa_hoc.ten_khoa_hoc : "",
                    TrinhDo = l.khoa_hoc != null ? l.khoa_hoc.trinh_do : "",
                    HocPhi = l.khoa_hoc != null ? l.khoa_hoc.hoc_phi : 0,
                    TenGiaoVien = l.giao_vien != null ? l.giao_vien.ho_ten : "",
                    NgayBatDau = l.ngay_bat_dau,
                    NgayKetThuc = l.ngay_ket_thuc,
                    SoLuongDaDangKy = _db.dang_ky.Count(d => d.ma_lop_hoc == l.ma_lop_hoc && d.trang_thai != "Cancelled"),
                    SoLuongToiDa = l.so_luong_toi_da,
                    TrangThaiDangKy = trangThai
                };
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
            public int SoLuongToiDa { get; set; }
            public string TrangThaiDangKy { get; set; } = "";
        }
    }
}