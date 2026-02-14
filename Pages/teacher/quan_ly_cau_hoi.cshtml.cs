using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class quan_ly_cau_hoiModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public quan_ly_cau_hoiModel(du_lieu db)
        {
            _db = db;
        }

        public int BaiId { get; set; }
        public bai_kiem_tra? BaiKiemTra { get; set; }
        public List<cau_hoi> CauHois { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int baiId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadData(baiId);
            return Page();
        }

        public IActionResult OnPostThem(int baiId, string noi_dung, string dap1, string dap2, string dap3, string dap4, int dap_dung)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (dap_dung < 1 || dap_dung > 4)
            {
                ThongBao = "Dap an dung khong hop le!";
                LoadData(baiId);
                return Page();
            }

            var cauHoi = new cau_hoi
            {
                ma_bai_kiem_tra = baiId,
                noi_dung = noi_dung.Trim()
            };

            _db.cau_hoi.Add(cauHoi);
            _db.SaveChanges();

            var dapAns = new List<dap_an>
            {
                new dap_an { ma_cau_hoi = cauHoi.ma_cau_hoi, noi_dung = dap1, la_dap_an_dung = dap_dung == 1 },
                new dap_an { ma_cau_hoi = cauHoi.ma_cau_hoi, noi_dung = dap2, la_dap_an_dung = dap_dung == 2 },
                new dap_an { ma_cau_hoi = cauHoi.ma_cau_hoi, noi_dung = dap3, la_dap_an_dung = dap_dung == 3 },
                new dap_an { ma_cau_hoi = cauHoi.ma_cau_hoi, noi_dung = dap4, la_dap_an_dung = dap_dung == 4 }
            };

            _db.dap_an.AddRange(dapAns);
            _db.SaveChanges();

            LoadData(baiId);
            return Page();
        }

        public IActionResult OnPostXoa(int baiId, int cauHoiId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var cauHoi = _db.cau_hoi.Find(cauHoiId);
            if (cauHoi != null)
            {
                var dapAns = _db.dap_an.Where(d => d.ma_cau_hoi == cauHoiId).ToList();
                _db.dap_an.RemoveRange(dapAns);
                _db.cau_hoi.Remove(cauHoi);
                _db.SaveChanges();
            }

            LoadData(baiId);
            return Page();
        }

        private void LoadData(int baiId)
        {
            BaiId = baiId;
            BaiKiemTra = _db.bai_kiem_tra.Find(baiId);
            CauHois = _db.cau_hoi
                .Include(c => c.dap_ans)
                .Where(c => c.ma_bai_kiem_tra == baiId)
                .ToList();
        }
    }
}