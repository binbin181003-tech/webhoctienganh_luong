using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class sua_bai_hocModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public sua_bai_hocModel(du_lieu db)
        {
            _db = db;
        }

        public bai_hoc? BaiHoc { get; set; }
        public List<tai_lieu> DsTaiLieu { get; set; } = new();
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            LoadData(id);
            return Page();
        }

        public IActionResult OnPost(int id, string tieu_de, string noi_dung)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var bai = _db.bai_hoc.Find(id);
            if (bai == null)
            {
                ThongBao = "Bai hoc khong ton tai!";
                return Page();
            }

            bai.tieu_de = tieu_de.Trim();
            bai.noi_dung = noi_dung?.Trim() ?? "";
            _db.SaveChanges();

            LoadData(id);
            return Page();
        }

        public IActionResult OnPostThemTaiLieu(int id, string duong_dan_file, string loai_tai_lieu)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            if (string.IsNullOrWhiteSpace(duong_dan_file))
            {
                ThongBao = "Duong dan khong duoc de trong!";
                LoadData(id);
                return Page();
            }

            _db.tai_lieu.Add(new tai_lieu
            {
                ma_bai_hoc = id,
                duong_dan_file = duong_dan_file.Trim(),
                loai_tai_lieu = loai_tai_lieu.Trim()
            });

            _db.SaveChanges();
            LoadData(id);
            return Page();
        }

        public IActionResult OnPostXoaTaiLieu(int id, int taiLieuId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var tl = _db.tai_lieu.Find(taiLieuId);
            if (tl != null)
            {
                _db.tai_lieu.Remove(tl);
                _db.SaveChanges();
            }

            LoadData(id);
            return Page();
        }

        private void LoadData(int id)
        {
            BaiHoc = _db.bai_hoc.Include(b => b.khoa_hoc).FirstOrDefault(b => b.ma_bai_hoc == id);
            DsTaiLieu = _db.tai_lieu.Where(t => t.ma_bai_hoc == id).ToList();
        }
    }
}