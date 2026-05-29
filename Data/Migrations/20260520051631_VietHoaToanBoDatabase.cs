using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class VietHoaToanBoDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_Claim_NguoiDung_MaNguoiDung",
                table: "NguoiDung_Claim");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_DangNhap_NguoiDung_MaNguoiDung",
                table: "NguoiDung_DangNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_Token_NguoiDung_MaNguoiDung",
                table: "NguoiDung_Token");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_VaiTro_NguoiDung_MaNguoiDung",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_VaiTro_VaiTro_MaVaiTro",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_VaiTro_Claim_VaiTro_MaVaiTro",
                table: "VaiTro_Claim");

            migrationBuilder.RenameColumn(
                name: "MaVaiTro",
                table: "VaiTro_Claim",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "LoaiClaim",
                table: "VaiTro_Claim",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "GiaTriClaim",
                table: "VaiTro_Claim",
                newName: "ClaimValue");

            migrationBuilder.RenameIndex(
                name: "IX_VaiTro_Claim_MaVaiTro",
                table: "VaiTro_Claim",
                newName: "IX_VaiTro_Claim_RoleId");

            migrationBuilder.RenameColumn(
                name: "DauAnDongThoi",
                table: "VaiTro",
                newName: "DauDongThoi");

            migrationBuilder.RenameColumn(
                name: "MaVaiTro",
                table: "NguoiDung_VaiTro",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "NguoiDung_VaiTro",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_VaiTro_MaVaiTro",
                table: "NguoiDung_VaiTro",
                newName: "IX_NguoiDung_VaiTro_RoleId");

            migrationBuilder.RenameColumn(
                name: "GiaTriToken",
                table: "NguoiDung_Token",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "TenToken",
                table: "NguoiDung_Token",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NhaCungCapToken",
                table: "NguoiDung_Token",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "NguoiDung_Token",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TenNhaCungCap",
                table: "NguoiDung_DangNhap",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "NguoiDung_DangNhap",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "KhoaNhaCungCap",
                table: "NguoiDung_DangNhap",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "NhaCungCapDangNhap",
                table: "NguoiDung_DangNhap",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_DangNhap_MaNguoiDung",
                table: "NguoiDung_DangNhap",
                newName: "IX_NguoiDung_DangNhap_UserId");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "NguoiDung_Claim",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "LoaiClaim",
                table: "NguoiDung_Claim",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "GiaTriClaim",
                table: "NguoiDung_Claim",
                newName: "ClaimValue");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_Claim_MaNguoiDung",
                table: "NguoiDung_Claim",
                newName: "IX_NguoiDung_Claim_UserId");

            migrationBuilder.RenameColumn(
                name: "XacThucHaiYeuTo",
                table: "NguoiDung",
                newName: "KichHoatHaiLop");

            migrationBuilder.RenameColumn(
                name: "SoDangNhapSai",
                table: "NguoiDung",
                newName: "SoLanDangNhapSai");

            migrationBuilder.RenameColumn(
                name: "MatKhauDaMaHoa",
                table: "NguoiDung",
                newName: "MatKhauHash");

            migrationBuilder.RenameColumn(
                name: "KhoaDenHan",
                table: "NguoiDung",
                newName: "ThoiGianKhoa");

            migrationBuilder.RenameColumn(
                name: "DauAnDongThoi",
                table: "NguoiDung",
                newName: "DauDongThoi");

            migrationBuilder.RenameColumn(
                name: "DauAnBaoMat",
                table: "NguoiDung",
                newName: "DauBanMat");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_Claim_NguoiDung_UserId",
                table: "NguoiDung_Claim",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_DangNhap_NguoiDung_UserId",
                table: "NguoiDung_DangNhap",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_Token_NguoiDung_UserId",
                table: "NguoiDung_Token",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_VaiTro_NguoiDung_UserId",
                table: "NguoiDung_VaiTro",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_VaiTro_VaiTro_RoleId",
                table: "NguoiDung_VaiTro",
                column: "RoleId",
                principalTable: "VaiTro",
                principalColumn: "MaVaiTro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VaiTro_Claim_VaiTro_RoleId",
                table: "VaiTro_Claim",
                column: "RoleId",
                principalTable: "VaiTro",
                principalColumn: "MaVaiTro",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_Claim_NguoiDung_UserId",
                table: "NguoiDung_Claim");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_DangNhap_NguoiDung_UserId",
                table: "NguoiDung_DangNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_Token_NguoiDung_UserId",
                table: "NguoiDung_Token");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_VaiTro_NguoiDung_UserId",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDung_VaiTro_VaiTro_RoleId",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_VaiTro_Claim_VaiTro_RoleId",
                table: "VaiTro_Claim");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "VaiTro_Claim",
                newName: "MaVaiTro");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "VaiTro_Claim",
                newName: "GiaTriClaim");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "VaiTro_Claim",
                newName: "LoaiClaim");

            migrationBuilder.RenameIndex(
                name: "IX_VaiTro_Claim_RoleId",
                table: "VaiTro_Claim",
                newName: "IX_VaiTro_Claim_MaVaiTro");

            migrationBuilder.RenameColumn(
                name: "DauDongThoi",
                table: "VaiTro",
                newName: "DauAnDongThoi");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "NguoiDung_VaiTro",
                newName: "MaVaiTro");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDung_VaiTro",
                newName: "MaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_VaiTro_RoleId",
                table: "NguoiDung_VaiTro",
                newName: "IX_NguoiDung_VaiTro_MaVaiTro");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "NguoiDung_Token",
                newName: "GiaTriToken");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "NguoiDung_Token",
                newName: "TenToken");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "NguoiDung_Token",
                newName: "NhaCungCapToken");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDung_Token",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDung_DangNhap",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                table: "NguoiDung_DangNhap",
                newName: "TenNhaCungCap");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                table: "NguoiDung_DangNhap",
                newName: "KhoaNhaCungCap");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "NguoiDung_DangNhap",
                newName: "NhaCungCapDangNhap");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_DangNhap_UserId",
                table: "NguoiDung_DangNhap",
                newName: "IX_NguoiDung_DangNhap_MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDung_Claim",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "NguoiDung_Claim",
                newName: "GiaTriClaim");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "NguoiDung_Claim",
                newName: "LoaiClaim");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_Claim_UserId",
                table: "NguoiDung_Claim",
                newName: "IX_NguoiDung_Claim_MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "ThoiGianKhoa",
                table: "NguoiDung",
                newName: "KhoaDenHan");

            migrationBuilder.RenameColumn(
                name: "SoLanDangNhapSai",
                table: "NguoiDung",
                newName: "SoDangNhapSai");

            migrationBuilder.RenameColumn(
                name: "MatKhauHash",
                table: "NguoiDung",
                newName: "MatKhauDaMaHoa");

            migrationBuilder.RenameColumn(
                name: "KichHoatHaiLop",
                table: "NguoiDung",
                newName: "XacThucHaiYeuTo");

            migrationBuilder.RenameColumn(
                name: "DauDongThoi",
                table: "NguoiDung",
                newName: "DauAnDongThoi");

            migrationBuilder.RenameColumn(
                name: "DauBanMat",
                table: "NguoiDung",
                newName: "DauAnBaoMat");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_Claim_NguoiDung_MaNguoiDung",
                table: "NguoiDung_Claim",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_DangNhap_NguoiDung_MaNguoiDung",
                table: "NguoiDung_DangNhap",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_Token_NguoiDung_MaNguoiDung",
                table: "NguoiDung_Token",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_VaiTro_NguoiDung_MaNguoiDung",
                table: "NguoiDung_VaiTro",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDung_VaiTro_VaiTro_MaVaiTro",
                table: "NguoiDung_VaiTro",
                column: "MaVaiTro",
                principalTable: "VaiTro",
                principalColumn: "MaVaiTro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VaiTro_Claim_VaiTro_MaVaiTro",
                table: "VaiTro_Claim",
                column: "MaVaiTro",
                principalTable: "VaiTro",
                principalColumn: "MaVaiTro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
