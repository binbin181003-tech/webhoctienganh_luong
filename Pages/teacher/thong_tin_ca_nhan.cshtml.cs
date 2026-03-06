using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class thong_tin_ca_nhanModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;
        private readonly IWebHostEnvironment _env;

        public thong_tin_ca_nhanModel(du_lieu db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public nguoi_dung CurrentUser { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!LoadUser()) return Page();
            return Page();
        }

        public IActionResult OnPostUploadAvatar(IFormFile anh_dai_dien)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!LoadUser()) return Page();
            if (anh_dai_dien == null || anh_dai_dien.Length == 0)
            {
                ThongBao = "Chon file anh hop le!";
                return Page();
            }

            var uploads = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anh_dai_dien.FileName)}";
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                anh_dai_dien.CopyTo(stream);
            }

            if (!string.IsNullOrEmpty(CurrentUser.anh_dai_dien))
            {
                var oldPath = Path.Combine(_env.WebRootPath, CurrentUser.anh_dai_dien.TrimStart('/'));
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

            CurrentUser.anh_dai_dien = $"/uploads/avatars/{fileName}";
            _db.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteAvatar()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!LoadUser()) return Page();

            if (!string.IsNullOrEmpty(CurrentUser.anh_dai_dien))
            {
                var oldPath = Path.Combine(_env.WebRootPath, CurrentUser.anh_dai_dien.TrimStart('/'));
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                CurrentUser.anh_dai_dien = "";
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

        private bool LoadUser()
        {
            var id = HttpContext.Session.GetString("ma_nguoi_dung");
            var user = _db.nguoi_dung.Find(id);
            if (user == null)
            {
                ThongBao = "Khong tim thay thong tin!";
                return false;
            }
            CurrentUser = user;
            return true;
        }
    }
}