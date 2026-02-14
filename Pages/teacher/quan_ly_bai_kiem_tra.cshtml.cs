using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class quan_ly_bai_kiem_traModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_bai_kiem_traModel(du_lieu db)
        {
            _db = db;
        }

        public List<bai_kiem_tra> DanhSach { get; set; } = new();
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

            DanhSach = _db.bai_kiem_tra
                .Include(b => b.khoa_hoc)
                .Where(b => khoaHocIds.Contains(b.ma_khoa_hoc))
                .ToList();

            return Page();
        }

        public IActionResult OnPostXoa(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var bai = _db.bai_kiem_tra.Find(id);
            if (bai != null)
            {
                var cauHois = _db.cau_hoi.Where(c => c.ma_bai_kiem_tra == id).ToList();
                var cauHoiIds = cauHois.Select(c => c.ma_cau_hoi).ToList();
                var dapAns = _db.dap_an.Where(d => cauHoiIds.Contains(d.ma_cau_hoi)).ToList();
                var ketQuas = _db.ket_qua_kiem_tra.Where(k => k.ma_bai_kiem_tra == id).ToList();

                _db.dap_an.RemoveRange(dapAns);
                _db.cau_hoi.RemoveRange(cauHois);
                _db.ket_qua_kiem_tra.RemoveRange(ketQuas);
                _db.bai_kiem_tra.Remove(bai);
                _db.SaveChanges();

                ThongBao = "Da xoa bai kiem tra!";
            }

            return RedirectToPage();
        }
    }
}