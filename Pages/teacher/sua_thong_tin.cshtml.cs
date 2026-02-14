using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class sua_thong_tinModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public sua_thong_tinModel(du_lieu db)
        {
            _db = db;
        }

        public string Field { get; set; } = "";
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(string field)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            Field = field;
            return Page();
        }

        public IActionResult OnPost(string field, string gia_tri_cu, string gia_tri_moi, string xac_nhan)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var id = HttpContext.Session.GetString("ma_nguoi_dung");
            var user = _db.nguoi_dung.Find(id);

            if (user == null)
            {
                ThongBao = "Khong tim thay nguoi dung!";
                return Page();
            }

            if (gia_tri_moi != xac_nhan)
            {
                ThongBao = "Xac nhan khong khop!";
                return Page();
            }

            if (field == "mat_khau")
            {
                if (!BCrypt.Net.BCrypt.Verify(gia_tri_cu, user.mat_khau_hash))
                {
                    ThongBao = "Mat khau cu khong dung!";
                    return Page();
                }

                if (gia_tri_moi.Length < 6)
                {
                    ThongBao = "Mat khau moi phai >= 6 ky tu!";
                    return Page();
                }

                user.mat_khau_hash = BCrypt.Net.BCrypt.HashPassword(gia_tri_moi);
            }
            else
            {
                string current = field switch
                {
                    "ho_ten" => user.ho_ten,
                    "so_dien_thoai" => user.so_dien_thoai,
                    "anh_dai_dien" => user.anh_dai_dien,
                    _ => ""
                };

                if (current != gia_tri_cu)
                {
                    ThongBao = "Gia tri cu khong dung!";
                    return Page();
                }

                if (field == "ho_ten") user.ho_ten = gia_tri_moi;
                if (field == "so_dien_thoai") user.so_dien_thoai = gia_tri_moi;
                if (field == "anh_dai_dien") user.anh_dai_dien = gia_tri_moi;
            }

            _db.SaveChanges();
            return RedirectToPage("/teacher/thong_tin_ca_nhan");
        }
    }
}