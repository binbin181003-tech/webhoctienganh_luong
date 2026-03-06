using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webhoctienganh.Data;

namespace webhoctienganh.Pages
{
    public class trang_chuModel : PageModel
    {
        private readonly du_lieu _db;
        public trang_chuModel(du_lieu db)
        {
            _db = db;
        }

        public string HoTen { get; set; } = "";
        public string Role { get; set; } = "";
        public string Email { get; set; } = "";
        public string Avatar { get; set; } = "";
        public string Initials => string.IsNullOrWhiteSpace(HoTen)
            ? "?" : new string(HoTen.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).Take(2).ToArray()).ToUpper();

        public IActionResult OnGet()
        {
            var maNguoiDung = HttpContext.Session.GetString("ma_nguoi_dung");
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToPage("/dang_nhap");
            }

            Role = HttpContext.Session.GetString("role") ?? "user";

            var user = _db.nguoi_dung.FirstOrDefault(n => n.ma_nguoi_dung == maNguoiDung);
            if (user != null)
            {
                HoTen = user.ho_ten;
                Email = user.email;
                Avatar = user.anh_dai_dien ?? "";
            }
            else
            {
                HoTen = HttpContext.Session.GetString("ho_ten") ?? "";
                Email = HttpContext.Session.GetString("email") ?? "";
            }

            return Page();
        }

        public IActionResult OnPostDangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/trang_vang_lai");
        }
    }
}