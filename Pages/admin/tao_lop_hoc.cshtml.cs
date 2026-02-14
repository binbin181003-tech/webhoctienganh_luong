using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.admin
{
    public class tao_lop_hocModel : AdminBasePageModel
    {
        private readonly du_lieu _db;

        public tao_lop_hocModel(du_lieu db)
        {
            _db = db;
        }

        public List<khoa_hoc> DsKhoaHoc { get; set; } = new();
        public List<nguoi_dung> DsGiaoVien { get; set; } = new();
        public string ThongBao { get; set; } = "";

        private void LoadDropdowns()
        {
            DsKhoaHoc = _db.khoa_hoc.ToList();

            var teacherIds = _db.nguoi_dung_vai_tro
                .Where(nv => nv.ma_vai_tro == "teacher")
                .Select(nv => nv.ma_nguoi_dung)
                .ToList();

            DsGiaoVien = _db.nguoi_dung
                .Where(n => teacherIds.Contains(n.ma_nguoi_dung))
                .ToList();
        }

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDropdowns();
            return Page();
        }

        public IActionResult OnPost(
            int ma_khoa_hoc,
            string ma_giao_vien,
            DateTime ngay_bat_dau,
            DateTime ngay_ket_thuc,
            int so_luong_toi_da,
            string thu_trong_tuan,
            string gio_bat_dau,
            string gio_ket_thuc,
            string phong_hoc)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadDropdowns();

            var kh = _db.khoa_hoc.Find(ma_khoa_hoc);
            if (kh == null)
            {
                ThongBao = "Khoa hoc khong ton tai!";
                return Page();
            }

            var gvExists = _db.nguoi_dung_vai_tro
                .Any(nv => nv.ma_nguoi_dung == ma_giao_vien && nv.ma_vai_tro == "teacher");
            if (!gvExists)
            {
                ThongBao = "Giao vien khong hop le!";
                return Page();
            }

            if (ngay_ket_thuc <= ngay_bat_dau)
            {
                ThongBao = "Ngay ket thuc phai sau ngay bat dau!";
                return Page();
            }

            if (so_luong_toi_da < 1)
            {
                ThongBao = "So luong toi da phai >= 1!";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(thu_trong_tuan) ||
                string.IsNullOrWhiteSpace(gio_bat_dau) ||
                string.IsNullOrWhiteSpace(gio_ket_thuc) ||
                string.IsNullOrWhiteSpace(phong_hoc))
            {
                ThongBao = "Vui long nhap day du thong tin lich hoc!";
                return Page();
            }

            var lh = new lop_hoc
            {
                ma_khoa_hoc = ma_khoa_hoc,
                ma_giao_vien = ma_giao_vien,
                ngay_bat_dau = ngay_bat_dau,
                ngay_ket_thuc = ngay_ket_thuc,
                so_luong_toi_da = so_luong_toi_da,
                trang_thai = "open"
            };

            _db.lop_hoc.Add(lh);
            _db.SaveChanges();

            var lich = new lich_hoc
            {
                ma_lop_hoc = lh.ma_lop_hoc,
                thu_trong_tuan = thu_trong_tuan,
                gio_bat_dau = gio_bat_dau,
                gio_ket_thuc = gio_ket_thuc,
                phong_hoc = phong_hoc
            };

            _db.lich_hoc.Add(lich);
            _db.SaveChanges();

            return RedirectToPage("/admin/quan_ly_lop_hoc");
        }
    }
}