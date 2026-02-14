using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class tao_bai_hocModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public tao_bai_hocModel(du_lieu db)
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

        public IActionResult OnPost(int ma_khoa_hoc, string tieu_de, string noi_dung)
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

            var baiHoc = new bai_hoc
            {
                ma_khoa_hoc = ma_khoa_hoc,
                tieu_de = tieu_de.Trim(),
                noi_dung = noi_dung?.Trim() ?? "",
                nguoi_tao = maGV,
                ngay_tao = DateTime.Now
            };

            _db.bai_hoc.Add(baiHoc);
            _db.SaveChanges();

            return RedirectToPage("/teacher/quan_ly_bai_hoc");
        }
    }
}