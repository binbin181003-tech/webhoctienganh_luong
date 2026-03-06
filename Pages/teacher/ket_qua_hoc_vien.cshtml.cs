using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

namespace webhoctienganh.Pages.teacher
{
    public class ket_qua_hoc_vienModel : TeacherBasePageModel
    {
        private readonly du_lieu _db;

        public ket_qua_hoc_vienModel(du_lieu db)
        {
            _db = db;
        }

        public List<LopView> DsLop { get; set; } = new();
        public List<HocVienView> DsHocVien { get; set; } = new();
        public List<KetQuaView> DsKetQua { get; set; } = new();
        public int? ClassId { get; set; }
        public string? StudentId { get; set; }
        public string StudentInfo { get; set; } = "";
        public string ThongBao { get; set; } = "";

        public IActionResult OnGet(int? classId, string? studentId)
        {
            var check = KiemTraQuyen();
            if (check != null) return check;

            var maGV = HttpContext.Session.GetString("ma_nguoi_dung") ?? "";

            // 1) Danh sach lop giao vien day
            DsLop = _db.lop_hoc
                .Include(l => l.khoa_hoc)
                .Where(l => l.ma_giao_vien == maGV)
                .Select(l => new LopView
                {
                    MaLop = l.ma_lop_hoc,
                    TenKhoaHoc = l.khoa_hoc != null ? l.khoa_hoc.ten_khoa_hoc : "",
                    TrangThai = l.trang_thai
                })
                .ToList();

            ClassId = classId;

            // 2) Neu chon lop -> load hoc vien (da thanh toan)
            if (ClassId.HasValue)
            {
                DsHocVien = _db.dang_ky
                    .Include(d => d.hoc_vien)
                    .Where(d => d.ma_lop_hoc == ClassId.Value && d.trang_thai == "DaThanhToan")
                    .Select(d => new HocVienView
                    {
                        MaHocVien = d.ma_hoc_vien,
                        HoTen = d.hoc_vien != null ? d.hoc_vien.ho_ten : "",
                        Email = d.hoc_vien != null ? d.hoc_vien.email : "",
                        SoDienThoai = d.hoc_vien != null ? d.hoc_vien.so_dien_thoai : "",
                        TrangThai = d.trang_thai
                    })
                    .ToList();
            }

            StudentId = studentId;

            // 3) Neu chon hoc vien -> load ket qua bai kiem tra
            if (!string.IsNullOrEmpty(StudentId))
            {
                var student = _db.nguoi_dung.Find(StudentId);
                StudentInfo = student != null ? $"{student.ho_ten} ({student.email})" : StudentId;

                DsKetQua = _db.ket_qua_kiem_tra
                    .Include(k => k.bai_kiem_tra)
                    .ThenInclude(b => b!.khoa_hoc)
                    .Where(k => k.ma_hoc_vien == StudentId)
                    .Select(k => new KetQuaView
                    {
                        TieuDe = k.bai_kiem_tra != null ? k.bai_kiem_tra.tieu_de : "",
                        TenKhoaHoc = k.bai_kiem_tra != null && k.bai_kiem_tra.khoa_hoc != null
                            ? k.bai_kiem_tra.khoa_hoc.ten_khoa_hoc
                            : "",
                        DiemSo = k.diem_so,
                        NgayLam = k.ngay_lam_bai
                    })
                    .OrderByDescending(k => k.NgayLam)
                    .ToList();
            }

            return Page();
        }

        public class LopView
        {
            public int MaLop { get; set; }
            public string TenKhoaHoc { get; set; } = "";
            public string TrangThai { get; set; } = "";
        }

        public class HocVienView
        {
            public string MaHocVien { get; set; } = "";
            public string HoTen { get; set; } = "";
            public string Email { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
            public string TrangThai { get; set; } = "";
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