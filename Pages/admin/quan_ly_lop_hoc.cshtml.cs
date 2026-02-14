using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class quan_ly_lop_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_lop_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<lop_hoc> DanhSach { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            DanhSach = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Include(l => l.giao_vien)
                .ToList();

            return Page();
        }

        public IActionResult OnPostXoa(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var lh = _db.lop_hoc.Find(id);
            if (lh != null)
            {
                bool coDangKy = _db.dang_ky.Any(d => d.ma_lop_hoc == id);
                bool coLichHoc = _db.lich_hoc.Any(l => l.ma_lop_hoc == id);

                if (coDangKy || coLichHoc)
                {
                    ThongBao = "Khong the xoa lop hoc vi con du lieu lien quan!";
                }
                else
                {
                    _db.lop_hoc.Remove(lh);
                    _db.SaveChanges();
                    ThongBao = "Da xoa lop hoc!";
                }
            }

            DanhSach = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Include(l => l.giao_vien)
                .ToList();

            return Page();
        }
    }
}