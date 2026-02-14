using Microsoft.EntityFrameworkCore;
using webhoctienganh.Models;

namespace webhoctienganh.Data
{
    public class du_lieu : DbContext
    {
        public du_lieu(DbContextOptions<du_lieu> options) : base(options) { }

        // DbSets
        public DbSet<nguoi_dung> nguoi_dung { get; set; }
        public DbSet<vai_tro> vai_tro { get; set; }
        public DbSet<nguoi_dung_vai_tro> nguoi_dung_vai_tro { get; set; }
        public DbSet<khoa_hoc> khoa_hoc { get; set; }
        public DbSet<lop_hoc> lop_hoc { get; set; }
        public DbSet<dang_ky_model> dang_ky { get; set; }
        public DbSet<lich_hoc> lich_hoc { get; set; }
        public DbSet<bai_hoc> bai_hoc { get; set; }
        public DbSet<tai_lieu> tai_lieu { get; set; }
        public DbSet<bai_kiem_tra> bai_kiem_tra { get; set; }
        public DbSet<cau_hoi> cau_hoi { get; set; }
        public DbSet<dap_an> dap_an { get; set; }
        public DbSet<ket_qua_kiem_tra> ket_qua_kiem_tra { get; set; }
        public DbSet<thanh_toan> thanh_toan { get; set; }
        public DbSet<hoa_don> hoa_don { get; set; }
        public DbSet<danh_gia> danh_gia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========== NGUOI_DUNG_VAI_TRO (composite key) ==========
            modelBuilder.Entity<nguoi_dung_vai_tro>()
                .HasKey(nv => new { nv.ma_nguoi_dung, nv.ma_vai_tro });

            modelBuilder.Entity<nguoi_dung_vai_tro>()
                .HasOne(nv => nv.nguoi_dung)
                .WithMany(n => n.vai_tros)
                .HasForeignKey(nv => nv.ma_nguoi_dung);

            modelBuilder.Entity<nguoi_dung_vai_tro>()
                .HasOne(nv => nv.vai_tro)
                .WithMany(v => v.nguoi_dungs)
                .HasForeignKey(nv => nv.ma_vai_tro);

            // ========== KHOA_HOC -> NGUOI_TAO ==========
            modelBuilder.Entity<khoa_hoc>()
                .HasOne(k => k.nguoi_tao_nav)
                .WithMany()
                .HasForeignKey(k => k.nguoi_tao)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== LOP_HOC ==========
            modelBuilder.Entity<lop_hoc>()
                .HasOne(l => l.khoa_hoc)
                .WithMany(k => k.lop_hocs)
                .HasForeignKey(l => l.ma_khoa_hoc);

            modelBuilder.Entity<lop_hoc>()
                .HasOne(l => l.giao_vien)
                .WithMany(n => n.lop_giang_days)
                .HasForeignKey(l => l.ma_giao_vien)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== DANG_KY ==========
            modelBuilder.Entity<dang_ky_model>()
                .HasOne(d => d.hoc_vien)
                .WithMany(n => n.dang_kys)
                .HasForeignKey(d => d.ma_hoc_vien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<dang_ky_model>()
                .HasOne(d => d.lop_hoc)
                .WithMany(l => l.dang_kys)
                .HasForeignKey(d => d.ma_lop_hoc);

            // ========== LICH_HOC ==========
            modelBuilder.Entity<lich_hoc>()
                .HasOne(l => l.lop_hoc)
                .WithMany(lh => lh.lich_hocs)
                .HasForeignKey(l => l.ma_lop_hoc);

            // ========== BAI_HOC ==========
            modelBuilder.Entity<bai_hoc>()
                .HasOne(b => b.khoa_hoc)
                .WithMany(k => k.bai_hocs)
                .HasForeignKey(b => b.ma_khoa_hoc);

            modelBuilder.Entity<bai_hoc>()
                .HasOne(b => b.nguoi_tao_nav)
                .WithMany()
                .HasForeignKey(b => b.nguoi_tao)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== TAI_LIEU ==========
            modelBuilder.Entity<tai_lieu>()
                .HasOne(t => t.bai_hoc)
                .WithMany(b => b.tai_lieus)
                .HasForeignKey(t => t.ma_bai_hoc);

            // ========== BAI_KIEM_TRA ==========
            modelBuilder.Entity<bai_kiem_tra>()
                .HasOne(b => b.khoa_hoc)
                .WithMany(k => k.bai_kiem_tras)
                .HasForeignKey(b => b.ma_khoa_hoc);

            modelBuilder.Entity<bai_kiem_tra>()
                .HasOne(b => b.nguoi_tao_nav)
                .WithMany()
                .HasForeignKey(b => b.nguoi_tao)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== CAU_HOI ==========
            modelBuilder.Entity<cau_hoi>()
                .HasOne(c => c.bai_kiem_tra)
                .WithMany(b => b.cau_hois)
                .HasForeignKey(c => c.ma_bai_kiem_tra);

            // ========== DAP_AN ==========
            modelBuilder.Entity<dap_an>()
                .HasOne(d => d.cau_hoi)
                .WithMany(c => c.dap_ans)
                .HasForeignKey(d => d.ma_cau_hoi);

            // ========== KET_QUA_KIEM_TRA ==========
            modelBuilder.Entity<ket_qua_kiem_tra>()
                .HasOne(k => k.bai_kiem_tra)
                .WithMany(b => b.ket_quas)
                .HasForeignKey(k => k.ma_bai_kiem_tra);

            modelBuilder.Entity<ket_qua_kiem_tra>()
                .HasOne(k => k.hoc_vien)
                .WithMany(n => n.ket_quas)
                .HasForeignKey(k => k.ma_hoc_vien)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== HOA_DON (1-1 with DANG_KY) ==========
            modelBuilder.Entity<hoa_don>()
                .HasOne(h => h.dang_ky)
                .WithOne(d => d.hoa_don)
                .HasForeignKey<hoa_don>(h => h.ma_dang_ky);

            // ========== THANH_TOAN (1-1 with HOA_DON) ==========
            modelBuilder.Entity<thanh_toan>()
                .HasOne(t => t.hoa_don)
                .WithOne(h => h.thanh_toan)
                .HasForeignKey<thanh_toan>(t => t.ma_hoa_don);

            // ========== DANH_GIA ==========
            modelBuilder.Entity<danh_gia>()
                .HasOne(d => d.hoc_vien)
                .WithMany(n => n.danh_gias)
                .HasForeignKey(d => d.ma_hoc_vien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<danh_gia>()
                .HasOne(d => d.khoa_hoc)
                .WithMany(k => k.danh_gias)
                .HasForeignKey(d => d.ma_khoa_hoc);

            // ========== SEED VAI_TRO ==========
            modelBuilder.Entity<vai_tro>().HasData(
                new vai_tro { ma_vai_tro = "user", ten_vai_tro = "Hoc vien" },
                new vai_tro { ma_vai_tro = "teacher", ten_vai_tro = "Giao vien" },
                new vai_tro { ma_vai_tro = "admin", ten_vai_tro = "Quan tri vien" }
            );

            // ========== UNIQUE INDEX ==========
            modelBuilder.Entity<nguoi_dung>()
                .HasIndex(n => n.email)
                .IsUnique();
        }
    }
}