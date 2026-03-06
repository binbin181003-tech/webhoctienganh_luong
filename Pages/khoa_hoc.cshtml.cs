using Microsoft.AspNetCore.Mvc.RazorPages;
using webhoctienganh.Data;

namespace webhoctienganh.Pages
{
    public class khoa_hocModel : PageModel
    {
        private readonly du_lieu _db;

        public khoa_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<KhoaHocView> DanhSach { get; set; } = new();

        public void OnGet()
        {
            var danhGiaMap = _db.danh_gia
                .GroupBy(d => d.ma_khoa_hoc)
                .Select(g => new
                {
                    MaKhoaHoc = g.Key,
                    DiemTB = g.Average(x => x.so_sao),
                    SoLuong = g.Count()
                })
                .ToList();

            DanhSach = _db.khoa_hoc
                .Select(k => new KhoaHocView
                {
                    MaKhoaHoc = k.ma_khoa_hoc,
                    TenKhoaHoc = k.ten_khoa_hoc,
                    TrinhDo = k.trinh_do,
                    HocPhi = k.hoc_phi
                })
                .ToList();

            foreach (var kh in DanhSach)
            {
                var dg = danhGiaMap.FirstOrDefault(x => x.MaKhoaHoc == kh.MaKhoaHoc);
                kh.DiemTrungBinh = dg != null ? Math.Round(dg.DiemTB, 1) : 0;
                kh.SoLuongDanhGia = dg?.SoLuong ?? 0;
            }
        }

        public class KhoaHocView
        {
            public int MaKhoaHoc { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public string TrinhDo { get; set; } = "";
            public decimal HocPhi { get; set; }
            public double DiemTrungBinh { get; set; }
            public int SoLuongDanhGia { get; set; }
        }
    }
}