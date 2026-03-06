using Microsoft.AspNetCore.Mvc;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class sua_bai_kiem_traModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public sua_bai_kiem_traModel(du_lieu db)
        {
            _db = db;
        }

        public bai_kiem_tra? BaiKiemTra { get; set; }
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenBaiKiemTra(id))
            {
                ThongBao = "Khong co quyen sua bai kiem tra!";
                return Page();
            }

            BaiKiemTra = _db.bai_kiem_tra.Find(id);
            return Page();
        }

        public IActionResult OnPost(int id, string tieu_de, int thoi_luong_phut)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (!KiemTraQuyenBaiKiemTra(id))
            {
                ThongBao = "Khong co quyen sua bai kiem tra!";
                return Page();
            }

            var bai = _db.bai_kiem_tra.Find(id);
            if (bai == null)
            {
                ThongBao = "Bai kiem tra khong ton tai!";
                return Page();
            }

            bai.tieu_de = tieu_de.Trim();
            bai.thoi_luong_phut = thoi_luong_phut;

            _db.SaveChanges();
            return RedirectToPage("/teacher/quan_ly_bai_kiem_tra");
        }

        private bool KiemTraQuyenBaiKiemTra(int baiId)
        {
            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var khoaHocIds = _db.lop_hoc
                .Where(l => l.ma_giao_vien == maGV)
                .Select(l => l.ma_khoa_hoc)
                .Distinct()
                .ToList();

            var bai = _db.bai_kiem_tra.Find(baiId);
            return bai != null && khoaHocIds.Contains(bai.ma_khoa_hoc);
        }
    }
}