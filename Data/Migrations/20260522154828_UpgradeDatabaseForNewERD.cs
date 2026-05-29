using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeDatabaseForNewERD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MatKhau",
                table: "NhanVien",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CaLamViec",
                columns: table => new
                {
                    MaCa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenCa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaLamViec", x => x.MaCa);
                });

            migrationBuilder.CreateTable(
                name: "ChucVu",
                columns: table => new
                {
                    MaChucVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenChucVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVu", x => x.MaChucVu);
                });

            migrationBuilder.CreateTable(
                name: "TraHang",
                columns: table => new
                {
                    MaTraHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayHoan = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraHang", x => x.MaTraHang);
                    table.ForeignKey(
                        name: "FK_TraHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhanCongCa",
                columns: table => new
                {
                    MaPhanCongCa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaCa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioVaoThucTe = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCongCa", x => x.MaPhanCongCa);
                    table.ForeignKey(
                        name: "FK_PhanCongCa_CaLamViec_MaCa",
                        column: x => x.MaCa,
                        principalTable: "CaLamViec",
                        principalColumn: "MaCa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhanCongCa_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichSuCongTac",
                columns: table => new
                {
                    MaLichSuCT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaChucVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhoHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuCongTac", x => x.MaLichSuCT);
                    table.ForeignKey(
                        name: "FK_LichSuCongTac_ChucVu_MaChucVu",
                        column: x => x.MaChucVu,
                        principalTable: "ChucVu",
                        principalColumn: "MaChucVu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichSuCongTac_KhoHang_MaKhoHang",
                        column: x => x.MaKhoHang,
                        principalTable: "KhoHang",
                        principalColumn: "MaKhoHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichSuCongTac_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichSuCongTac_MaChucVu",
                table: "LichSuCongTac",
                column: "MaChucVu");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuCongTac_MaKhoHang",
                table: "LichSuCongTac",
                column: "MaKhoHang");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuCongTac_MaNhanVien",
                table: "LichSuCongTac",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongCa_MaCa",
                table: "PhanCongCa",
                column: "MaCa");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongCa_MaNhanVien",
                table: "PhanCongCa",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_TraHang_MaDonHang",
                table: "TraHang",
                column: "MaDonHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichSuCongTac");

            migrationBuilder.DropTable(
                name: "PhanCongCa");

            migrationBuilder.DropTable(
                name: "TraHang");

            migrationBuilder.DropTable(
                name: "ChucVu");

            migrationBuilder.DropTable(
                name: "CaLamViec");

            migrationBuilder.DropColumn(
                name: "MatKhau",
                table: "NhanVien");
        }
    }
}
