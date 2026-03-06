using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.admin
{
    public class thong_keModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public thong_keModel(du_lieu db)
        {
            _db = db;
        }

        public int SoHocVien { get; set; }
        public int SoGiaoVien { get; set; }
        public int SoKhoaHoc { get; set; }
        public int SoLopMo { get; set; }
        public decimal DoanhThu { get; set; }
        public List<TopKhoaHocView> TopKhoaHoc { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            SoHocVien = _db.nguoi_dung_vai_tro.Count(v => v.ma_vai_tro == "user");
            SoGiaoVien = _db.nguoi_dung_vai_tro.Count(v => v.ma_vai_tro == "teacher");
            SoKhoaHoc = _db.khoa_hoc.Count();
            SoLopMo = _db.lop_hoc.Count(l => l.trang_thai == "open");
            DoanhThu = _db.hoa_don.Where(h => h.trang_thai == "DaThanhToan").Sum(h => h.so_tien);

            TopKhoaHoc = _db.dang_ky
                .GroupBy(d => d.lop_hoc!.ma_khoa_hoc)
                .Select(g => new TopKhoaHocView
                {
                    MaKhoaHoc = g.Key,
                    SoDangKy = g.Count()
                })
                .OrderByDescending(t => t.SoDangKy)
                .Take(5)
                .Select(t => new TopKhoaHocView
                {
                    MaKhoaHoc = t.MaKhoaHoc,
                    TenKhoaHoc = _db.khoa_hoc.Where(k => k.ma_khoa_hoc == t.MaKhoaHoc).Select(k => k.ten_khoa_hoc).FirstOrDefault() ?? "",
                    SoDangKy = t.SoDangKy
                })
                .ToList();

            return Page();
        }

        public class TopKhoaHocView
        {
            public int MaKhoaHoc { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public int SoDangKy { get; set; }
        }
    }
}