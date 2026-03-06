using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;

namespace webhoctienganh.Pages.teacher
{
    public class lich_dayModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public lich_dayModel(du_lieu db)
        {
            _db = db;
        }

        public string ThongBao { get; set; } = "";
        public int WeekOffset { get; set; }
        public DateTime TuanBatDau { get; set; }
        public DateTime TuanKetThuc { get; set; }
        public List<NgayDayView> Tuan { get; set; } = new();

        public IActionResult OnGet(int week = 0)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            WeekOffset = week;

            var today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            TuanBatDau = today.AddDays(-diff).AddDays(7 * week);
            TuanKetThuc = TuanBatDau.AddDays(6);

            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            // Lớp giáo viên dạy
            var lopIds = _db.lop_hoc
                .Where(l => l.ma_giao_vien == maGV)
                .Select(l => l.ma_lop_hoc)
                .ToList();

            // Lịch dạy trong tuần & trong phạm vi ngày lớp mở
            var lichHocs = _db.lich_hoc
                .Include(l => l.lop_hoc)
                .ThenInclude(lh => lh!.khoa_hoc)
                .Where(l => lopIds.Contains(l.ma_lop_hoc)
                    && l.lop_hoc != null
                    && l.lop_hoc.ngay_bat_dau <= TuanKetThuc
                    && l.lop_hoc.ngay_ket_thuc >= TuanBatDau)
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
                    .Select(l => new LichDayItem
                    {
                        TenKhoaHoc = l.lop_hoc?.khoa_hoc?.ten_khoa_hoc ?? "",
                        MaLop = l.ma_lop_hoc,
                        GioBatDau = l.gio_bat_dau,
                        GioKetThuc = l.gio_ket_thuc,
                        LinkHoc = l.phong_hoc
                    })
                    .ToList();

                Tuan.Add(new NgayDayView
                {
                    Ngay = ngay,
                    ThuLabel = thuLabelMap[ngay.DayOfWeek],
                    LichHoc = danhSach
                });
            }

            return Page();
        }

        public class NgayDayView
        {
            public DateTime Ngay { get; set; }
            public string ThuLabel { get; set; } = "";
            public List<LichDayItem> LichHoc { get; set; } = new();
        }

        public class LichDayItem
        {
            public string TenKhoaHoc { get; set; } = "";
            public int MaLop { get; set; }
            public string GioBatDau { get; set; } = "";
            public string GioKetThuc { get; set; } = "";
            public string LinkHoc { get; set; } = "";
        }

        // Giữ lại LopView, LichView, HocVienView nếu nơi khác dùng (không bắt buộc xóa)
        public class LopView
        {
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public string TrangThai { get; set; } = "";
        }

        public class LichView
        {
            public int MaLich { get; set; }
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public string Thu { get; set; } = "";
            public string GioBatDau { get; set; } = "";
            public string GioKetThuc { get; set; } = "";
            public string PhongHoc { get; set; } = "";
        }

        public class HocVienView
        {
            public string HoTen { get; set; } = "";
            public string Email { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
        }
    }
}