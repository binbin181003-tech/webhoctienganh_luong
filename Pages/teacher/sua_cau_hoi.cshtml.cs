using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class sua_cau_hoiModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public sua_cau_hoiModel(du_lieu db)
        {
            _db = db;
        }

        public cau_hoi? CauHoi { get; set; }
        public int BaiId { get; set; }
        public List<DapAnView> DapAns { get; set; } = new();
        public int DapDungIndex { get; set; } = 1;
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int cauHoiId, int baiId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenCauHoi(cauHoiId))
            {
                ThongBao = "Khong co quyen sua cau hoi!";
                return Page();
            }

            BaiId = baiId;
            CauHoi = _db.cau_hoi.Find(cauHoiId);
            if (CauHoi == null)
            {
                ThongBao = "Cau hoi khong ton tai!";
                return Page();
            }

            var dapAns = _db.dap_an
                .Where(d => d.ma_cau_hoi == cauHoiId)
                .OrderBy(d => d.ma_dap_an)
                .ToList();

            DapAns = dapAns.Select(d => new DapAnView
            {
                MaDapAn = d.ma_dap_an,
                NoiDung = d.noi_dung,
                LaDung = d.la_dap_an_dung
            }).ToList();

            var indexDung = DapAns.FindIndex(d => d.LaDung);
            DapDungIndex = indexDung >= 0 ? indexDung + 1 : 1;

            return Page();
        }

        public IActionResult OnPost(int cauHoiId, int baiId, string noi_dung, int dap_dung)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenCauHoi(cauHoiId))
            {
                ThongBao = "Khong co quyen sua cau hoi!";
                return Page();
            }

            var cauHoi = _db.cau_hoi.Find(cauHoiId);
            if (cauHoi == null)
            {
                ThongBao = "Cau hoi khong ton tai!";
                return Page();
            }

            var dapAns = _db.dap_an
                .Where(d => d.ma_cau_hoi == cauHoiId)
                .OrderBy(d => d.ma_dap_an)
                .ToList();

            if (dap_dung < 1 || dap_dung > dapAns.Count)
            {
                ThongBao = "Dap an dung khong hop le!";
                return Page();
            }

            cauHoi.noi_dung = noi_dung.Trim();

            for (int i = 0; i < dapAns.Count; i++)
            {
                var idKey = $"dap_an_id_{i}";
                var contentKey = $"dap_an_noi_dung_{i}";
                var idVal = Request.Form[idKey].ToString();
                var contentVal = Request.Form[contentKey].ToString();

                if (int.TryParse(idVal, out int dapAnId))
                {
                    var dap = dapAns.FirstOrDefault(d => d.ma_dap_an == dapAnId);
                    if (dap != null)
                    {
                        dap.noi_dung = contentVal.Trim();
                        dap.la_dap_an_dung = (i + 1) == dap_dung;
                    }
                }
            }

            _db.SaveChanges();

            return RedirectToPage("/teacher/quan_ly_cau_hoi", new { baiId });
        }

        private bool KiemTraQuyenCauHoi(int cauHoiId)
        {
            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var cauHoi = _db.cau_hoi
                .Include(c => c.bai_kiem_tra)
                .FirstOrDefault(c => c.ma_cau_hoi == cauHoiId);

            if (cauHoi?.bai_kiem_tra == null) return false;

            var khoaHocIds = _db.lop_hoc
                .Where(l => l.ma_giao_vien == maGV)
                .Select(l => l.ma_khoa_hoc)
                .Distinct()
                .ToList();

            return khoaHocIds.Contains(cauHoi.bai_kiem_tra.ma_khoa_hoc);
        }

        public class DapAnView
        {
            public int MaDapAn { get; set; }
            public string NoiDung { get; set; } = "";
            public bool LaDung { get; set; }
        }
    }
}