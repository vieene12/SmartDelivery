using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuanLyKhoAndNewERD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "KhoiLuongThucTe",
                table: "QuanLyKho",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MucDoNguyenVen",
                table: "QuanLyKho",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuongKienHang",
                table: "QuanLyKho",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TinhTrangBaoBi",
                table: "QuanLyKho",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KhoiLuongThucTe",
                table: "QuanLyKho");

            migrationBuilder.DropColumn(
                name: "MucDoNguyenVen",
                table: "QuanLyKho");

            migrationBuilder.DropColumn(
                name: "SoLuongKienHang",
                table: "QuanLyKho");

            migrationBuilder.DropColumn(
                name: "TinhTrangBaoBi",
                table: "QuanLyKho");
        }
    }
}
