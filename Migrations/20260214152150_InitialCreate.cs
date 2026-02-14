using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webhoctienganh.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nguoi_dung",
                columns: table => new
                {
                    ma_nguoi_dung = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    mat_khau_hash = table.Column<string>(type: "TEXT", nullable: false),
                    ho_ten = table.Column<string>(type: "TEXT", nullable: false),
                    so_dien_thoai = table.Column<string>(type: "TEXT", nullable: false),
                    anh_dai_dien = table.Column<string>(type: "TEXT", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    trang_thai = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nguoi_dung", x => x.ma_nguoi_dung);
                });

            migrationBuilder.CreateTable(
                name: "vai_tro",
                columns: table => new
                {
                    ma_vai_tro = table.Column<string>(type: "TEXT", nullable: false),
                    ten_vai_tro = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vai_tro", x => x.ma_vai_tro);
                });

            migrationBuilder.CreateTable(
                name: "khoa_hoc",
                columns: table => new
                {
                    ma_khoa_hoc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ten_khoa_hoc = table.Column<string>(type: "TEXT", nullable: false),
                    mo_ta = table.Column<string>(type: "TEXT", nullable: false),
                    hoc_phi = table.Column<decimal>(type: "TEXT", nullable: false),
                    trinh_do = table.Column<string>(type: "TEXT", nullable: false),
                    thoi_luong_tuan = table.Column<int>(type: "INTEGER", nullable: false),
                    nguoi_tao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khoa_hoc", x => x.ma_khoa_hoc);
                    table.ForeignKey(
                        name: "FK_khoa_hoc_nguoi_dung_nguoi_tao",
                        column: x => x.nguoi_tao,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nguoi_dung_vai_tro",
                columns: table => new
                {
                    ma_nguoi_dung = table.Column<string>(type: "TEXT", nullable: false),
                    ma_vai_tro = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nguoi_dung_vai_tro", x => new { x.ma_nguoi_dung, x.ma_vai_tro });
                    table.ForeignKey(
                        name: "FK_nguoi_dung_vai_tro_nguoi_dung_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nguoi_dung_vai_tro_vai_tro_ma_vai_tro",
                        column: x => x.ma_vai_tro,
                        principalTable: "vai_tro",
                        principalColumn: "ma_vai_tro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bai_hoc",
                columns: table => new
                {
                    ma_bai_hoc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_khoa_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    tieu_de = table.Column<string>(type: "TEXT", nullable: false),
                    noi_dung = table.Column<string>(type: "TEXT", nullable: false),
                    nguoi_tao = table.Column<string>(type: "TEXT", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bai_hoc", x => x.ma_bai_hoc);
                    table.ForeignKey(
                        name: "FK_bai_hoc_khoa_hoc_ma_khoa_hoc",
                        column: x => x.ma_khoa_hoc,
                        principalTable: "khoa_hoc",
                        principalColumn: "ma_khoa_hoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bai_hoc_nguoi_dung_nguoi_tao",
                        column: x => x.nguoi_tao,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bai_kiem_tra",
                columns: table => new
                {
                    ma_bai_kiem_tra = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_khoa_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    tieu_de = table.Column<string>(type: "TEXT", nullable: false),
                    thoi_luong_phut = table.Column<int>(type: "INTEGER", nullable: false),
                    nguoi_tao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bai_kiem_tra", x => x.ma_bai_kiem_tra);
                    table.ForeignKey(
                        name: "FK_bai_kiem_tra_khoa_hoc_ma_khoa_hoc",
                        column: x => x.ma_khoa_hoc,
                        principalTable: "khoa_hoc",
                        principalColumn: "ma_khoa_hoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bai_kiem_tra_nguoi_dung_nguoi_tao",
                        column: x => x.nguoi_tao,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "danh_gia",
                columns: table => new
                {
                    ma_danh_gia = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_hoc_vien = table.Column<string>(type: "TEXT", nullable: false),
                    ma_khoa_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    so_sao = table.Column<int>(type: "INTEGER", nullable: false),
                    noi_dung = table.Column<string>(type: "TEXT", nullable: false),
                    ngay_danh_gia = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danh_gia", x => x.ma_danh_gia);
                    table.ForeignKey(
                        name: "FK_danh_gia_khoa_hoc_ma_khoa_hoc",
                        column: x => x.ma_khoa_hoc,
                        principalTable: "khoa_hoc",
                        principalColumn: "ma_khoa_hoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_danh_gia_nguoi_dung_ma_hoc_vien",
                        column: x => x.ma_hoc_vien,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lop_hoc",
                columns: table => new
                {
                    ma_lop_hoc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_khoa_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    ma_giao_vien = table.Column<string>(type: "TEXT", nullable: false),
                    ngay_bat_dau = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ngay_ket_thuc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    so_luong_toi_da = table.Column<int>(type: "INTEGER", nullable: false),
                    trang_thai = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lop_hoc", x => x.ma_lop_hoc);
                    table.ForeignKey(
                        name: "FK_lop_hoc_khoa_hoc_ma_khoa_hoc",
                        column: x => x.ma_khoa_hoc,
                        principalTable: "khoa_hoc",
                        principalColumn: "ma_khoa_hoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lop_hoc_nguoi_dung_ma_giao_vien",
                        column: x => x.ma_giao_vien,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tai_lieu",
                columns: table => new
                {
                    ma_tai_lieu = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_bai_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    duong_dan_file = table.Column<string>(type: "TEXT", nullable: false),
                    loai_tai_lieu = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tai_lieu", x => x.ma_tai_lieu);
                    table.ForeignKey(
                        name: "FK_tai_lieu_bai_hoc_ma_bai_hoc",
                        column: x => x.ma_bai_hoc,
                        principalTable: "bai_hoc",
                        principalColumn: "ma_bai_hoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cau_hoi",
                columns: table => new
                {
                    ma_cau_hoi = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_bai_kiem_tra = table.Column<int>(type: "INTEGER", nullable: false),
                    noi_dung = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cau_hoi", x => x.ma_cau_hoi);
                    table.ForeignKey(
                        name: "FK_cau_hoi_bai_kiem_tra_ma_bai_kiem_tra",
                        column: x => x.ma_bai_kiem_tra,
                        principalTable: "bai_kiem_tra",
                        principalColumn: "ma_bai_kiem_tra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ket_qua_kiem_tra",
                columns: table => new
                {
                    ma_ket_qua = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_bai_kiem_tra = table.Column<int>(type: "INTEGER", nullable: false),
                    ma_hoc_vien = table.Column<string>(type: "TEXT", nullable: false),
                    diem_so = table.Column<decimal>(type: "TEXT", nullable: false),
                    ngay_lam_bai = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ket_qua_kiem_tra", x => x.ma_ket_qua);
                    table.ForeignKey(
                        name: "FK_ket_qua_kiem_tra_bai_kiem_tra_ma_bai_kiem_tra",
                        column: x => x.ma_bai_kiem_tra,
                        principalTable: "bai_kiem_tra",
                        principalColumn: "ma_bai_kiem_tra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ket_qua_kiem_tra_nguoi_dung_ma_hoc_vien",
                        column: x => x.ma_hoc_vien,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dang_ky",
                columns: table => new
                {
                    ma_dang_ky = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_hoc_vien = table.Column<string>(type: "TEXT", nullable: false),
                    ma_lop_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    ngay_dang_ky = table.Column<DateTime>(type: "TEXT", nullable: false),
                    trang_thai = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dang_ky", x => x.ma_dang_ky);
                    table.ForeignKey(
                        name: "FK_dang_ky_lop_hoc_ma_lop_hoc",
                        column: x => x.ma_lop_hoc,
                        principalTable: "lop_hoc",
                        principalColumn: "ma_lop_hoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dang_ky_nguoi_dung_ma_hoc_vien",
                        column: x => x.ma_hoc_vien,
                        principalTable: "nguoi_dung",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lich_hoc",
                columns: table => new
                {
                    ma_lich_hoc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_lop_hoc = table.Column<int>(type: "INTEGER", nullable: false),
                    thu_trong_tuan = table.Column<string>(type: "TEXT", nullable: false),
                    gio_bat_dau = table.Column<string>(type: "TEXT", nullable: false),
                    gio_ket_thuc = table.Column<string>(type: "TEXT", nullable: false),
                    phong_hoc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lich_hoc", x => x.ma_lich_hoc);
                    table.ForeignKey(
                        name: "FK_lich_hoc_lop_hoc_ma_lop_hoc",
                        column: x => x.ma_lop_hoc,
                        principalTable: "lop_hoc",
                        principalColumn: "ma_lop_hoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dap_an",
                columns: table => new
                {
                    ma_dap_an = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_cau_hoi = table.Column<int>(type: "INTEGER", nullable: false),
                    noi_dung = table.Column<string>(type: "TEXT", nullable: false),
                    la_dap_an_dung = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dap_an", x => x.ma_dap_an);
                    table.ForeignKey(
                        name: "FK_dap_an_cau_hoi_ma_cau_hoi",
                        column: x => x.ma_cau_hoi,
                        principalTable: "cau_hoi",
                        principalColumn: "ma_cau_hoi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hoa_don",
                columns: table => new
                {
                    ma_hoa_don = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_dang_ky = table.Column<int>(type: "INTEGER", nullable: false),
                    so_tien = table.Column<decimal>(type: "TEXT", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    trang_thai = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoa_don", x => x.ma_hoa_don);
                    table.ForeignKey(
                        name: "FK_hoa_don_dang_ky_ma_dang_ky",
                        column: x => x.ma_dang_ky,
                        principalTable: "dang_ky",
                        principalColumn: "ma_dang_ky",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "thanh_toan",
                columns: table => new
                {
                    ma_thanh_toan = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ma_hoa_don = table.Column<int>(type: "INTEGER", nullable: false),
                    ngay_thanh_toan = table.Column<DateTime>(type: "TEXT", nullable: false),
                    phuong_thuc_thanh_toan = table.Column<string>(type: "TEXT", nullable: false),
                    trang_thai = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thanh_toan", x => x.ma_thanh_toan);
                    table.ForeignKey(
                        name: "FK_thanh_toan_hoa_don_ma_hoa_don",
                        column: x => x.ma_hoa_don,
                        principalTable: "hoa_don",
                        principalColumn: "ma_hoa_don",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "vai_tro",
                columns: new[] { "ma_vai_tro", "ten_vai_tro" },
                values: new object[,]
                {
                    { "admin", "Quan tri vien" },
                    { "teacher", "Giao vien" },
                    { "user", "Hoc vien" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bai_hoc_ma_khoa_hoc",
                table: "bai_hoc",
                column: "ma_khoa_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_bai_hoc_nguoi_tao",
                table: "bai_hoc",
                column: "nguoi_tao");

            migrationBuilder.CreateIndex(
                name: "IX_bai_kiem_tra_ma_khoa_hoc",
                table: "bai_kiem_tra",
                column: "ma_khoa_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_bai_kiem_tra_nguoi_tao",
                table: "bai_kiem_tra",
                column: "nguoi_tao");

            migrationBuilder.CreateIndex(
                name: "IX_cau_hoi_ma_bai_kiem_tra",
                table: "cau_hoi",
                column: "ma_bai_kiem_tra");

            migrationBuilder.CreateIndex(
                name: "IX_dang_ky_ma_hoc_vien",
                table: "dang_ky",
                column: "ma_hoc_vien");

            migrationBuilder.CreateIndex(
                name: "IX_dang_ky_ma_lop_hoc",
                table: "dang_ky",
                column: "ma_lop_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_danh_gia_ma_hoc_vien",
                table: "danh_gia",
                column: "ma_hoc_vien");

            migrationBuilder.CreateIndex(
                name: "IX_danh_gia_ma_khoa_hoc",
                table: "danh_gia",
                column: "ma_khoa_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_dap_an_ma_cau_hoi",
                table: "dap_an",
                column: "ma_cau_hoi");

            migrationBuilder.CreateIndex(
                name: "IX_hoa_don_ma_dang_ky",
                table: "hoa_don",
                column: "ma_dang_ky",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ket_qua_kiem_tra_ma_bai_kiem_tra",
                table: "ket_qua_kiem_tra",
                column: "ma_bai_kiem_tra");

            migrationBuilder.CreateIndex(
                name: "IX_ket_qua_kiem_tra_ma_hoc_vien",
                table: "ket_qua_kiem_tra",
                column: "ma_hoc_vien");

            migrationBuilder.CreateIndex(
                name: "IX_khoa_hoc_nguoi_tao",
                table: "khoa_hoc",
                column: "nguoi_tao");

            migrationBuilder.CreateIndex(
                name: "IX_lich_hoc_ma_lop_hoc",
                table: "lich_hoc",
                column: "ma_lop_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_lop_hoc_ma_giao_vien",
                table: "lop_hoc",
                column: "ma_giao_vien");

            migrationBuilder.CreateIndex(
                name: "IX_lop_hoc_ma_khoa_hoc",
                table: "lop_hoc",
                column: "ma_khoa_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_nguoi_dung_email",
                table: "nguoi_dung",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_nguoi_dung_vai_tro_ma_vai_tro",
                table: "nguoi_dung_vai_tro",
                column: "ma_vai_tro");

            migrationBuilder.CreateIndex(
                name: "IX_tai_lieu_ma_bai_hoc",
                table: "tai_lieu",
                column: "ma_bai_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_thanh_toan_ma_hoa_don",
                table: "thanh_toan",
                column: "ma_hoa_don",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "danh_gia");

            migrationBuilder.DropTable(
                name: "dap_an");

            migrationBuilder.DropTable(
                name: "ket_qua_kiem_tra");

            migrationBuilder.DropTable(
                name: "lich_hoc");

            migrationBuilder.DropTable(
                name: "nguoi_dung_vai_tro");

            migrationBuilder.DropTable(
                name: "tai_lieu");

            migrationBuilder.DropTable(
                name: "thanh_toan");

            migrationBuilder.DropTable(
                name: "cau_hoi");

            migrationBuilder.DropTable(
                name: "vai_tro");

            migrationBuilder.DropTable(
                name: "bai_hoc");

            migrationBuilder.DropTable(
                name: "hoa_don");

            migrationBuilder.DropTable(
                name: "bai_kiem_tra");

            migrationBuilder.DropTable(
                name: "dang_ky");

            migrationBuilder.DropTable(
                name: "lop_hoc");

            migrationBuilder.DropTable(
                name: "khoa_hoc");

            migrationBuilder.DropTable(
                name: "nguoi_dung");
        }
    }
}
