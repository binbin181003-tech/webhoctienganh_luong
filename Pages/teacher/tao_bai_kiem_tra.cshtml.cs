using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class tao_bai_kiem_traModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public tao_bai_kiem_traModel(du_lieu db)
        {
            _db = db;
        }

        public List<khoa_hoc> DsKhoaHoc { get; set; } = new();
        public string ThongBao { get; set; } = "";

        private void LoadKhoaHoc()
        {
            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var khoaHocIds = _db.lop_hoc
                .Where(l => l.ma_giao_vien == maGV)
                .Select(l => l.ma_khoa_hoc)
                .Distinct()
                .ToList();

            DsKhoaHoc = _db.khoa_hoc.Where(k => khoaHocIds.Contains(k.ma_khoa_hoc)).ToList();
        }

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadKhoaHoc();
            return Page();
        }

        public IActionResult OnPost(int ma_khoa_hoc, string tieu_de, int thoi_luong_phut)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadKhoaHoc();

            if (string.IsNullOrWhiteSpace(tieu_de))
            {
                ThongBao = "Tieu de khong duoc de trong!";
                return Page();
            }

            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            _db.bai_kiem_tra.Add(new bai_kiem_tra
            {
                ma_khoa_hoc = ma_khoa_hoc,
                tieu_de = tieu_de.Trim(),
                thoi_luong_phut = thoi_luong_phut,
                nguoi_tao = maGV
            });

            _db.SaveChanges();
            return RedirectToPage("/teacher/quan_ly_bai_kiem_tra");
        }
    }
}