using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseToFinal16Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichSuCongTac");

            migrationBuilder.DropTable(
                name: "LichSuGiaoHang");

            migrationBuilder.DropTable(
                name: "PhanCongGiaoHang");

            migrationBuilder.DropTable(
                name: "QuanLyKho");

            migrationBuilder.DropTable(
                name: "TraHang");

            migrationBuilder.DropTable(
                name: "ChucVu");

            migrationBuilder.CreateTable(
                name: "HanhTrinhDonHang",
                columns: table => new
                {
                    MaHanhTrinh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThoiGianTiepNhan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianHoanThanh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ViTriHienTai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LyDoThatBai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HinhAnhThucTe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HanhTrinhDonHang", x => x.MaHanhTrinh);
                    table.ForeignKey(
                        name: "FK_HanhTrinhDonHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HanhTrinhDonHang_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhapKho",
                columns: table => new
                {
                    MaNhapKho = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhoHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThoiGianNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViTriLuuTru = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThaiKho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    KhoiLuongThucTe = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    SoLuongKienHang = table.Column<int>(type: "int", nullable: true),
                    TinhTrangDonHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhapKho", x => x.MaNhapKho);
                    table.ForeignKey(
                        name: "FK_NhapKho_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhapKho_KhoHang_MaKhoHang",
                        column: x => x.MaKhoHang,
                        principalTable: "KhoHang",
                        principalColumn: "MaKhoHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NhapKho_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HanhTrinhDonHang_MaDonHang",
                table: "HanhTrinhDonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_HanhTrinhDonHang_MaNhanVien",
                table: "HanhTrinhDonHang",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhapKho_MaDonHang",
                table: "NhapKho",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_NhapKho_MaKhoHang",
                table: "NhapKho",
                column: "MaKhoHang");

            migrationBuilder.CreateIndex(
                name: "IX_NhapKho_MaNhanVien",
                table: "NhapKho",
                column: "MaNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HanhTrinhDonHang");

            migrationBuilder.DropTable(
                name: "NhapKho");

            migrationBuilder.CreateTable(
                name: "ChucVu",
                columns: table => new
                {
                    MaChucVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TenChucVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVu", x => x.MaChucVu);
                });

            migrationBuilder.CreateTable(
                name: "LichSuGiaoHang",
                columns: table => new
                {
                    MaLichSu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiCapNhat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ViTriHienTai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuGiaoHang", x => x.MaLichSu);
                    table.ForeignKey(
                        name: "FK_LichSuGiaoHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuGiaoHang_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhanCongGiaoHang",
                columns: table => new
                {
                    MaPhanCong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HinhAnhThucTe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LyDoThatBai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThoiGianHoanThanh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianTiepNhan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiGiaoHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCongGiaoHang", x => x.MaPhanCong);
                    table.ForeignKey(
                        name: "FK_PhanCongGiaoHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanCongGiaoHang_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuanLyKho",
                columns: table => new
                {
                    MaQuanLyKho = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhoHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KhoiLuongThucTe = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    MucDoNguyenVen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoLuongKienHang = table.Column<int>(type: "int", nullable: true),
                    ThoiGianNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TinhTrangBaoBi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiKho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ViTriLuuTru = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLyKho", x => x.MaQuanLyKho);
                    table.ForeignKey(
                        name: "FK_QuanLyKho_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuanLyKho_KhoHang_MaKhoHang",
                        column: x => x.MaKhoHang,
                        principalTable: "KhoHang",
                        principalColumn: "MaKhoHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuanLyKho_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraHang",
                columns: table => new
                {
                    MaTraHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgayHoan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                name: "LichSuCongTac",
                columns: table => new
                {
                    MaLichSuCT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaChucVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhoHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "IX_LichSuGiaoHang_MaDonHang",
                table: "LichSuGiaoHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuGiaoHang_MaNhanVien",
                table: "LichSuGiaoHang",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongGiaoHang_MaDonHang",
                table: "PhanCongGiaoHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongGiaoHang_MaNhanVien",
                table: "PhanCongGiaoHang",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLyKho_MaDonHang",
                table: "QuanLyKho",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLyKho_MaKhoHang",
                table: "QuanLyKho",
                column: "MaKhoHang");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLyKho_MaNhanVien",
                table: "QuanLyKho",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_TraHang_MaDonHang",
                table: "TraHang",
                column: "MaDonHang");
        }
    }
}
