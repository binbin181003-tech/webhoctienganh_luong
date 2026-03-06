using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.admin
{
    public class quan_ly_lich_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_lich_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<LichHocView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            DanhSach = _db.lich_hoc
                .Include(l => l.lop_hoc)
                .ThenInclude(lh => lh!.khoa_hoc)
                .Select(l => new LichHocView
                {
                    MaLich = l.ma_lich_hoc,
                    MaLop = l.ma_lop_hoc,
                    TenKhoaHoc = l.lop_hoc != null && l.lop_hoc.khoa_hoc != null ? l.lop_hoc.khoa_hoc.ten_khoa_hoc : "",
                    ThuTrongTuan = l.thu_trong_tuan,
                    GioBatDau = l.gio_bat_dau,
                    GioKetThuc = l.gio_ket_thuc,
                    PhongHoc = l.phong_hoc
                })
                .ToList();

            return Page();
        }

        public IActionResult OnPostXoa(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var lich = _db.lich_hoc.Find(id);
            if (lich != null)
            {
                _db.lich_hoc.Remove(lich);
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

        public class LichHocView
        {
            public int MaLich { get; set; }
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public string ThuTrongTuan { get; set; } = "";
            public string GioBatDau { get; set; } = "";
            public string GioKetThuc { get; set; } = "";
            public string PhongHoc { get; set; } = "";
        }
    }
}