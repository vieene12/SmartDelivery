SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. TẠO CƠ SỞ DỮ LIỆU MỚI NẾU CHƯA TỒN TẠI
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'SDMS_DB')
BEGIN
    CREATE DATABASE [SDMS_DB];
END;
GO

USE [SDMS_DB];
GO

-- 2. DỌN DẸP TOÀN BỘ BẢNG CŨ NẾU CÓ (Theo thứ tự khóa ngoại từ chi tiết đến bảng cha)
BEGIN TRANSACTION;

IF OBJECT_ID(N'[PhanCongCa]', N'U') IS NOT NULL DROP TABLE [PhanCongCa];
IF OBJECT_ID(N'[PhanCongTuyen]', N'U') IS NOT NULL DROP TABLE [PhanCongTuyen];
IF OBJECT_ID(N'[CaLamViec]', N'U') IS NOT NULL DROP TABLE [CaLamViec];
IF OBJECT_ID(N'[TuyenGiao]', N'U') IS NOT NULL DROP TABLE [TuyenGiao];
IF OBJECT_ID(N'[NhatKyHeThong]', N'U') IS NOT NULL DROP TABLE [NhatKyHeThong];
IF OBJECT_ID(N'[ThanhToan]', N'U') IS NOT NULL DROP TABLE [ThanhToan];
IF OBJECT_ID(N'[HanhTrinhDonHang]', N'U') IS NOT NULL DROP TABLE [HanhTrinhDonHang];
IF OBJECT_ID(N'[NhapKho]', N'U') IS NOT NULL DROP TABLE [NhapKho];
IF OBJECT_ID(N'[ChiTietDonHang]', N'U') IS NOT NULL DROP TABLE [ChiTietDonHang];
IF OBJECT_ID(N'[DonHang]', N'U') IS NOT NULL DROP TABLE [DonHang];
IF OBJECT_ID(N'[KhoHang]', N'U') IS NOT NULL DROP TABLE [KhoHang];
IF OBJECT_ID(N'[NhanVien]', N'U') IS NOT NULL DROP TABLE [NhanVien];
IF OBJECT_ID(N'[KhachHang]', N'U') IS NOT NULL DROP TABLE [KhachHang];
IF OBJECT_ID(N'[HangHoa]', N'U') IS NOT NULL DROP TABLE [HangHoa];
IF OBJECT_ID(N'[NhomHang]', N'U') IS NOT NULL DROP TABLE [NhomHang];

-- Dọn dẹp các bảng phụ của Identity
IF OBJECT_ID(N'[NguoiDung_Token]', N'U') IS NOT NULL DROP TABLE [NguoiDung_Token];
IF OBJECT_ID(N'[NguoiDung_DangNhap]', N'U') IS NOT NULL DROP TABLE [NguoiDung_DangNhap];
IF OBJECT_ID(N'[NguoiDung_Claim]', N'U') IS NOT NULL DROP TABLE [NguoiDung_Claim];
IF OBJECT_ID(N'[VaiTro_Claim]', N'U') IS NOT NULL DROP TABLE [VaiTro_Claim];
IF OBJECT_ID(N'[NguoiDung_VaiTro]', N'U') IS NOT NULL DROP TABLE [NguoiDung_VaiTro];
IF OBJECT_ID(N'[NguoiDung]', N'U') IS NOT NULL DROP TABLE [NguoiDung];
IF OBJECT_ID(N'[VaiTro]', N'U') IS NOT NULL DROP TABLE [VaiTro];

IF OBJECT_ID(N'[__EFMigrationsHistory]', N'U') IS NOT NULL DROP TABLE [__EFMigrationsHistory];

COMMIT;
GO

-- 3. TẠO CÁC BẢNG LƯU TRỮ VÀ RÀNG BUỘC KỸ THUẬT (DDL)
BEGIN TRANSACTION;

-- A. Các bảng cấu trúc phân quyền tài khoản (ASP.NET Core Identity Việt hóa)
CREATE TABLE [NguoiDung] (
    [MaNguoiDung] nvarchar(450) NOT NULL,
    [HoTen] nvarchar(max) NOT NULL,
    [DiaChi] nvarchar(max) NULL,
    [TenDangNhap] nvarchar(256) NULL,
    [TenDangNhapChuanHoa] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [EmailChuanHoa] nvarchar(256) NULL,
    [XacNhanEmail] bit NOT NULL,
    [MatKhauHash] nvarchar(max) NULL,
    [DauBanMat] nvarchar(max) NULL,
    [DauDongThoi] nvarchar(max) NULL,
    [SoDienThoai] nvarchar(max) NULL,
    [XacNhanSoDienThoai] bit NOT NULL,
    [KichHoatHaiLop] bit NOT NULL,
    [ThoiGianKhoa] datetimeoffset NULL,
    [ChoPhepKhoa] bit NOT NULL,
    [SoLanDangNhapSai] int NOT NULL,
    CONSTRAINT [PK_NguoiDung] PRIMARY KEY ([MaNguoiDung])
);

CREATE TABLE [VaiTro] (
    [MaVaiTro] nvarchar(450) NOT NULL,
    [TenVaiTro] nvarchar(256) NULL,
    [TenVaiTroChuanHoa] nvarchar(256) NULL,
    [DauDongThoi] nvarchar(max) NULL,
    CONSTRAINT [PK_VaiTro] PRIMARY KEY ([MaVaiTro])
);

CREATE TABLE [NguoiDung_VaiTro] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_NguoiDung_VaiTro] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_NguoiDung_VaiTro_NguoiDung_UserId] FOREIGN KEY ([UserId]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE CASCADE,
    CONSTRAINT [FK_NguoiDung_VaiTro_VaiTro_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [VaiTro] ([MaVaiTro]) ON DELETE CASCADE
);

CREATE TABLE [NguoiDung_Claim] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_NguoiDung_Claim] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NguoiDung_Claim_NguoiDung_UserId] FOREIGN KEY ([UserId]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE CASCADE
);

CREATE TABLE [VaiTro_Claim] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_VaiTro_Claim] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VaiTro_Claim_VaiTro_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [VaiTro] ([MaVaiTro]) ON DELETE CASCADE
);

CREATE TABLE [NguoiDung_DangNhap] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_NguoiDung_DangNhap] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_NguoiDung_DangNhap_NguoiDung_UserId] FOREIGN KEY ([UserId]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE CASCADE
);

CREATE TABLE [NguoiDung_Token] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_NguoiDung_Token] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_NguoiDung_Token_NguoiDung_UserId] FOREIGN KEY ([UserId]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE CASCADE
);

-- B. Bảng Nhóm hàng & Hàng hóa
CREATE TABLE [NhomHang] (
    [MaNhomHang] nvarchar(20) NOT NULL,
    [TenNhomHang] nvarchar(100) NOT NULL,
    [MoTa] nvarchar(200) NULL,
    CONSTRAINT [PK_NhomHang] PRIMARY KEY ([MaNhomHang])
);

CREATE TABLE [HangHoa] (
    [MaHangHoa] nvarchar(20) NOT NULL,
    [MaNhomHang] nvarchar(20) NOT NULL,
    [TenHangHoa] nvarchar(100) NOT NULL,
    [DonViTinh] nvarchar(50) NULL,
    [KhoiLuong] decimal(10,2) NOT NULL,
    [KichThuoc] nvarchar(100) NULL,
    [MoTaChiTiet] nvarchar(255) NULL,
    CONSTRAINT [PK_HangHoa] PRIMARY KEY ([MaHangHoa]),
    CONSTRAINT [FK_HangHoa_NhomHang_MaNhomHang] FOREIGN KEY ([MaNhomHang]) REFERENCES [NhomHang] ([MaNhomHang]) ON DELETE CASCADE
);

-- C. Bảng Khách hàng & Nhân viên
CREATE TABLE [KhachHang] (
    [MaKhachHang] nvarchar(20) NOT NULL,
    [HoTen] nvarchar(100) NOT NULL,
    [SoDienThoai] nvarchar(15) NOT NULL,
    [DiaChi] nvarchar(255) NULL,
    [Email] nvarchar(100) NULL,
    [UserId] nvarchar(450) NULL,
    CONSTRAINT [PK_KhachHang] PRIMARY KEY ([MaKhachHang]),
    CONSTRAINT [FK_KhachHang_NguoiDung_UserId] FOREIGN KEY ([UserId]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE SET NULL
);

CREATE TABLE [NhanVien] (
    [MaNhanVien] nvarchar(20) NOT NULL,
    [HoTen] nvarchar(100) NOT NULL,
    [GioiTinh] nvarchar(10) NULL,
    [NgaySinh] datetime2 NOT NULL,
    [ChucVu] nvarchar(50) NOT NULL,
    [SoDienThoai] nvarchar(15) NULL,
    [DiaChi] nvarchar(255) NULL,
    [Email] nvarchar(100) NULL,
    [TrangThaiLamViec] nvarchar(150) NULL,
    [MatKhau] nvarchar(255) NOT NULL DEFAULT N'123456',
    [UserId] nvarchar(450) NULL,
    CONSTRAINT [PK_NhanVien] PRIMARY KEY ([MaNhanVien]),
    CONSTRAINT [FK_NhanVien_NguoiDung_UserId] FOREIGN KEY ([UserId]) REFERENCES [NguoiDung] ([MaNguoiDung]) ON DELETE SET NULL
);

-- D. Bảng Kho bãi
CREATE TABLE [KhoHang] (
    [MaKhoHang] nvarchar(20) NOT NULL,
    [TenKho] nvarchar(100) NOT NULL,
    [DiaChiKho] nvarchar(255) NULL,
    [DienTichKho] decimal(10,2) NOT NULL,
    [SucChuaKho] int NOT NULL,
    [TrangThai] nvarchar(50) NOT NULL DEFAULT N'Hoạt động',
    CONSTRAINT [PK_KhoHang] PRIMARY KEY ([MaKhoHang])
);

-- E. Bảng Đơn hàng & Chi tiết Đơn hàng
CREATE TABLE [DonHang] (
    [MaDonHang] nvarchar(20) NOT NULL,
    [MaKhachHang] nvarchar(20) NOT NULL,
    [TenNguoiNhan] nvarchar(100) NOT NULL,
    [SoDienThoaiNguoiNhan] nvarchar(15) NOT NULL,
    [DiaChiNguoiNhan] nvarchar(255) NULL,
    [TongKhoiLuong] decimal(10,2) NOT NULL,
    [PhiGiaoHang] decimal(18,2) NOT NULL,
    [HinhThucThanhToan] nvarchar(50) NULL,
    [TrangThaiDonHang] nvarchar(50) NOT NULL DEFAULT N'Mới tạo',
    [NgayGiaoDuKien] datetime2 NULL,
    [NgayHoanThanh] datetime2 NULL,
    [ThoiGianTao] datetime2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_DonHang] PRIMARY KEY ([MaDonHang]),
    CONSTRAINT [FK_DonHang_KhachHang_MaKhachHang] FOREIGN KEY ([MaKhachHang]) REFERENCES [KhachHang] ([MaKhachHang]) ON DELETE CASCADE
);

CREATE TABLE [ChiTietDonHang] (
    [MaHangHoa] nvarchar(20) NOT NULL,
    [MaDonHang] nvarchar(20) NOT NULL,
    [SoLuong] int NOT NULL,
    [TinhTrangHangHoa] nvarchar(255) NULL,
    CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY ([MaHangHoa], [MaDonHang]),
    CONSTRAINT [FK_ChiTietDonHang_DonHang_MaDonHang] FOREIGN KEY ([MaDonHang]) REFERENCES [DonHang] ([MaDonHang]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChiTietDonHang_HangHoa_MaHangHoa] FOREIGN KEY ([MaHangHoa]) REFERENCES [HangHoa] ([MaHangHoa]) ON DELETE CASCADE
);

-- F. Bảng Nhập kho kiểm định
CREATE TABLE [NhapKho] (
    [MaNhapKho] nvarchar(20) NOT NULL,
    [MaDonHang] nvarchar(20) NOT NULL,
    [MaKhoHang] nvarchar(20) NOT NULL,
    [MaNhanVien] nvarchar(20) NOT NULL,
    [ThoiGianNhap] datetime2 NOT NULL DEFAULT GETDATE(),
    [ViTriLuuTru] nvarchar(100) NULL,
    [TrangThaiKho] nvarchar(100) NULL,
    [KhoiLuongThucTe] decimal(10,2) NULL,
    [SoLuongKienHang] int NULL,
    [TinhTrangDonHang] nvarchar(255) NULL,
    CONSTRAINT [PK_NhapKho] PRIMARY KEY ([MaNhapKho]),
    CONSTRAINT [FK_NhapKho_DonHang_MaDonHang] FOREIGN KEY ([MaDonHang]) REFERENCES [DonHang] ([MaDonHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK_NhapKho_KhoHang_MaKhoHang] FOREIGN KEY ([MaKhoHang]) REFERENCES [KhoHang] ([MaKhoHang]) ON DELETE CASCADE,
    CONSTRAINT [FK_NhapKho_NhanVien_MaNhanVien] FOREIGN KEY ([MaNhanVien]) REFERENCES [NhanVien] ([MaNhanVien]) ON DELETE CASCADE
);

-- G. Bảng Hành trình vận chuyển
CREATE TABLE [HanhTrinhDonHang] (
    [MaHanhTrinh] nvarchar(20) NOT NULL,
    [MaDonHang] nvarchar(20) NOT NULL,
    [MaNhanVien] nvarchar(20) NOT NULL,
    [ThoiGianTiepNhan] datetime2 NOT NULL DEFAULT GETDATE(),
    [ThoiGianHoanThanh] datetime2 NULL,
    [TrangThai] nvarchar(50) NOT NULL DEFAULT N'Chờ shipper lấy',
    [ViTriHienTai] nvarchar(255) NULL,
    [LyDoThatBai] nvarchar(255) NULL,
    [HinhAnhThucTe] nvarchar(255) NULL,
    CONSTRAINT [PK_HanhTrinhDonHang] PRIMARY KEY ([MaHanhTrinh]),
    CONSTRAINT [FK_HanhTrinhDonHang_DonHang_MaDonHang] FOREIGN KEY ([MaDonHang]) REFERENCES [DonHang] ([MaDonHang]) ON DELETE NO ACTION,
    CONSTRAINT [FK_HanhTrinhDonHang_NhanVien_MaNhanVien] FOREIGN KEY ([MaNhanVien]) REFERENCES [NhanVien] ([MaNhanVien]) ON DELETE CASCADE
);

-- H. Bảng Thanh toán (COD/Banking)
CREATE TABLE [ThanhToan] (
    [MaThanhToan] nvarchar(20) NOT NULL,
    [MaDonHang] nvarchar(20) NOT NULL,
    [MaShipper] nvarchar(20) NULL,
    [SoTienThanhToan] decimal(18,2) NOT NULL,
    [PhuongThucThanhToan] nvarchar(50) NULL,
    [ThoiGianThanhToan] datetime2 NOT NULL DEFAULT GETDATE(),
    [TrangThaiThanhToan] nvarchar(50) NULL,
    CONSTRAINT [PK_ThanhToan] PRIMARY KEY ([MaThanhToan]),
    CONSTRAINT [FK_ThanhToan_DonHang_MaDonHang] FOREIGN KEY ([MaDonHang]) REFERENCES [DonHang] ([MaDonHang]) ON DELETE NO ACTION
);

-- I. Bảng Nhật ký hệ thống (Audit Logs)
CREATE TABLE [NhatKyHeThong] (
    [MaNhatKy] nvarchar(20) NOT NULL,
    [MaNhanVien] nvarchar(20) NOT NULL,
    [HanhDong] nvarchar(100) NOT NULL,
    [DuLieuTacDong] nvarchar(100) NULL,
    [ThoiGian] datetime2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_NhatKyHeThong] PRIMARY KEY ([MaNhatKy]),
    CONSTRAINT [FK_NhatKyHeThong_NhanVien_MaNhanVien] FOREIGN KEY ([MaNhanVien]) REFERENCES [NhanVien] ([MaNhanVien]) ON DELETE CASCADE
);

-- J. Bảng Tuyến giao & Phân công tuyến
CREATE TABLE [TuyenGiao] (
    [MaTuyen] nvarchar(20) NOT NULL,
    [TenTuyen] nvarchar(100) NOT NULL,
    [KhuVuc] nvarchar(100) NOT NULL,
    [MoTa] nvarchar(255) NULL,
    CONSTRAINT [PK_TuyenGiao] PRIMARY KEY ([MaTuyen])
);

CREATE TABLE [PhanCongTuyen] (
    [MaPhanCongTuyen] nvarchar(20) NOT NULL,
    [MaNhanVien] nvarchar(20) NOT NULL,
    [MaTuyen] nvarchar(20) NOT NULL,
    [NgayBatDau] datetime2 NOT NULL DEFAULT GETDATE(),
    [NgayKetThuc] datetime2 NULL,
    CONSTRAINT [PK_PhanCongTuyen] PRIMARY KEY ([MaPhanCongTuyen]),
    CONSTRAINT [FK_PhanCongTuyen_NhanVien_MaNhanVien] FOREIGN KEY ([MaNhanVien]) REFERENCES [NhanVien] ([MaNhanVien]) ON DELETE CASCADE,
    CONSTRAINT [FK_PhanCongTuyen_TuyenGiao_MaTuyen] FOREIGN KEY ([MaTuyen]) REFERENCES [TuyenGiao] ([MaTuyen]) ON DELETE CASCADE
);

-- K. Bảng Ca làm việc & Điểm danh
CREATE TABLE [CaLamViec] (
    [MaCa] nvarchar(20) NOT NULL,
    [TenCa] nvarchar(100) NOT NULL,
    [GioBatDau] datetime2 NOT NULL,
    [GioKetThuc] datetime2 NOT NULL,
    CONSTRAINT [PK_CaLamViec] PRIMARY KEY ([MaCa])
);

CREATE TABLE [PhanCongCa] (
    [MaPhanCongCa] nvarchar(20) NOT NULL,
    [MaCa] nvarchar(20) NOT NULL,
    [MaNhanVien] nvarchar(20) NOT NULL,
    [NgayLam] datetime2 NOT NULL,
    [TrangThai] nvarchar(100) NOT NULL,
    [GioVaoThucTe] datetime2 NULL,
    CONSTRAINT [PK_PhanCongCa] PRIMARY KEY ([MaPhanCongCa]),
    CONSTRAINT [FK_PhanCongCa_CaLamViec_MaCa] FOREIGN KEY ([MaCa]) REFERENCES [CaLamViec] ([MaCa]) ON DELETE CASCADE,
    CONSTRAINT [FK_PhanCongCa_NhanVien_MaNhanVien] FOREIGN KEY ([MaNhanVien]) REFERENCES [NhanVien] ([MaNhanVien]) ON DELETE CASCADE
);

-- L. EF Migrations History
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
);

COMMIT;
GO

-- 4. TẠO CÁC CHỈ MỤC (INDEXES) ĐỂ TỐI ƯU HÓA TRUY VẤN
BEGIN TRANSACTION;

CREATE UNIQUE INDEX [UserNameIndex] ON [NguoiDung] ([TenDangNhapChuanHoa]) WHERE [TenDangNhapChuanHoa] IS NOT NULL;
CREATE INDEX [EmailIndex] ON [NguoiDung] ([EmailChuanHoa]);
CREATE UNIQUE INDEX [RoleNameIndex] ON [VaiTro] ([TenVaiTroChuanHoa]) WHERE [TenVaiTroChuanHoa] IS NOT NULL;

CREATE INDEX [IX_NguoiDung_VaiTro_RoleId] ON [NguoiDung_VaiTro] ([RoleId]);
CREATE INDEX [IX_NguoiDung_Claim_UserId] ON [NguoiDung_Claim] ([UserId]);
CREATE INDEX [IX_VaiTro_Claim_RoleId] ON [VaiTro_Claim] ([RoleId]);
CREATE INDEX [IX_NguoiDung_DangNhap_UserId] ON [NguoiDung_DangNhap] ([UserId]);

CREATE INDEX [IX_KhachHang_UserId] ON [KhachHang] ([UserId]);
CREATE INDEX [IX_NhanVien_UserId] ON [NhanVien] ([UserId]);
CREATE INDEX [IX_HangHoa_MaNhomHang] ON [HangHoa] ([MaNhomHang]);
CREATE INDEX [IX_DonHang_MaKhachHang] ON [DonHang] ([MaKhachHang]);
CREATE INDEX [IX_ChiTietDonHang_MaDonHang] ON [ChiTietDonHang] ([MaDonHang]);

CREATE INDEX [IX_NhapKho_MaDonHang] ON [NhapKho] ([MaDonHang]);
CREATE INDEX [IX_NhapKho_MaKhoHang] ON [NhapKho] ([MaKhoHang]);
CREATE INDEX [IX_NhapKho_MaNhanVien] ON [NhapKho] ([MaNhanVien]);

CREATE INDEX [IX_HanhTrinhDonHang_MaDonHang] ON [HanhTrinhDonHang] ([MaDonHang]);
CREATE INDEX [IX_HanhTrinhDonHang_MaNhanVien] ON [HanhTrinhDonHang] ([MaNhanVien]);

CREATE INDEX [IX_ThanhToan_MaDonHang] ON [ThanhToan] ([MaDonHang]);
CREATE INDEX [IX_NhatKyHeThong_MaNhanVien] ON [NhatKyHeThong] ([MaNhanVien]);

CREATE INDEX [IX_PhanCongTuyen_MaNhanVien] ON [PhanCongTuyen] ([MaNhanVien]);
CREATE INDEX [IX_PhanCongTuyen_MaTuyen] ON [PhanCongTuyen] ([MaTuyen]);
CREATE INDEX [IX_PhanCongCa_MaCa] ON [PhanCongCa] ([MaCa]);
CREATE INDEX [IX_PhanCongCa_MaNhanVien] ON [PhanCongCa] ([MaNhanVien]);

COMMIT;
GO


-- 5. CHÈN DỮ LIỆU SEED KIỂM THỬ BAN ĐẦU ĐẦY ĐỦ VÀ PHONG PHÚ (DML)
BEGIN TRANSACTION;

-- A. Seed các vai trò cốt lõi trong hệ thống (VaiTro)
INSERT INTO [VaiTro] ([MaVaiTro], [TenVaiTro], [TenVaiTroChuanHoa], [DauDongThoi])
VALUES 
('admin-role-id', 'Admin', 'ADMIN', NEWID()),
('warehouse-role-id', 'WarehouseStaff', 'WAREHOUSESTAFF', NEWID()),
('shipper-role-id', 'Shipper', 'SHIPPER', NEWID()),
('customer-role-id', 'Customer', 'CUSTOMER', NEWID());

-- Mật khẩu mã hóa cho chuỗi 'Admin@123' mặc định
DECLARE @DefaultPasswordHash nvarchar(max) = 'AQAAAAIAAYagAAAAEG3g1H5qR+w1cTqI4lA5gO0h/8nQ+Z1p/0gA5rB7o4W/3kF6g9z5gG9g+u0G9g==';

-- B. Seed các tài khoản người dùng mẫu (NguoiDung)
INSERT INTO [NguoiDung] (
    [MaNguoiDung], [TenDangNhap], [TenDangNhapChuanHoa], [Email], [EmailChuanHoa], 
    [XacNhanEmail], [MatKhauHash], [DauBanMat], [DauDongThoi], [SoDienThoai], 
    [XacNhanSoDienThoai], [KichHoatHaiLop], [ThoiGianKhoa], [ChoPhepKhoa], [SoLanDangNhapSai],
    [HoTen], [DiaChi]
)
VALUES 
('admin-test-id', 'admin@sdms.com', 'ADMIN@SDMS.COM', 'admin@sdms.com', 'ADMIN@SDMS.COM', 1, @DefaultPasswordHash, NEWID(), NEWID(), '0900000001', 1, 0, NULL, 1, 0, N'System Administrator', N'Văn phòng Ocean Tech, TP. HCM'),
('warehouse-staff-id-01', 'warehouse@sdms.com', 'WAREHOUSE@SDMS.COM', 'warehouse@sdms.com', 'WAREHOUSE@SDMS.COM', 1, @DefaultPasswordHash, NEWID(), NEWID(), '0933333333', 1, 0, NULL, 1, 0, N'Phạm Thị Thủ Kho', N'456 Võ Văn Tần, Quận 3, TP. HCM'),
('shipper-test-id', 'shipper@sdms.com', 'SHIPPER@SDMS.COM', 'shipper@sdms.com', 'SHIPPER@SDMS.COM', 1, @DefaultPasswordHash, NEWID(), NEWID(), '0987654321', 1, 0, NULL, 1, 0, N'Nguyễn Văn Shipper', N'123 Nguyễn Huệ, Quận 1, TP. HCM'),
('shipper-staff-id-02', 'shipper2@sdms.com', 'SHIPPER2@SDMS.COM', 'shipper2@sdms.com', 'SHIPPER2@SDMS.COM', 1, @DefaultPasswordHash, NEWID(), NEWID(), '0922222222', 1, 0, NULL, 1, 0, N'Trần Văn Bưu Tá', N'789 Cách Mạng Tháng 8, Quận 10, TP. HCM'),
('customer-test-id', 'customer@sdms.com', 'CUSTOMER@SDMS.COM', 'customer@sdms.com', 'CUSTOMER@SDMS.COM', 1, @DefaultPasswordHash, NEWID(), NEWID(), '0909090909', 1, 0, NULL, 1, 0, N'Lê Thị Khách Hàng', N'78 Lê Lợi, Quận 1, TP. HCM');

-- C. Gán vai trò tài khoản người dùng (NguoiDung_VaiTro)
INSERT INTO [NguoiDung_VaiTro] ([UserId], [RoleId])
VALUES 
('admin-test-id', 'admin-role-id'),
('warehouse-staff-id-01', 'warehouse-role-id'),
('shipper-test-id', 'shipper-role-id'),
('shipper-staff-id-02', 'shipper-role-id'),
('customer-test-id', 'customer-role-id');

-- D. Tạo hồ sơ Nhân viên (NhanVien)
INSERT INTO [NhanVien] (
    [MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [ChucVu], 
    [SoDienThoai], [DiaChi], [Email], [TrangThaiLamViec], [MatKhau], [UserId]
)
VALUES 
('NV001', N'System Administrator', N'Nam', '1990-01-01', 'Admin', '0900000001', N'Văn phòng Ocean Tech, TP. HCM', 'admin@sdms.com', N'Đang làm việc', 'Admin@123', 'admin-test-id'),
('NV_WAREHOUSE_01', N'Phạm Thị Thủ Kho', N'Nữ', '1992-08-20', 'WarehouseStaff', '0933333333', N'456 Võ Văn Tần, Quận 3, TP. HCM', 'warehouse@sdms.com', N'Đang làm việc', 'Admin@123', 'warehouse-staff-id-01'),
('NV_SHIPPER_TEST', N'Nguyễn Văn Shipper', N'Nam', '1995-05-15', 'Shipper', '0987654321', N'123 Nguyễn Huệ, Quận 1, TP. HCM', 'shipper@sdms.com', N'Đang làm việc', 'Admin@123', 'shipper-test-id'),
('NV_SHIPPER_02', N'Trần Văn Bưu Tá', N'Nam', '1997-11-02', 'Shipper', '0922222222', N'789 Cách Mạng Tháng 8, Quận 10, TP. HCM', 'shipper2@sdms.com', N'Đang làm việc', 'Admin@123', 'shipper-staff-id-02');

-- E. Tạo danh sách Khách hàng kiểm thử (KhachHang)
INSERT INTO [KhachHang] ([MaKhachHang], [HoTen], [SoDienThoai], [DiaChi], [Email], [UserId])
VALUES 
('KH_TEST_01', N'Lê Thị Khách Hàng', '0909090909', N'Quận 1, TP. HCM', 'customer@sdms.com', 'customer-test-id'),
('KH_TEST_02', N'Nguyễn Văn An', '0901234567', N'22 Hàng Tre, Hoàn Kiếm, Hà Nội', 'an.nguyen@gmail.com', NULL),
('KH_TEST_03', N'Trần Thị Bình', '0902345678', N'120 Hai Bà Trưng, Quận 1, TP. HCM', 'binh.tran@gmail.com', NULL),
('KH_TEST_04', N'Lê Văn Cường', '0903456789', N'34 Nguyễn Thị Minh Khai, Quận 3, TP. HCM', 'cuong.le@gmail.com', NULL);

-- F. Tạo kho bãi trung chuyển (KhoHang)
INSERT INTO [KhoHang] ([MaKhoHang], [TenKho], [DiaChiKho], [DienTichKho], [SucChuaKho], [TrangThai])
VALUES 
('K01', N'Kho Quận 1', N'Quận 1, TP. HCM', 1000.00, 5000, N'Hoạt động'),
('K02', N'Kho Trung Chuyển Miền Bắc', N'Hà Nội', 4500.00, 9000, N'Hoạt động');

-- G. Tạo các Nhóm hàng phân loại (NhomHang)
INSERT INTO [NhomHang] ([MaNhomHang], [TenNhomHang], [MoTa])
VALUES 
('DT', N'Điện tử', N'Điện thoại, máy tính, phụ kiện công nghệ'),
('DV', N'Dễ vỡ', N'Gốm sứ, thủy tinh, vật liệu mỹ nghệ'),
('TC', N'Cồng kềnh', N'Tủ lạnh, máy giặt, đồ nội thất lớn');

-- H. Tạo danh sách các Hàng hóa cụ thể (HangHoa)
INSERT INTO [HangHoa] ([MaHangHoa], [MaNhomHang], [TenHangHoa], [DonViTinh], [KhoiLuong], [KichThuoc], [MoTaChiTiet])
VALUES 
('HH_TEST_01', 'DT', N'iPhone 15 Pro Max', N'Cái', 0.25, '15x8x1 cm', N'Điện thoại thông minh Apple'),
('HH_TEST_02', 'DT', N'Laptop Dell XPS 13', N'Cái', 1.80, '30x20x2 cm', N'Máy tính xách tay văn phòng cao cấp'),
('HH_TEST_03', 'DV', N'Bộ Ly Thủy Tinh (6 cái)', N'Hộp', 0.60, '25x15x10 cm', N'Sản phẩm thủy tinh dễ vỡ'),
('HH_TEST_04', 'TC', N'Tủ Lạnh LG Smart Inverter', N'Cái', 75.00, '180x80x75 cm', N'Thiết bị gia dụng bảo quản lạnh');

-- I. Thiết lập Tuyến đường giao hàng địa lý (TuyenGiao)
INSERT INTO [TuyenGiao] ([MaTuyen], [TenTuyen], [KhuVuc], [MoTa])
VALUES 
('TG_Q1', N'Quận 1', N'TP. Hồ Chí Minh', N'Tuyến giao bưu cục Quận 1 - Trung tâm tài chính'),
('TG_Q3', N'Quận 3', N'TP. Hồ Chí Minh', N'Tuyến giao bưu cục Quận 3 - Nội thành'),
('TG_HN_DD', N'Đống Đa', N'Hà Nội', N'Tuyến bưu cục Hà Nội quận Đống Đa');

-- J. Phân công Tuyến vận hành cho shipper (PhanCongTuyen)
INSERT INTO [PhanCongTuyen] ([MaPhanCongTuyen], [MaNhanVien], [MaTuyen], [NgayBatDau], [NgayKetThuc])
VALUES 
('PCT_TEST_01', 'NV_SHIPPER_TEST', 'TG_Q1', GETDATE(), DATEADD(year, 1, GETDATE())),
('PCT_TEST_02', 'NV_SHIPPER_02', 'TG_Q3', GETDATE(), DATEADD(year, 1, GETDATE()));

-- K. Tạo Ca làm việc (CaLamViec)
INSERT INTO [CaLamViec] ([MaCa], [TenCa], [GioBatDau], [GioKetThuc])
VALUES 
('CA_SANG', N'Ca Sáng', '2026-05-22 08:00:00', '2026-05-22 12:00:00'),
('CA_CHIEU', N'Ca Chiều', '2026-05-22 13:30:00', '2026-05-22 17:30:00');

-- L. Phân công Ca làm việc cho nhân sự (PhanCongCa)
INSERT INTO [PhanCongCa] ([MaPhanCongCa], [MaCa], [MaNhanVien], [NgayLam], [TrangThai], [GioVaoThucTe])
VALUES 
('PCC_TEST_01', 'CA_SANG', 'NV_SHIPPER_TEST', GETDATE(), N'Đã điểm danh', '2026-05-22 07:55:00'),
('PCC_TEST_02', 'CA_SANG', 'NV_SHIPPER_02', GETDATE(), N'Đã điểm danh', '2026-05-22 07:50:00');

-- M. Tạo Đơn hàng kiểm thử mẫu đa dạng trạng thái (DonHang & ChiTietDonHang)

-- Đơn hàng 1: Chờ shipper lấy hàng (Giao Quận 1)
INSERT INTO [DonHang] (
    [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
    [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], [NgayHoanThanh], [ThoiGianTao]
)
VALUES (
    'DH_TEST_01', 'KH_TEST_01', N'Người nhận Q1', '0911222333', N'78 Lê Lợi, Bến Nghé, Quận 1, TP. HCM',
    0.25, 30000.00, N'COD', N'Chờ shipper lấy hàng', DATEADD(day, 2, GETDATE()), NULL, GETDATE()
);
INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
VALUES ('HH_TEST_01', 'DH_TEST_01', 1, N'Mới nguyên seal bọc nilong');

-- Đơn hàng 2: Chờ shipper lấy hàng (Giao Quận 3) - Sai tuyến với shipper 1 nhưng hợp shipper 2
INSERT INTO [DonHang] (
    [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
    [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], [NgayHoanThanh], [ThoiGianTao]
)
VALUES (
    'DH_TEST_02', 'KH_TEST_01', N'Người nhận Q3', '0922333444', N'456 Lê Văn Sỹ, Phường 14, Quận 3, TP. HCM',
    0.25, 35000.00, N'COD', N'Chờ shipper lấy hàng', DATEADD(day, 2, GETDATE()), NULL, GETDATE()
);
INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
VALUES ('HH_TEST_01', 'DH_TEST_02', 1, N'Mới nguyên seal');

-- Đơn hàng 3: Giao thành công (Quận 1)
INSERT INTO [DonHang] (
    [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
    [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], [NgayHoanThanh], [ThoiGianTao]
)
VALUES (
    'DH_TEST_03', 'KH_TEST_02', N'Trần Thị Hoàng Yến', '0912233445', N'15 Nguyễn Trãi, Quận 1, TP. HCM',
    1.80, 45000.00, N'COD', N'Giao hàng thành công', DATEADD(day, -1, GETDATE()), GETDATE(), DATEADD(day, -3, GETDATE())
);
INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
VALUES ('HH_TEST_02', 'DH_TEST_03', 1, N'Mới nguyên seal, nguyên thùng gỗ');

-- Đơn hàng 4: Giao thành công (Quận 3)
INSERT INTO [DonHang] (
    [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
    [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], [NgayHoanThanh], [ThoiGianTao]
)
VALUES (
    'DH_TEST_04', 'KH_TEST_03', N'Phạm Minh Hoàng', '0913344556', N'789 Điện Biên Phủ, Phường 10, Quận 3, TP. HCM',
    0.60, 30000.00, N'COD', N'Giao hàng thành công', DATEADD(day, -1, GETDATE()), GETDATE(), DATEADD(day, -2, GETDATE())
);
INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
VALUES ('HH_TEST_03', 'DH_TEST_04', 1, N'Hộp nguyên vẹn bọc bong bóng xốp');

-- Đơn hàng 5: Giao thất bại
INSERT INTO [DonHang] (
    [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
    [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], [NgayHoanThanh], [ThoiGianTao]
)
VALUES (
    'DH_TEST_05', 'KH_TEST_04', N'Lê Minh Cường', '0914455667', N'12 Mạc Đĩnh Chi, Đa Kao, Quận 1, TP. HCM',
    75.00, 250000.00, N'Chuyển khoản', N'Giao hàng thất bại', DATEADD(day, -1, GETDATE()), GETDATE(), DATEADD(day, -4, GETDATE())
);
INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
VALUES ('HH_TEST_04', 'DH_TEST_05', 1, N'Hàng đóng khung chống trầy xước');

-- Đơn hàng 6: Mới tạo (Chờ xử lý kho bãi)
INSERT INTO [DonHang] (
    [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
    [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], [NgayHoanThanh], [ThoiGianTao]
)
VALUES (
    'DH_TEST_06', 'KH_TEST_02', N'Vũ Hoàng Nam', '0915566778', N'45 Hai Bà Trưng, Đa Kao, Quận 1, TP. HCM',
    0.25, 30000.00, N'COD', N'Mới tạo', DATEADD(day, 2, GETDATE()), NULL, GETDATE()
);
INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
VALUES ('HH_TEST_01', 'DH_TEST_06', 1, N'Hộp nguyên vẹn');


-- N. Seed thông tin Nhập kho thực tế (NhapKho)
INSERT INTO [NhapKho] (
    [MaNhapKho], [MaDonHang], [MaKhoHang], [MaNhanVien], [ThoiGianNhap], 
    [ViTriLuuTru], [TrangThaiKho], [KhoiLuongThucTe], [SoLuongKienHang], [TinhTrangDonHang]
)
VALUES 
('NK_TEST_01', 'DH_TEST_01', 'K01', 'NV001', GETDATE(), N'Kệ hàng Quận 1', N'Đã nhập kho', 0.25, 1, N'Nguyên seal hộp'),
('NK_TEST_02', 'DH_TEST_02', 'K01', 'NV001', GETDATE(), N'Kệ hàng Quận 3', N'Đã nhập kho', 0.25, 1, N'Móp nhẹ góc hộp'),
('NK_TEST_03', 'DH_TEST_03', 'K01', 'NV_WAREHOUSE_01', DATEADD(day, -2, GETDATE()), N'Kệ điện tử A-12', N'Đã nhập kho', 1.85, 1, N'Nguyên seal'),
('NK_TEST_04', 'DH_TEST_04', 'K01', 'NV_WAREHOUSE_01', DATEADD(day, -1, GETDATE()), N'Kệ dễ vỡ B-04', N'Đã nhập kho', 0.60, 1, N'Hộp bọc xốp chống shock'),
('NK_TEST_05', 'DH_TEST_05', 'K01', 'NV_WAREHOUSE_01', DATEADD(day, -3, GETDATE()), N'Kệ cồng kềnh C-01', N'Đã nhập kho', 76.50, 1, N'Hơi trầy xước nhẹ');


-- O. Seed thông tin Phân công bưu tá & Hành trình thực tế (HanhTrinhDonHang)
INSERT INTO [HanhTrinhDonHang] (
    [MaHanhTrinh], [MaDonHang], [MaNhanVien], [ThoiGianTiepNhan], [ThoiGianHoanThanh], 
    [TrangThai], [ViTriHienTai], [LyDoThatBai], [HinhAnhThucTe]
)
VALUES 
('HTGH_TEST_01', 'DH_TEST_01', 'NV_SHIPPER_TEST', GETDATE(), NULL, N'Chờ shipper lấy hàng', N'Kho Quận 1', NULL, NULL),
('HTGH_TEST_02', 'DH_TEST_02', 'NV_SHIPPER_TEST', GETDATE(), NULL, N'Chờ shipper lấy hàng', N'Kho Quận 1', NULL, NULL),
('HTGH_TEST_03', 'DH_TEST_03', 'NV_SHIPPER_TEST', DATEADD(day, -1, GETDATE()), GETDATE(), N'Giao hàng thành công', N'15 Nguyễn Trãi, Quận 1, TP. HCM', NULL, N'/images/proof_DH_TEST_03.jpg'),
('HTGH_TEST_04', 'DH_TEST_04', 'NV_SHIPPER_02', DATEADD(day, -1, GETDATE()), GETDATE(), N'Giao hàng thành công', N'789 Điện Biên Phủ, Quận 3, TP. HCM', NULL, N'/images/proof_DH_TEST_04.jpg'),
('HTGH_TEST_05', 'DH_TEST_05', 'NV_SHIPPER_TEST', DATEADD(day, -1, GETDATE()), GETDATE(), N'Giao hàng thất bại', N'12 Mạc Đĩnh Chi, Quận 1, TP. HCM', N'Khách đi công tác hẹn giao lại tuần sau', N'/images/proof_fail_DH_TEST_05.jpg');


-- P. Seed thông tin Thanh toán thực tế của đơn thành công (ThanhToan)
INSERT INTO [ThanhToan] (
    [MaThanhToan], [MaDonHang], [MaShipper], [SoTienThanhToan], [PhuongThucThanhToan], 
    [ThoiGianThanhToan], [TrangThaiThanhToan]
)
VALUES 
('PAY_TEST_03', 'DH_TEST_03', 'NV_SHIPPER_TEST', 25045000.00, N'Tiền mặt (COD)', GETDATE(), N'Đã thu hộ'),
('PAY_TEST_04', 'DH_TEST_04', 'NV_SHIPPER_02', 1230000.00, N'Tiền mặt (COD)', GETDATE(), N'Đã thu hộ');


-- Q. Seed Nhật ký hoạt động mẫu (NhatKyHeThong)
INSERT INTO [NhatKyHeThong] ([MaNhatKy], [HanhDong], [DuLieuTacDong], [ThoiGian], [MaNhanVien])
VALUES 
('LOG_TEST_01', N'Khởi tạo bưu cục bãi', N'Kho Quận 1 (K01)', DATEADD(day, -5, GETDATE()), 'NV001'),
('LOG_TEST_02', N'Tạo tài khoản Shipper mới', N'Nguyễn Văn Shipper (NV_SHIPPER_TEST)', DATEADD(day, -4, GETDATE()), 'NV001'),
('LOG_TEST_03', N'Kiểm định & Nhập kho đơn hàng', N'Nhập kho đơn hàng DH_TEST_03', DATEADD(day, -2, GETDATE()), 'NV_WAREHOUSE_01'),
('LOG_TEST_04', N'Điểm danh ca làm việc', N'Check-in ca sáng Shipper 1', DATEADD(hour, -2, GETDATE()), 'NV_SHIPPER_TEST');

-- R. Insert EF Migrations History record to prevent EF from complaining about outstanding migrations
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES 
(N'20260514042848_InitialCreate', N'10.0.8'),
(N'20260520004458_VietHoaDatabase', N'10.0.8'),
(N'20260520051631_VietHoaToanBoDatabase', N'10.0.8'),
(N'20260521051534_AddTuyenGiaoAndPhanCongTuyen', N'10.0.8'),
(N'20260522133500_UpgradeDatabaseForNewERD', N'10.0.8'),
(N'20260523000000_AddWarehouseInspectionFields', N'10.0.8');

COMMIT TRANSACTION;
PRINT N'Đã chèn dữ liệu seed kiểm thử và bưu cục logistics mẫu thành công!';
GO
