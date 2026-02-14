using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages
{
    public class dang_kyModel : PageModel
    {
        private readonly du_lieu _db;

        public dang_kyModel(du_lieu db)
        {
            _db = db;
        }

        public string ThongBao { get; set; } = "";
        public string ThanhCong { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost(string ho_ten, string email, string so_dien_thoai, string mat_khau, string nhap_lai_mat_khau)
        {
            if (string.IsNullOrWhiteSpace(ho_ten))
            {
                ThongBao = "Ho ten khong duoc de trong!";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ThongBao = "Email khong duoc de trong!";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(so_dien_thoai))
            {
                ThongBao = "So dien thoai khong duoc de trong!";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(mat_khau))
            {
                ThongBao = "Mat khau khong duoc de trong!";
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

            var user = new nguoi_dung
            {
                ma_nguoi_dung = Guid.NewGuid().ToString(),
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
                ma_nguoi_dung = user.ma_nguoi_dung,
                ma_vai_tro = "user"
            });

            _db.SaveChanges();

            return RedirectToPage("/dang_nhap");
        }
    }
}