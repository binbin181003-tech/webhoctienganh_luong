using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class danh_gia_khoa_hocModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public danh_gia_khoa_hocModel(du_lieu db)
        {
            _db = db;
        }

        public khoa_hoc? KhoaHoc { get; set; }
        public string ThongBao { get; set; } = "";
        public bool DaCoDanhGia { get; set; }
        public int SoSao { get; set; } = 5;
        public string NoiDung { get; set; } = "";

        public IActionResult OnGet(int khoaHocId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenDanhGia(khoaHocId))
            {
                ThongBao = "Ban khong du dieu kien danh gia khoa hoc nay!";
                return Page();
            }

            KhoaHoc = _db.khoa_hoc.Find(khoaHocId);
            if (KhoaHoc == null)
            {
                ThongBao = "Khoa hoc khong ton tai!";
                return Page();
            }

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";
            var dg = _db.danh_gia.FirstOrDefault(d => d.ma_khoa_hoc == khoaHocId && d.ma_hoc_vien == maHocVien);

            if (dg != null)
            {
                DaCoDanhGia = true;
                SoSao = dg.so_sao;
                NoiDung = dg.noi_dung;
            }

            return Page();
        }

        public IActionResult OnPost(int khoaHocId, int so_sao, string noi_dung)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenDanhGia(khoaHocId))
            {
                ThongBao = "Ban khong du dieu kien danh gia khoa hoc nay!";
                return Page();
            }

            if (so_sao < 1 || so_sao > 5)
            {
                ThongBao = "So sao khong hop le!";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(noi_dung))
            {
                ThongBao = "Noi dung khong duoc de trong!";
                return Page();
            }

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";
            var dg = _db.danh_gia.FirstOrDefault(d => d.ma_khoa_hoc == khoaHocId && d.ma_hoc_vien == maHocVien);

            if (dg == null)
            {
                _db.danh_gia.Add(new danh_gia
                {
                    ma_hoc_vien = maHocVien,
                    ma_khoa_hoc = khoaHocId,
                    so_sao = so_sao,
                    noi_dung = noi_dung.Trim(),
                    ngay_danh_gia = DateTime.Now
                });
            }
            else
            {
                dg.so_sao = so_sao;
                dg.noi_dung = noi_dung.Trim();
                dg.ngay_danh_gia = DateTime.Now;
            }

            _db.SaveChanges();

            ThongBao = "Da luu danh gia!";
            return RedirectToPage("/student/danh_gia_khoa_hoc", new { khoaHocId });
        }

        public IActionResult OnPostXoa(int khoaHocId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";
            var dg = _db.danh_gia.FirstOrDefault(d => d.ma_khoa_hoc == khoaHocId && d.ma_hoc_vien == maHocVien);

            if (dg != null)
            {
                _db.danh_gia.Remove(dg);
                _db.SaveChanges();
            }

            return RedirectToPage("/student/danh_gia_khoa_hoc", new { khoaHocId });
        }

        private bool KiemTraQuyenDanhGia(int khoaHocId)
        {
            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var khoaHocIds = _db.dang_ky
                .Where(d => d.ma_hoc_vien == maHocVien && d.trang_thai == "DaThanhToan")
                .Include(d => d.lop_hoc)
                .Select(d => d.lop_hoc!.ma_khoa_hoc)
                .Distinct()
                .ToList();

            return khoaHocIds.Contains(khoaHocId);
        }
    }
}