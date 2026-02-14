using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class quan_ly_khoa_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_khoa_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<khoa_hoc> DanhSach { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            DanhSach = _db.khoa_hoc.ToList();
            return Page();
        }

        public IActionResult OnPostXoa(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var kh = _db.khoa_hoc.Find(id);
            if (kh != null)
            {
                bool coLopHoc = _db.lop_hoc.Any(l => l.ma_khoa_hoc == id);
                bool coBaiHoc = _db.bai_hoc.Any(b => b.ma_khoa_hoc == id);
                bool coBaiKiemTra = _db.bai_kiem_tra.Any(b => b.ma_khoa_hoc == id);

                if (coLopHoc || coBaiHoc || coBaiKiemTra)
                {
                    ThongBao = "Khong the xoa khoa hoc vi con du lieu lien quan!";
                }
                else
                {
                    _db.khoa_hoc.Remove(kh);
                    _db.SaveChanges();
                    ThongBao = "Da xoa khoa hoc!";
                }
            }

            DanhSach = _db.khoa_hoc.ToList();
            return Page();
        }
    }
}