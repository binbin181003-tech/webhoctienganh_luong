using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class thong_tin_ca_nhanModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public thong_tin_ca_nhanModel(du_lieu db)
        {
            _db = db;
        }

        public nguoi_dung CurrentUser { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var id = HttpContext.Session.GetString("ma_nguoi_dung");
            var user = _db.nguoi_dung.Find(id);

            if (user == null)
            {
                ThongBao = "Khong tim thay thong tin!";
                return Page();
            }

            CurrentUser = user;
            return Page();
        }
    }
}