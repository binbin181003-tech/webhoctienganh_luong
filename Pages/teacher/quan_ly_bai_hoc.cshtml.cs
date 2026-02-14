using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class quan_ly_bai_hocModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_bai_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<bai_hoc> DanhSach { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var khoaHocIds = _db.lop_hoc
                .Where(l => l.ma_giao_vien == maGV)
                .Select(l => l.ma_khoa_hoc)
                .Distinct()
                .ToList();

            DanhSach = _db.bai_hoc
                .Include(b => b.khoa_hoc)
                .Where(b => khoaHocIds.Contains(b.ma_khoa_hoc))
                .ToList();

            return Page();
        }

        public IActionResult OnPostXoa(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var baiHoc = _db.bai_hoc.Find(id);
            if (baiHoc != null)
            {
                var taiLieus = _db.tai_lieu.Where(t => t.ma_bai_hoc == id).ToList();
                _db.tai_lieu.RemoveRange(taiLieus);
                _db.bai_hoc.Remove(baiHoc);
                _db.SaveChanges();
                ThongBao = "Da xoa bai hoc!";
            }

            return RedirectToPage();
        }
    }
}