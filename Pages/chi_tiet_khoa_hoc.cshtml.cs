using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages
{
    public class chi_tiet_khoa_hocModel : PageModel
    {
        private readonly du_lieu _db;

        public chi_tiet_khoa_hocModel(du_lieu db)
        {
            _db = db;
        }

        public khoa_hoc? KhoaHoc { get; set; }
        public List<DanhGiaView> DanhGia { get; set; } = new();
        public double DiemTrungBinh { get; set; }
        public int SoLuongDanhGia { get; set; }
        public List<LopMoView> LopMo { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int id)
        {
            KhoaHoc = _db.khoa_hoc.Find(id);
            if (KhoaHoc == null)
            {
                ThongBao = "Khoa hoc khong ton tai!";
                return Page();
            }

            DanhGia = _db.danh_gia
                .Include(d => d.hoc_vien)
                .Where(d => d.ma_khoa_hoc == id)
                .Select(d => new DanhGiaView
                {
                    HoTen = d.hoc_vien != null ? d.hoc_vien.ho_ten : "",
                    SoSao = d.so_sao,
                    NoiDung = d.noi_dung
                })
                .ToList();

            if (DanhGia.Any())
            {
                DiemTrungBinh = Math.Round(DanhGia.Average(d => d.SoSao), 1);
                SoLuongDanhGia = DanhGia.Count;
            }

            LopMo = _db.lop_hoc
                .Include(l => l.giao_vien)
                .Where(l => l.ma_khoa_hoc == id && l.trang_thai == "open")
                .Select(l => new LopMoView
                {
                    MaLop = l.ma_lop_hoc,
                    TenGiaoVien = l.giao_vien != null ? l.giao_vien.ho_ten : "",
                    NgayBatDau = l.ngay_bat_dau,
                    NgayKetThuc = l.ngay_ket_thuc,
                    SoLuongToiDa = l.so_luong_toi_da,
                    TrangThai = l.trang_thai
                })
                .ToList();

            return Page();
        }

        public class DanhGiaView
        {
            public string HoTen { get; set; } = "";
            public int SoSao { get; set; }
            public string NoiDung { get; set; } = "";
        }

        public class LopMoView
        {
            public int MaLop { get; set; }
            public string TenGiaoVien { get; set; } = "";
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public int SoLuongToiDa { get; set; }
            public string TrangThai { get; set; } = "";
        }
    }
}