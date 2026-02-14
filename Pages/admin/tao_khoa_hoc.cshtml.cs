using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class tao_khoa_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public tao_khoa_hocModel(du_lieu db)
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

        public IActionResult OnPost(string ten_khoa_hoc, string mo_ta, decimal hoc_phi, string trinh_do, int thoi_luong_tuan)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            // Validate
            if (string.IsNullOrWhiteSpace(ten_khoa_hoc))
            {
                ThongBao = "Ten khoa hoc khong duoc de trong!";
                return Page();
            }

            if (hoc_phi < 0)
            {
                ThongBao = "Hoc phi phai >= 0!";
                return Page();
            }

            string[] trinhDoHopLe = { "beginner", "intermediate", "advanced" };
            if (!trinhDoHopLe.Contains(trinh_do))
            {
                ThongBao = "Trinh do khong hop le!";
                return Page();
            }

            if (thoi_luong_tuan < 1)
            {
                ThongBao = "Thoi luong phai >= 1 tuan!";
                return Page();
            }

            var kh = new khoa_hoc
            {
                ten_khoa_hoc = ten_khoa_hoc.Trim(),
                mo_ta = mo_ta?.Trim() ?? "",
                hoc_phi = hoc_phi,
                trinh_do = trinh_do,
                thoi_luong_tuan = thoi_luong_tuan,
                nguoi_tao = MaNguoiDung // admin dang login
            };

            _db.khoa_hoc.Add(kh);
            _db.SaveChanges();

            return RedirectToPage("/admin/quan_ly_khoa_hoc");
        }
    }
}