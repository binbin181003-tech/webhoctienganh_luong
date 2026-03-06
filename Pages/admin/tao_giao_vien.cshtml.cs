using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class tao_giao_vienModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public tao_giao_vienModel(du_lieu db)
        {
            _db = db;
        }

        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;
            return Page();
        }

        public IActionResult OnPost(string ho_ten, string email, string so_dien_thoai, string mat_khau, string nhap_lai_mat_khau)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            // Validate
            if (string.IsNullOrWhiteSpace(ho_ten) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(so_dien_thoai) ||
                string.IsNullOrWhiteSpace(mat_khau) ||
                string.IsNullOrWhiteSpace(nhap_lai_mat_khau))
            {
                ThongBao = "Vui long nhap day du thong tin!";
                return Page();
            }

            if (mat_khau.Length < 6)
            {
                ThongBao = "Mat khau phai co it nhat 6 ky tu!";
                return Page();
            }

            if (mat_khau != nhap_lai_mat_khau)
            {
                ThongBao = "Mat khau khong khop!";
                return Page();
            }

            var normalizedEmail = email.Trim().ToLower();
            var existing = _db.nguoi_dung.FirstOrDefault(n => n.email == normalizedEmail);
            if (existing != null)
            {
                ThongBao = "Email da ton tai!";
                return Page();
            }

            var teacherId = Guid.NewGuid().ToString();
            var user = new nguoi_dung
            {
                ma_nguoi_dung = teacherId,
                ho_ten = ho_ten.Trim(),
                email = normalizedEmail,
                so_dien_thoai = so_dien_thoai.Trim(),
                mat_khau_hash = BCrypt.Net.BCrypt.HashPassword(mat_khau),
                ngay_tao = DateTime.Now,
                trang_thai = "active"
            };

            _db.nguoi_dung.Add(user);
            _db.nguoi_dung_vai_tro.Add(new nguoi_dung_vai_tro
            {
                ma_nguoi_dung = teacherId,
                ma_vai_tro = "teacher"
            });

            _db.SaveChanges();
            return RedirectToPage("/admin/quan_ly_giao_vien");
        }
    }
}