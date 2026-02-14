using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.student
{
    public class chi_tiet_bai_hocModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public chi_tiet_bai_hocModel(du_lieu db)
        {
            _db = db;
        }

        public bai_hoc? BaiHoc { get; set; }
        public List<tai_lieu> DsTaiLieu { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            BaiHoc = _db.bai_hoc.Include(b => b.khoa_hoc).FirstOrDefault(b => b.ma_bai_hoc == id);
            DsTaiLieu = _db.tai_lieu.Where(t => t.ma_bai_hoc == id).ToList();

            return Page();
        }
    }
}