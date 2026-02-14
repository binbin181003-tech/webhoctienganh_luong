using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webhoctienganh.Pages
{
    public class trang_chuModel : PageModel
    {
        public string HoTen { get; set; } = "";
        public string Role { get; set; } = "";

        public IActionResult OnGet()
        {
            var maNguoiDung = HttpContext.Session.GetString("ma_nguoi_dung");
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToPage("/dang_nhap");
            }

            HoTen = HttpContext.Session.GetString("ho_ten") ?? "";
            Role = HttpContext.Session.GetString("role") ?? "user";
            return Page();
        }

        public IActionResult OnPostDangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/trang_vang_lai");
        }
    }
}