using Microsoft.AspNetCore.Mvc;

namespace webhoctienganh.Pages.student
{
    public class tong_quanModel : StudentBasePageModel
    {
        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;
            return Page();
        }

        public IActionResult OnPostDangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/trang_vang_lai");
        }
    }
}