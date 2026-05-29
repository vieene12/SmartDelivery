using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTuyenGiaoAndPhanCongTuyen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TuyenGiao",
                columns: table => new
                {
                    MaTuyen = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenTuyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KhuVuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyenGiao", x => x.MaTuyen);
                });

            migrationBuilder.CreateTable(
                name: "PhanCongTuyen",
                columns: table => new
                {
                    MaPhanCongTuyen = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaTuyen = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCongTuyen", x => x.MaPhanCongTuyen);
                    table.ForeignKey(
                        name: "FK_PhanCongTuyen_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhanCongTuyen_TuyenGiao_MaTuyen",
                        column: x => x.MaTuyen,
                        principalTable: "TuyenGiao",
                        principalColumn: "MaTuyen",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongTuyen_MaNhanVien",
                table: "PhanCongTuyen",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongTuyen_MaTuyen",
                table: "PhanCongTuyen",
                column: "MaTuyen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhanCongTuyen");

            migrationBuilder.DropTable(
                name: "TuyenGiao");
        }
    }
}
