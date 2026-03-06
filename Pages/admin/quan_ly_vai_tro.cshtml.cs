using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class quan_ly_vai_troModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_vai_troModel(du_lieu db)
        {
            _db = db;
        }

        public List<UserView> DanhSach { get; set; } = new();
        public List<vai_tro> DanhSachVaiTro { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDanhSach();
            return Page();
        }

        public IActionResult OnPostCapNhatVaiTro(string id, string vai_tro)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var user = _db.nguoi_dung.Find(id);
            if (user == null)
            {
                ThongBao = "Nguoi dung khong ton tai!";
                LoadDanhSach();
                return Page();
            }

            var oldRoles = _db.nguoi_dung_vai_tro.Where(nv => nv.ma_nguoi_dung == id).ToList();
            _db.nguoi_dung_vai_tro.RemoveRange(oldRoles);

            _db.nguoi_dung_vai_tro.Add(new nguoi_dung_vai_tro
            {
                ma_nguoi_dung = id,
                ma_vai_tro = vai_tro
            });

            _db.SaveChanges();

            ThongBao = "Da cap nhat vai tro!";
            LoadDanhSach();
            return Page();
        }

        private void LoadDanhSach()
        {
            DanhSachVaiTro = _db.vai_tro.ToList();

            DanhSach = _db.nguoi_dung
                .Select(u => new UserView
                {
                    MaNguoiDung = u.ma_nguoi_dung,
                    HoTen = u.ho_ten,
                    Email = u.email,
                    VaiTro = _db.nguoi_dung_vai_tro
                        .Where(nv => nv.ma_nguoi_dung == u.ma_nguoi_dung)
                        .Select(nv => nv.ma_vai_tro)
                        .FirstOrDefault() ?? "user"
                })
                .ToList();
        }

        public class UserView
        {
            public string MaNguoiDung { get; set; } = "";
            public string HoTen { get; set; } = "";
            public string Email { get; set; } = "";
            public string VaiTro { get; set; } = "";
        }
    }
}