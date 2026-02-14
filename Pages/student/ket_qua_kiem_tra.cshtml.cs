using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.student
{
    public class ket_qua_kiem_traModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public ket_qua_kiem_traModel(du_lieu db)
        {
            _db = db;
        }

        public List<KetQuaView> DanhSach { get; set; } = new();

        public IActionResult OnGet()
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            DanhSach = _db.ket_qua_kiem_tra
                .Include(k => k.bai_kiem_tra)
                .ThenInclude(b => b!.khoa_hoc)
                .Where(k => k.ma_hoc_vien == maHocVien)
                .Select(k => new KetQuaView
                {
                    TieuDe = k.bai_kiem_tra != null ? k.bai_kiem_tra.tieu_de : "",
                    TenKhoaHoc = k.bai_kiem_tra != null && k.bai_kiem_tra.khoa_hoc != null ? k.bai_kiem_tra.khoa_hoc.ten_khoa_hoc : "",
                    DiemSo = k.diem_so,
                    NgayLam = k.ngay_lam_bai
                })
                .ToList();

            return Page();
        }

        public class KetQuaView
        {
            public string TieuDe { get; set; } = "";
            public string TenKhoaHoc { get; set; } = "";
            public decimal DiemSo { get; set; }
            public DateTime NgayLam { get; set; }
        }
    }
}