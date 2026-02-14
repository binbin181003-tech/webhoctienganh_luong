using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class sua_khoa_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public sua_khoa_hocModel(du_lieu db)
        {
            _db = db;
        }

        public khoa_hoc? KhoaHoc { get; set; }
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            KhoaHoc = _db.khoa_hoc.Find(id);
            return Page();
        }

        public IActionResult OnPost(int id, string ten_khoa_hoc, string mo_ta, decimal hoc_phi, string trinh_do, int thoi_luong_tuan)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var kh = _db.khoa_hoc.Find(id);
            if (kh == null)
            {
                ThongBao = "Khoa hoc khong ton tai!";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(ten_khoa_hoc))
            {
                ThongBao = "Ten khoa hoc khong duoc de trong!";
                KhoaHoc = kh;
                return Page();
            }

            kh.ten_khoa_hoc = ten_khoa_hoc.Trim();
            kh.mo_ta = mo_ta?.Trim() ?? "";
            kh.hoc_phi = hoc_phi;
            kh.trinh_do = trinh_do;
            kh.thoi_luong_tuan = thoi_luong_tuan;

            _db.SaveChanges();

            return RedirectToPage("/admin/quan_ly_khoa_hoc");
        }
    }
}