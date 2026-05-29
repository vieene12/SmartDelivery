using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class VietHoaDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                table: "ChiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHangs_HangHoas_MaHangHoa",
                table: "ChiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_KhachHangs_MaKhachHang",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HangHoas_NhomHangs_MaNhomHang",
                table: "HangHoas");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_AspNetUsers_UserId",
                table: "KhachHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuGiaoHangs_DonHangs_MaDonHang",
                table: "LichSuGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuGiaoHangs_NhanViens_MaNhanVien",
                table: "LichSuGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_AspNetUsers_UserId",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyHeThongs_NhanViens_MaNhanVien",
                table: "NhatKyHeThongs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCongGiaoHangs_DonHangs_MaDonHang",
                table: "PhanCongGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCongGiaoHangs_NhanViens_MaNhanVien",
                table: "PhanCongGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLyKhos_DonHangs_MaDonHang",
                table: "QuanLyKhos");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLyKhos_KhoHangs_MaKhoHang",
                table: "QuanLyKhos");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLyKhos_NhanViens_MaNhanVien",
                table: "QuanLyKhos");

            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToans_DonHangs_MaDonHang",
                table: "ThanhToans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThanhToans",
                table: "ThanhToans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuanLyKhos",
                table: "QuanLyKhos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhanCongGiaoHangs",
                table: "PhanCongGiaoHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhomHangs",
                table: "NhomHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhatKyHeThongs",
                table: "NhatKyHeThongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhanViens",
                table: "NhanViens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LichSuGiaoHangs",
                table: "LichSuGiaoHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhoHangs",
                table: "KhoHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhachHangs",
                table: "KhachHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HangHoas",
                table: "HangHoas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonHangs",
                table: "DonHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDonHangs",
                table: "ChiTietDonHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "ThanhToans",
                newName: "ThanhToan");

            migrationBuilder.RenameTable(
                name: "QuanLyKhos",
                newName: "QuanLyKho");

            migrationBuilder.RenameTable(
                name: "PhanCongGiaoHangs",
                newName: "PhanCongGiaoHang");

            migrationBuilder.RenameTable(
                name: "NhomHangs",
                newName: "NhomHang");

            migrationBuilder.RenameTable(
                name: "NhatKyHeThongs",
                newName: "NhatKyHeThong");

            migrationBuilder.RenameTable(
                name: "NhanViens",
                newName: "NhanVien");

            migrationBuilder.RenameTable(
                name: "LichSuGiaoHangs",
                newName: "LichSuGiaoHang");

            migrationBuilder.RenameTable(
                name: "KhoHangs",
                newName: "KhoHang");

            migrationBuilder.RenameTable(
                name: "KhachHangs",
                newName: "KhachHang");

            migrationBuilder.RenameTable(
                name: "HangHoas",
                newName: "HangHoa");

            migrationBuilder.RenameTable(
                name: "DonHangs",
                newName: "DonHang");

            migrationBuilder.RenameTable(
                name: "ChiTietDonHangs",
                newName: "ChiTietDonHang");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "NguoiDung_Token");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "NguoiDung");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "NguoiDung_VaiTro");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "NguoiDung_DangNhap");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "NguoiDung_Claim");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "VaiTro");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "VaiTro_Claim");

            migrationBuilder.RenameIndex(
                name: "IX_ThanhToans_MaDonHang",
                table: "ThanhToan",
                newName: "IX_ThanhToan_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_QuanLyKhos_MaNhanVien",
                table: "QuanLyKho",
                newName: "IX_QuanLyKho_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_QuanLyKhos_MaKhoHang",
                table: "QuanLyKho",
                newName: "IX_QuanLyKho_MaKhoHang");

            migrationBuilder.RenameIndex(
                name: "IX_QuanLyKhos_MaDonHang",
                table: "QuanLyKho",
                newName: "IX_QuanLyKho_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_PhanCongGiaoHangs_MaNhanVien",
                table: "PhanCongGiaoHang",
                newName: "IX_PhanCongGiaoHang_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_PhanCongGiaoHangs_MaDonHang",
                table: "PhanCongGiaoHang",
                newName: "IX_PhanCongGiaoHang_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_NhatKyHeThongs_MaNhanVien",
                table: "NhatKyHeThong",
                newName: "IX_NhatKyHeThong_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_UserId",
                table: "NhanVien",
                newName: "IX_NhanVien_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuGiaoHangs_MaNhanVien",
                table: "LichSuGiaoHang",
                newName: "IX_LichSuGiaoHang_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuGiaoHangs_MaDonHang",
                table: "LichSuGiaoHang",
                newName: "IX_LichSuGiaoHang_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_KhachHangs_UserId",
                table: "KhachHang",
                newName: "IX_KhachHang_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HangHoas_MaNhomHang",
                table: "HangHoa",
                newName: "IX_HangHoa_MaNhomHang");

            migrationBuilder.RenameIndex(
                name: "IX_DonHangs_MaKhachHang",
                table: "DonHang",
                newName: "IX_DonHang_MaKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonHangs_MaDonHang",
                table: "ChiTietDonHang",
                newName: "IX_ChiTietDonHang_MaDonHang");

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
                name: "UserName",
                table: "NguoiDung",
                newName: "TenDangNhap");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "NguoiDung",
                newName: "XacThucHaiYeuTo");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "NguoiDung",
                newName: "DauAnBaoMat");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "NguoiDung",
                newName: "XacNhanSoDienThoai");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "NguoiDung",
                newName: "SoDienThoai");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "NguoiDung",
                newName: "MatKhauDaMaHoa");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                table: "NguoiDung",
                newName: "TenDangNhapChuanHoa");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                table: "NguoiDung",
                newName: "EmailChuanHoa");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "NguoiDung",
                newName: "KhoaDenHan");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                table: "NguoiDung",
                newName: "ChoPhepKhoa");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "NguoiDung",
                newName: "HoTen");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "NguoiDung",
                newName: "XacNhanEmail");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "NguoiDung",
                newName: "DauAnDongThoi");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "NguoiDung",
                newName: "DiaChi");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "NguoiDung",
                newName: "SoDangNhapSai");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NguoiDung",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "NguoiDung_VaiTro",
                newName: "MaVaiTro");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NguoiDung_VaiTro",
                newName: "MaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "NguoiDung_VaiTro",
                newName: "IX_NguoiDung_VaiTro_MaVaiTro");

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
                name: "IX_AspNetUserLogins_UserId",
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
                name: "IX_AspNetUserClaims_UserId",
                table: "NguoiDung_Claim",
                newName: "IX_NguoiDung_Claim_MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "VaiTro",
                newName: "TenVaiTroChuanHoa");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VaiTro",
                newName: "TenVaiTro");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "VaiTro",
                newName: "DauAnDongThoi");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VaiTro",
                newName: "MaVaiTro");

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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "VaiTro_Claim",
                newName: "IX_VaiTro_Claim_MaVaiTro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThanhToan",
                table: "ThanhToan",
                column: "MaThanhToan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuanLyKho",
                table: "QuanLyKho",
                column: "MaQuanLyKho");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhanCongGiaoHang",
                table: "PhanCongGiaoHang",
                column: "MaPhanCong");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhomHang",
                table: "NhomHang",
                column: "MaNhomHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhatKyHeThong",
                table: "NhatKyHeThong",
                column: "MaNhatKy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhanVien",
                table: "NhanVien",
                column: "MaNhanVien");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LichSuGiaoHang",
                table: "LichSuGiaoHang",
                column: "MaLichSu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhoHang",
                table: "KhoHang",
                column: "MaKhoHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhachHang",
                table: "KhachHang",
                column: "MaKhachHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HangHoa",
                table: "HangHoa",
                column: "MaHangHoa");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonHang",
                table: "DonHang",
                column: "MaDonHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang",
                columns: new[] { "MaHangHoa", "MaDonHang" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung_Token",
                table: "NguoiDung_Token",
                columns: new[] { "MaNguoiDung", "NhaCungCapToken", "TenToken" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung",
                table: "NguoiDung",
                column: "MaNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung_VaiTro",
                table: "NguoiDung_VaiTro",
                columns: new[] { "MaNguoiDung", "MaVaiTro" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung_DangNhap",
                table: "NguoiDung_DangNhap",
                columns: new[] { "NhaCungCapDangNhap", "KhoaNhaCungCap" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung_Claim",
                table: "NguoiDung_Claim",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaiTro",
                table: "VaiTro",
                column: "MaVaiTro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaiTro_Claim",
                table: "VaiTro_Claim",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "NguoiDung",
                column: "TenDangNhapChuanHoa",
                unique: true,
                filter: "[TenDangNhapChuanHoa] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "VaiTro",
                column: "TenVaiTroChuanHoa",
                unique: true,
                filter: "[TenVaiTroChuanHoa] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_HangHoa_MaHangHoa",
                table: "ChiTietDonHang",
                column: "MaHangHoa",
                principalTable: "HangHoa",
                principalColumn: "MaHangHoa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_KhachHang_MaKhachHang",
                table: "DonHang",
                column: "MaKhachHang",
                principalTable: "KhachHang",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HangHoa_NhomHang_MaNhomHang",
                table: "HangHoa",
                column: "MaNhomHang",
                principalTable: "NhomHang",
                principalColumn: "MaNhomHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_NguoiDung_UserId",
                table: "KhachHang",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuGiaoHang_DonHang_MaDonHang",
                table: "LichSuGiaoHang",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuGiaoHang_NhanVien_MaNhanVien",
                table: "LichSuGiaoHang",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_NhanVien_NguoiDung_UserId",
                table: "NhanVien",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyHeThong_NhanVien_MaNhanVien",
                table: "NhatKyHeThong",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongGiaoHang_DonHang_MaDonHang",
                table: "PhanCongGiaoHang",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongGiaoHang_NhanVien_MaNhanVien",
                table: "PhanCongGiaoHang",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLyKho_DonHang_MaDonHang",
                table: "QuanLyKho",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLyKho_KhoHang_MaKhoHang",
                table: "QuanLyKho",
                column: "MaKhoHang",
                principalTable: "KhoHang",
                principalColumn: "MaKhoHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLyKho_NhanVien_MaNhanVien",
                table: "QuanLyKho",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToan_DonHang_MaDonHang",
                table: "ThanhToan",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VaiTro_Claim_VaiTro_MaVaiTro",
                table: "VaiTro_Claim",
                column: "MaVaiTro",
                principalTable: "VaiTro",
                principalColumn: "MaVaiTro",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_HangHoa_MaHangHoa",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_KhachHang_MaKhachHang",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_HangHoa_NhomHang_MaNhomHang",
                table: "HangHoa");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_NguoiDung_UserId",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuGiaoHang_DonHang_MaDonHang",
                table: "LichSuGiaoHang");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuGiaoHang_NhanVien_MaNhanVien",
                table: "LichSuGiaoHang");

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
                name: "FK_NhanVien_NguoiDung_UserId",
                table: "NhanVien");

            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyHeThong_NhanVien_MaNhanVien",
                table: "NhatKyHeThong");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCongGiaoHang_DonHang_MaDonHang",
                table: "PhanCongGiaoHang");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCongGiaoHang_NhanVien_MaNhanVien",
                table: "PhanCongGiaoHang");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLyKho_DonHang_MaDonHang",
                table: "QuanLyKho");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLyKho_KhoHang_MaKhoHang",
                table: "QuanLyKho");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLyKho_NhanVien_MaNhanVien",
                table: "QuanLyKho");

            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToan_DonHang_MaDonHang",
                table: "ThanhToan");

            migrationBuilder.DropForeignKey(
                name: "FK_VaiTro_Claim_VaiTro_MaVaiTro",
                table: "VaiTro_Claim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VaiTro_Claim",
                table: "VaiTro_Claim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VaiTro",
                table: "VaiTro");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "VaiTro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThanhToan",
                table: "ThanhToan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuanLyKho",
                table: "QuanLyKho");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhanCongGiaoHang",
                table: "PhanCongGiaoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhomHang",
                table: "NhomHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhatKyHeThong",
                table: "NhatKyHeThong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhanVien",
                table: "NhanVien");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung_VaiTro",
                table: "NguoiDung_VaiTro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung_Token",
                table: "NguoiDung_Token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung_DangNhap",
                table: "NguoiDung_DangNhap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung_Claim",
                table: "NguoiDung_Claim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung",
                table: "NguoiDung");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "NguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LichSuGiaoHang",
                table: "LichSuGiaoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhoHang",
                table: "KhoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhachHang",
                table: "KhachHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HangHoa",
                table: "HangHoa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonHang",
                table: "DonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.RenameTable(
                name: "VaiTro_Claim",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "VaiTro",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "ThanhToan",
                newName: "ThanhToans");

            migrationBuilder.RenameTable(
                name: "QuanLyKho",
                newName: "QuanLyKhos");

            migrationBuilder.RenameTable(
                name: "PhanCongGiaoHang",
                newName: "PhanCongGiaoHangs");

            migrationBuilder.RenameTable(
                name: "NhomHang",
                newName: "NhomHangs");

            migrationBuilder.RenameTable(
                name: "NhatKyHeThong",
                newName: "NhatKyHeThongs");

            migrationBuilder.RenameTable(
                name: "NhanVien",
                newName: "NhanViens");

            migrationBuilder.RenameTable(
                name: "NguoiDung_VaiTro",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "NguoiDung_Token",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "NguoiDung_DangNhap",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "NguoiDung_Claim",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "NguoiDung",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "LichSuGiaoHang",
                newName: "LichSuGiaoHangs");

            migrationBuilder.RenameTable(
                name: "KhoHang",
                newName: "KhoHangs");

            migrationBuilder.RenameTable(
                name: "KhachHang",
                newName: "KhachHangs");

            migrationBuilder.RenameTable(
                name: "HangHoa",
                newName: "HangHoas");

            migrationBuilder.RenameTable(
                name: "DonHang",
                newName: "DonHangs");

            migrationBuilder.RenameTable(
                name: "ChiTietDonHang",
                newName: "ChiTietDonHangs");

            migrationBuilder.RenameColumn(
                name: "MaVaiTro",
                table: "AspNetRoleClaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "LoaiClaim",
                table: "AspNetRoleClaims",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "GiaTriClaim",
                table: "AspNetRoleClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameIndex(
                name: "IX_VaiTro_Claim_MaVaiTro",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "TenVaiTroChuanHoa",
                table: "AspNetRoles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "TenVaiTro",
                table: "AspNetRoles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DauAnDongThoi",
                table: "AspNetRoles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "MaVaiTro",
                table: "AspNetRoles",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ThanhToan_MaDonHang",
                table: "ThanhToans",
                newName: "IX_ThanhToans_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_QuanLyKho_MaNhanVien",
                table: "QuanLyKhos",
                newName: "IX_QuanLyKhos_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_QuanLyKho_MaKhoHang",
                table: "QuanLyKhos",
                newName: "IX_QuanLyKhos_MaKhoHang");

            migrationBuilder.RenameIndex(
                name: "IX_QuanLyKho_MaDonHang",
                table: "QuanLyKhos",
                newName: "IX_QuanLyKhos_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_PhanCongGiaoHang_MaNhanVien",
                table: "PhanCongGiaoHangs",
                newName: "IX_PhanCongGiaoHangs_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_PhanCongGiaoHang_MaDonHang",
                table: "PhanCongGiaoHangs",
                newName: "IX_PhanCongGiaoHangs_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_NhatKyHeThong_MaNhanVien",
                table: "NhatKyHeThongs",
                newName: "IX_NhatKyHeThongs_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_NhanVien_UserId",
                table: "NhanViens",
                newName: "IX_NhanViens_UserId");

            migrationBuilder.RenameColumn(
                name: "MaVaiTro",
                table: "AspNetUserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_VaiTro_MaVaiTro",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "GiaTriToken",
                table: "AspNetUserTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "TenToken",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NhaCungCapToken",
                table: "AspNetUserTokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TenNhaCungCap",
                table: "AspNetUserLogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "KhoaNhaCungCap",
                table: "AspNetUserLogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "NhaCungCapDangNhap",
                table: "AspNetUserLogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_DangNhap_MaNguoiDung",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "LoaiClaim",
                table: "AspNetUserClaims",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "GiaTriClaim",
                table: "AspNetUserClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDung_Claim_MaNguoiDung",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameColumn(
                name: "XacThucHaiYeuTo",
                table: "AspNetUsers",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "XacNhanSoDienThoai",
                table: "AspNetUsers",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "XacNhanEmail",
                table: "AspNetUsers",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "TenDangNhapChuanHoa",
                table: "AspNetUsers",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "TenDangNhap",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "SoDienThoai",
                table: "AspNetUsers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SoDangNhapSai",
                table: "AspNetUsers",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "MatKhauDaMaHoa",
                table: "AspNetUsers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "KhoaDenHan",
                table: "AspNetUsers",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "HoTen",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "EmailChuanHoa",
                table: "AspNetUsers",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "DiaChi",
                table: "AspNetUsers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "DauAnDongThoi",
                table: "AspNetUsers",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "DauAnBaoMat",
                table: "AspNetUsers",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "ChoPhepKhoa",
                table: "AspNetUsers",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuGiaoHang_MaNhanVien",
                table: "LichSuGiaoHangs",
                newName: "IX_LichSuGiaoHangs_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuGiaoHang_MaDonHang",
                table: "LichSuGiaoHangs",
                newName: "IX_LichSuGiaoHangs_MaDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_KhachHang_UserId",
                table: "KhachHangs",
                newName: "IX_KhachHangs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HangHoa_MaNhomHang",
                table: "HangHoas",
                newName: "IX_HangHoas_MaNhomHang");

            migrationBuilder.RenameIndex(
                name: "IX_DonHang_MaKhachHang",
                table: "DonHangs",
                newName: "IX_DonHangs_MaKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonHang_MaDonHang",
                table: "ChiTietDonHangs",
                newName: "IX_ChiTietDonHangs_MaDonHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThanhToans",
                table: "ThanhToans",
                column: "MaThanhToan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuanLyKhos",
                table: "QuanLyKhos",
                column: "MaQuanLyKho");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhanCongGiaoHangs",
                table: "PhanCongGiaoHangs",
                column: "MaPhanCong");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhomHangs",
                table: "NhomHangs",
                column: "MaNhomHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhatKyHeThongs",
                table: "NhatKyHeThongs",
                column: "MaNhatKy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhanViens",
                table: "NhanViens",
                column: "MaNhanVien");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LichSuGiaoHangs",
                table: "LichSuGiaoHangs",
                column: "MaLichSu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhoHangs",
                table: "KhoHangs",
                column: "MaKhoHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhachHangs",
                table: "KhachHangs",
                column: "MaKhachHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HangHoas",
                table: "HangHoas",
                column: "MaHangHoa");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonHangs",
                table: "DonHangs",
                column: "MaDonHang");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDonHangs",
                table: "ChiTietDonHangs",
                columns: new[] { "MaHangHoa", "MaDonHang" });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHangs_HangHoas_MaHangHoa",
                table: "ChiTietDonHangs",
                column: "MaHangHoa",
                principalTable: "HangHoas",
                principalColumn: "MaHangHoa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_KhachHangs_MaKhachHang",
                table: "DonHangs",
                column: "MaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HangHoas_NhomHangs_MaNhomHang",
                table: "HangHoas",
                column: "MaNhomHang",
                principalTable: "NhomHangs",
                principalColumn: "MaNhomHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_AspNetUsers_UserId",
                table: "KhachHangs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuGiaoHangs_DonHangs_MaDonHang",
                table: "LichSuGiaoHangs",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuGiaoHangs_NhanViens_MaNhanVien",
                table: "LichSuGiaoHangs",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_AspNetUsers_UserId",
                table: "NhanViens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyHeThongs_NhanViens_MaNhanVien",
                table: "NhatKyHeThongs",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongGiaoHangs_DonHangs_MaDonHang",
                table: "PhanCongGiaoHangs",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongGiaoHangs_NhanViens_MaNhanVien",
                table: "PhanCongGiaoHangs",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLyKhos_DonHangs_MaDonHang",
                table: "QuanLyKhos",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLyKhos_KhoHangs_MaKhoHang",
                table: "QuanLyKhos",
                column: "MaKhoHang",
                principalTable: "KhoHangs",
                principalColumn: "MaKhoHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLyKhos_NhanViens_MaNhanVien",
                table: "QuanLyKhos",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToans_DonHangs_MaDonHang",
                table: "ThanhToans",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
