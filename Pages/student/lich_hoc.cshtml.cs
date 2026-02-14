using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.student
{
    public class lich_hocModel : StudentBasePageModel
    {
        private readonly du_lieu _db;

        public lich_hocModel(du_lieu db)
        {
            _db = db;
        }

        public int WeekOffset { get; set; }
        public DateTime TuanBatDau { get; set; }
        public DateTime TuanKetThuc { get; set; }
        public List<NgayHocView> Tuan { get; set; } = new();

        public IActionResult OnGet(int week = 0)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            WeekOffset = week;

            var today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            TuanBatDau = today.AddDays(-diff).AddDays(7 * week);
            TuanKetThuc = TuanBatDau.AddDays(6);

            var maHocVien = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            var lopHocIds = _db.dang_ky
                .Where(d => d.ma_hoc_vien == maHocVien && d.trang_thai == "DaThanhToan")
                .Select(d => d.ma_lop_hoc)
                .Distinct()
                .ToList();

            var lichHocs = _db.lich_hoc
                .Include(l => l.lop_hoc)
                .ThenInclude(lh => lh!.khoa_hoc)
                .Where(l => lopHocIds.Contains(l.ma_lop_hoc))
                .ToList();

            var dayMap = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Mon" },
                { DayOfWeek.Tuesday, "Tue" },
                { DayOfWeek.Wednesday, "Wed" },
                { DayOfWeek.Thursday, "Thu" },
                { DayOfWeek.Friday, "Fri" },
                { DayOfWeek.Saturday, "Sat" },
                { DayOfWeek.Sunday, "Sun" }
            };

            var thuLabelMap = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Thu 2" },
                { DayOfWeek.Tuesday, "Thu 3" },
                { DayOfWeek.Wednesday, "Thu 4" },
                { DayOfWeek.Thursday, "Thu 5" },
                { DayOfWeek.Friday, "Thu 6" },
                { DayOfWeek.Saturday, "Thu 7" },
                { DayOfWeek.Sunday, "Chu Nhat" }
            };

            for (int i = 0; i < 7; i++)
            {
                var ngay = TuanBatDau.AddDays(i);
                var key = dayMap[ngay.DayOfWeek];

                var danhSach = lichHocs
                    .Where(l => l.thu_trong_tuan == key)
                    .Select(l => new LichHocItem
                    {
                        TenKhoaHoc = l.lop_hoc?.khoa_hoc?.ten_khoa_hoc ?? "",
                        GioBatDau = l.gio_bat_dau,
                        GioKetThuc = l.gio_ket_thuc,
                        LinkHoc = l.phong_hoc
                    })
                    .ToList();

                Tuan.Add(new NgayHocView
                {
                    Ngay = ngay,
                    ThuLabel = thuLabelMap[ngay.DayOfWeek],
                    LichHoc = danhSach
                });
            }

            return Page();
        }

        public class NgayHocView
        {
            public DateTime Ngay { get; set; }
            public string ThuLabel { get; set; } = "";
            public List<LichHocItem> LichHoc { get; set; } = new();
        }

        public class LichHocItem
        {
            public string TenKhoaHoc { get; set; } = "";
            public string GioBatDau { get; set; } = "";
            public string GioKetThuc { get; set; } = "";
            public string LinkHoc { get; set; } = "";
        }
    }
}