using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class lam_bai_kiem_traModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public lam_bai_kiem_traModel(du_lieu db)
        {
            _db = db;
        }

        public bai_kiem_tra? BaiKiemTra { get; set; }
        public List<cau_hoi> CauHois { get; set; } = new();
        public bool DaNop { get; set; }
        public decimal DiemSo { get; set; }
        public List<ChiTietKetQua> ChiTiet { get; set; } = new();
        public string ThongBao { get; set; } = "";

        // === FIX #4: Kiem tra quyen truy cap bai kiem tra ===
        private bool KiemTraQuyenBaiKiemTra(int baiKiemTraId)
        {
            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var bai = _db.bai_kiem_tra.Find(baiKiemTraId);
            if (bai == null) return false;

            // Lay danh sach khoa_hoc ma hoc vien da thanh toan
            var khoaHocIds = _db.dang_ky
                .Where(d => d.ma_hoc_vien == maHocVien && d.trang_thai == "DaThanhToan")
                .Include(d => d.lop_hoc)
                .Select(d => d.lop_hoc!.ma_khoa_hoc)
                .Distinct()
                .ToList();

            return khoaHocIds.Contains(bai.ma_khoa_hoc);
        }

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenBaiKiemTra(id))
            {
                ThongBao = "Ban khong co quyen truy cap bai kiem tra nay!";
                return Page();
            }

            LoadData(id);
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenBaiKiemTra(id))
            {
                ThongBao = "Ban khong co quyen truy cap bai kiem tra nay!";
                return Page();
            }

            LoadData(id);

            if (BaiKiemTra == null)
            {
                return Page();
            }

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            bool daCoKetQua = _db.ket_qua_kiem_tra
                .Any(k => k.ma_bai_kiem_tra == id && k.ma_hoc_vien == maHocVien);

            if (daCoKetQua)
            {
                DaNop = true;
                return Page();
            }

            int tong = CauHois.Count;
            int dung = 0;

            foreach (var q in CauHois)
            {
                var selected = Request.Form[$"answer_{q.ma_cau_hoi}"].ToString();
                var dapDung = q.dap_ans.FirstOrDefault(d => d.la_dap_an_dung);

                if (dapDung != null && selected == dapDung.ma_dap_an.ToString())
                {
                    dung++;
                }

                ChiTiet.Add(new ChiTietKetQua
                {
                    NoiDung = q.noi_dung,
                    DapAnChon = q.dap_ans.FirstOrDefault(d => d.ma_dap_an.ToString() == selected)?.noi_dung ?? "Khong chon",
                    DapAnDung = dapDung?.noi_dung ?? ""
                });
            }

            DiemSo = tong == 0 ? 0 : Math.Round((decimal)dung * 10 / tong, 2);
            DaNop = true;

            _db.ket_qua_kiem_tra.Add(new ket_qua_kiem_tra
            {
                ma_bai_kiem_tra = id,
                ma_hoc_vien = maHocVien,
                diem_so = DiemSo,
                ngay_lam_bai = DateTime.Now
            });

            _db.SaveChanges();

            return Page();
        }

        private void LoadData(int id)
        {
            BaiKiemTra = _db.bai_kiem_tra.Find(id);
            CauHois = _db.cau_hoi
                .Include(c => c.dap_ans)
                .Where(c => c.ma_bai_kiem_tra == id)
                .ToList();
        }

        public class ChiTietKetQua
        {
            public string NoiDung { get; set; } = "";
            public string DapAnChon { get; set; } = "";
            public string DapAnDung { get; set; } = "";
        }
    }
}