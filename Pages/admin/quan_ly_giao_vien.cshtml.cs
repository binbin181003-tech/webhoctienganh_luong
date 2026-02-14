using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.admin
{
    public class quan_ly_giao_vienModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_giao_vienModel(du_lieu db)
        {
            _db = db;
        }

        public List<UserView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostKhoa(string id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var user = _db.nguoi_dung.Find(id);
            if (user != null)
            {
                user.trang_thai = "inactive";
                _db.SaveChanges();
            }

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostMoKhoa(string id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var user = _db.nguoi_dung.Find(id);
            if (user != null)
            {
                user.trang_thai = "active";
                _db.SaveChanges();
            }

            LoadDanhSach();
            return Page();
        }

        private void LoadDanhSach()
        {
            var gvIds = _db.nguoi_dung_vai_tro
                .Where(v => v.ma_vai_tro == "teacher")
                .Select(v => v.ma_nguoi_dung)
                .ToList();

            DanhSach = _db.nguoi_dung
                .Where(u => gvIds.Contains(u.ma_nguoi_dung))
                .Select(u => new UserView
                {
                    MaNguoiDung = u.ma_nguoi_dung,
                    HoTen = u.ho_ten,
                    Email = u.email,
                    SoDienThoai = u.so_dien_thoai,
                    TrangThai = u.trang_thai
                })
                .ToList();
        }

        public class UserView
        {
            public string MaNguoiDung { get; set; } = "";
            public string HoTen { get; set; } = "";
            public string Email { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
            public string TrangThai { get; set; } = "";
        }
    }
}