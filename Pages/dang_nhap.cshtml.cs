using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webhoctienganh.Data;
using Microsoft.EntityFrameworkCore;

namespace webhoctienganh.Pages
{
    public class dang_nhapModel : PageModel
    {
        private readonly du_lieu _db;

        public dang_nhapModel(du_lieu db)
        {
            _db = db;
        }

        public string ThongBao { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost(string email, string mat_khau)
        {
            // === Validate backend ===
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(mat_khau))
            {
                ThongBao = "Vui long nhap day du email va mat khau!";
                return Page();
            }

            // === Tim user theo email ===
            var user = _db.nguoi_dung.FirstOrDefault(n => n.email == email.Trim().ToLower());

            if (user == null)
            {
                ThongBao = "Sai email hoac mat khau!";
                return Page();
            }

            // === Check trang thai tai khoan ===
            if (user.trang_thai != "active")
            {
                ThongBao = "Tai khoan da bi khoa!";
                return Page();
            }

            // === Verify password hash ===
            if (!BCrypt.Net.BCrypt.Verify(mat_khau, user.mat_khau_hash))
            {
                ThongBao = "Sai email hoac mat khau!";
                return Page();
            }

            // === Lay role cua user ===
            var vaiTro = _db.nguoi_dung_vai_tro
                .Where(nv => nv.ma_nguoi_dung == user.ma_nguoi_dung)
                .Select(nv => nv.ma_vai_tro)
                .FirstOrDefault() ?? "user";

            // === Luu session ===
            HttpContext.Session.SetString("ma_nguoi_dung", user.ma_nguoi_dung);
            HttpContext.Session.SetString("ho_ten", user.ho_ten);
            HttpContext.Session.SetString("email", user.email);
            HttpContext.Session.SetString("role", vaiTro);

            // === Redirect theo role ===
            return vaiTro switch
            {
                "admin" => RedirectToPage("/admin/tong_quan"),
                "teacher" => RedirectToPage("/teacher/tong_quan"),
                _ => RedirectToPage("/trang_chu")
            };
        }
    }
}