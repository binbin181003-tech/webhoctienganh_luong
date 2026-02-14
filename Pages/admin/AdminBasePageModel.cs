using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webhoctienganh.Pages.admin
{
    public class AdminBasePageModel : PageModel
    {
        public string HoTen { get; set; } = "";
        public string MaNguoiDung { get; set; } = "";

        public IActionResult? KiemTraQuyen()
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "admin")
            {
                return RedirectToPage("/dang_nhap");
            }
            HoTen = HttpContext.Session.GetString("ho_ten") ?? "";
            MaNguoiDung = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";
            return null; // OK, co quyen
        }
    }
}