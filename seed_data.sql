-- =========================================================================
-- SCRIPT SEED DỮ LIỆU MẪU ĐỂ KIỂM THỬ HỆ THỐNG MỚI (16 BẢNG CHUẨN 3NF)
-- Dành cho hệ thống cơ sở dữ liệu Microsoft SQL Server (SDMS_DB)
-- =========================================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

USE [SDMS_DB];
GO

BEGIN TRANSACTION;

-- 1. THÊM TUYẾN GIAO MẪU (TuyenGiao)
IF NOT EXISTS (SELECT 1 FROM [TuyenGiao] WHERE [MaTuyen] = 'TG_Q1')
BEGIN
    INSERT INTO [TuyenGiao] ([MaTuyen], [TenTuyen], [KhuVuc], [MoTa])
    VALUES ('TG_Q1', N'Quận 1', N'TP. Hồ Chí Minh', N'Tuyến giao khu vực Quận 1');
END;

IF NOT EXISTS (SELECT 1 FROM [TuyenGiao] WHERE [MaTuyen] = 'TG_Q3')
BEGIN
    INSERT INTO [TuyenGiao] ([MaTuyen], [TenTuyen], [KhuVuc], [MoTa])
    VALUES ('TG_Q3', N'Quận 3', N'TP. Hồ Chí Minh', N'Tuyến giao khu vực Quận 3');
END;


-- 2. TẠO TÀI KHOẢN SHIPPER TRONG BẢNG [NguoiDung] (AspNetUsers)
-- Mật khẩu mặc định: Shipper@123
DECLARE @ShipperUserId nvarchar(450) = 'shipper-test-id';
IF NOT EXISTS (SELECT 1 FROM [NguoiDung] WHERE [MaNguoiDung] = @ShipperUserId)
BEGIN
    INSERT INTO [NguoiDung] (
        [MaNguoiDung], [TenDangNhap], [TenDangNhapChuanHoa], [Email], [EmailChuanHoa], 
        [XacNhanEmail], [MatKhauHash], [DauBanMat], [DauDongThoi], [SoDienThoai], 
        [XacNhanSoDienThoai], [KichHoatHaiLop], [ThoiGianKhoa], [ChoPhepKhoa], [SoLanDangNhapSai],
        [HoTen], [DiaChi]
    )
    VALUES (
        @ShipperUserId, 'shipper@sdms.com', 'SHIPPER@SDMS.COM', 'shipper@sdms.com', 'SHIPPER@SDMS.COM', 
        1, 'AQAAAAIAAYagAAAAEG3g1H5qR+w1cTqI4lA5gO0h/8nQ+Z1p/0gA5rB7o4W/3kF6g9z5gG9g+u0G9g==', 
        CAST(NEWID() AS nvarchar(36)), CAST(NEWID() AS nvarchar(36)), '0987654321', 
        0, 0, NULL, 1, 0,
        N'Nguyễn Văn Shipper', N'123 Nguyễn Huệ, Quận 1, TP. HCM'
    );
END;


-- 3. GÁN VAI TRÒ "Shipper" CHO TÀI KHOẢN TRÊN
DECLARE @ShipperRoleId nvarchar(450);
SELECT TOP 1 @ShipperRoleId = [MaVaiTro] FROM [VaiTro] WHERE [TenVaiTro] = 'Shipper';

-- Nếu chưa có vai trò Shipper, tự tạo mới
IF @ShipperRoleId IS NULL
BEGIN
    SET @ShipperRoleId = 'shipper-role-id';
    INSERT INTO [VaiTro] ([MaVaiTro], [TenVaiTro], [TenVaiTroChuanHoa], [DauDongThoi])
    VALUES (@ShipperRoleId, 'Shipper', 'SHIPPER', CAST(NEWID() AS nvarchar(36)));
END;

-- Map Người Dùng với Vai Trò
IF NOT EXISTS (SELECT 1 FROM [NguoiDung_VaiTro] WHERE [UserId] = @ShipperUserId AND [RoleId] = @ShipperRoleId)
BEGIN
    INSERT INTO [NguoiDung_VaiTro] ([UserId], [RoleId])
    VALUES (@ShipperUserId, @ShipperRoleId);
END;


-- 4. TẠO HỒ SƠ NHÂN VIÊN SHIPPER TRONG BẢNG [NhanVien]
IF NOT EXISTS (SELECT 1 FROM [NhanVien] WHERE [MaNhanVien] = 'NV_SHIPPER_TEST')
BEGIN
    INSERT INTO [NhanVien] (
        [MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [ChucVu], 
        [SoDienThoai], [DiaChi], [Email], [TrangThaiLamViec], [UserId]
    )
    VALUES (
        'NV_SHIPPER_TEST', N'Nguyễn Văn Shipper', N'Nam', '1995-05-15', 'Shipper', 
        '0987654321', N'123 Nguyễn Huệ, Quận 1, TP. HCM', 'shipper@sdms.com', N'Đang làm việc', @ShipperUserId
    );
END;


-- 5. PHÂN CÔNG TUYẾN GIAO CHO SHIPPER (PhanCongTuyen)
-- Phân công Shipper này đi tuyến Quận 1, thời hạn hoạt động từ hôm nay đến 1 năm sau
IF NOT EXISTS (SELECT 1 FROM [PhanCongTuyen] WHERE [MaPhanCongTuyen] = 'PCT_TEST_01')
BEGIN
    INSERT INTO [PhanCongTuyen] ([MaPhanCongTuyen], [MaNhanVien], [MaTuyen], [NgayBatDau], [NgayKetThuc])
    VALUES ('PCT_TEST_01', 'NV_SHIPPER_TEST', 'TG_Q1', GETDATE(), DATEADD(year, 1, GETDATE()));
END;


-- 6. TẠO DỮ LIỆU KHÁCH HÀNG MẪU (KhachHang)
IF NOT EXISTS (SELECT 1 FROM [KhachHang] WHERE [MaKhachHang] = 'KH_TEST_01')
BEGIN
    INSERT INTO [KhachHang] ([MaKhachHang], [HoTen], [SoDienThoai], [DiaChi], [Email], [UserId])
    VALUES ('KH_TEST_01', N'Lê Thị Khách Hàng', '0909090909', N'Quận 1, TP. HCM', 'customer@sdms.com', NULL);
END;


-- 7. ĐẢM BẢO KHO HÀNG VÀ NHÓM HÀNG TỒN TẠI (Để tránh lỗi khóa ngoại)
IF NOT EXISTS (SELECT 1 FROM [KhoHang] WHERE [MaKhoHang] = 'K01')
BEGIN
    INSERT INTO [KhoHang] ([MaKhoHang], [TenKho], [DiaChiKho], [DienTichKho], [SucChuaKho])
    VALUES ('K01', N'Kho Quận 1', N'Quận 1, TP. HCM', 1000.00, 5000);
END;

IF NOT EXISTS (SELECT 1 FROM [NhomHang] WHERE [MaNhomHang] = 'DT')
BEGIN
    INSERT INTO [NhomHang] ([MaNhomHang], [TenNhomHang], [MoTa])
    VALUES ('DT', N'Điện tử', N'Điện thoại, máy tính');
END;

-- Tạo hàng hóa mẫu
IF NOT EXISTS (SELECT 1 FROM [HangHoa] WHERE [MaHangHoa] = 'HH_TEST_01')
BEGIN
    INSERT INTO [HangHoa] ([MaHangHoa], [MaNhomHang], [TenHangHoa], [DonViTinh], [KhoiLuong], [KichThuoc], [MoTaChiTiet])
    VALUES ('HH_TEST_01', 'DT', N'iPhone 15 Pro Max', N'Cái', 0.25, '15x8x1 cm', N'Điện thoại cao cấp');
END;


-- 8. TẠO 2 ĐƠN HÀNG MẪU ĐỂ THỰC HIỆN KIỂM THỬ (DonHang)
-- Đơn hàng 1: Sẽ được đặt tại Kệ hàng thuộc Tuyến Quận 1 (HỢP LỆ với tuyến của shipper)
IF NOT EXISTS (SELECT 1 FROM [DonHang] WHERE [MaDonHang] = 'DH_TEST_01')
BEGIN
    INSERT INTO [DonHang] (
        [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
        [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], 
        [NgayHoanThanh], [ThoiGianTao]
    )
    VALUES (
        'DH_TEST_01', 'KH_TEST_01', N'Người nhận Q1', '0911222333', N'78 Lê Lợi, Bến Nghé, Quận 1, TP. HCM',
        0.25, 30000.00, N'COD', N'Chờ shipper lấy hàng', DATEADD(day, 2, GETDATE()), NULL, GETDATE()
    );
    
    INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
    VALUES ('HH_TEST_01', 'DH_TEST_01', 1, N'Mới nguyên seal');
END;

-- Đơn hàng 2: Sẽ được đặt tại Kệ hàng thuộc Tuyến Quận 3 (SAI TUYẾN với shipper)
IF NOT EXISTS (SELECT 1 FROM [DonHang] WHERE [MaDonHang] = 'DH_TEST_02')
BEGIN
    INSERT INTO [DonHang] (
        [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
        [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], 
        [NgayHoanThanh], [ThoiGianTao]
    )
    VALUES (
        'DH_TEST_02', 'KH_TEST_01', N'Người nhận Q3', '0922333444', N'456 Lê Văn Sỹ, Phường 14, Quận 3, TP. HCM',
        0.25, 35000.00, N'COD', N'Chờ shipper lấy hàng', DATEADD(day, 2, GETDATE()), NULL, GETDATE()
    );

    INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
    VALUES ('HH_TEST_01', 'DH_TEST_02', 1, N'Mới nguyên seal');
END;


-- 9. TẠO VỊ TRÍ LƯU KHO MẪU TRONG BẢNG [NhapKho] (Thay thế [QuanLyKho])
-- Đơn hàng 1 đặt tại "Kệ hàng Quận 1" (Trùng khớp với từ khóa "Quận 1" trong tên Tuyến giao của Shipper)
IF NOT EXISTS (SELECT 1 FROM [NhapKho] WHERE [MaNhapKho] = 'NK_TEST_01')
BEGIN
    INSERT INTO [NhapKho] (
        [MaNhapKho], [MaDonHang], [MaKhoHang], [MaNhanVien], [ThoiGianNhap], 
        [ViTriLuuTru], [TrangThaiKho], [KhoiLuongThucTe], [SoLuongKienHang], [TinhTrangDonHang]
    )
    VALUES (
        'NK_TEST_01', 'DH_TEST_01', 'K01', 'NV001', GETDATE(), 
        N'Kệ hàng Quận 1', N'Đã nhập kho', 0.25, 1, N'Mới nguyên seal'
    );
END;

-- Đơn hàng 2 đặt tại "Kệ hàng Quận 3" (Không chứa từ khóa "Quận 1" nên sẽ báo lỗi khi Shipper quét)
IF NOT EXISTS (SELECT 1 FROM [NhapKho] WHERE [MaNhapKho] = 'NK_TEST_02')
BEGIN
    INSERT INTO [NhapKho] (
        [MaNhapKho], [MaDonHang], [MaKhoHang], [MaNhanVien], [ThoiGianNhap], 
        [ViTriLuuTru], [TrangThaiKho], [KhoiLuongThucTe], [SoLuongKienHang], [TinhTrangDonHang]
    )
    VALUES (
        'NK_TEST_02', 'DH_TEST_02', 'K01', 'NV001', GETDATE(), 
        N'Kệ hàng Quận 3', N'Đã nhập kho', 0.25, 1, N'Hơi bóp méo nhẹ'
    );
END;


-- 10. PHÂN CÔNG GIAO HÀNG CHO SHIPPER TRONG BẢNG [HanhTrinhDonHang] (Thay thế [PhanCongGiaoHang])
-- Giao cả 2 đơn hàng này cho Shipper NV_SHIPPER_TEST dưới trạng thái "Chờ shipper lấy hàng" để họ có thể nhìn thấy và thực hiện quét
IF NOT EXISTS (SELECT 1 FROM [HanhTrinhDonHang] WHERE [MaHanhTrinh] = 'HTGH_TEST_01')
BEGIN
    INSERT INTO [HanhTrinhDonHang] (
        [MaHanhTrinh], [MaDonHang], [MaNhanVien], [ThoiGianTiepNhan], [ThoiGianHoanThanh], 
        [TrangThai], [ViTriHienTai], [LyDoThatBai], [HinhAnhThucTe]
    )
    VALUES (
        'HTGH_TEST_01', 'DH_TEST_01', 'NV_SHIPPER_TEST', GETDATE(), NULL, 
        N'Chờ shipper lấy hàng', N'Kho Quận 1', NULL, NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM [HanhTrinhDonHang] WHERE [MaHanhTrinh] = 'HTGH_TEST_02')
BEGIN
    INSERT INTO [HanhTrinhDonHang] (
        [MaHanhTrinh], [MaDonHang], [MaNhanVien], [ThoiGianTiepNhan], [ThoiGianHoanThanh], 
        [TrangThai], [ViTriHienTai], [LyDoThatBai], [HinhAnhThucTe]
    )
    VALUES (
        'HTGH_TEST_02', 'DH_TEST_02', 'NV_SHIPPER_TEST', GETDATE(), NULL, 
        N'Chờ shipper lấy hàng', N'Kho Quận 1', NULL, NULL
    );
END;


-- 11. THÊM CA LÀM VIỆC MẪU (CaLamViec)
IF NOT EXISTS (SELECT 1 FROM [CaLamViec] WHERE [MaCa] = 'CA_SANG')
BEGIN
    INSERT INTO [CaLamViec] ([MaCa], [TenCa], [GioBatDau], [GioKetThuc])
    VALUES ('CA_SANG', N'Ca Sáng', '2026-05-22 08:00:00', '2026-05-22 12:00:00');
END;

IF NOT EXISTS (SELECT 1 FROM [CaLamViec] WHERE [MaCa] = 'CA_CHIEU')
BEGIN
    INSERT INTO [CaLamViec] ([MaCa], [TenCa], [GioBatDau], [GioKetThuc])
    VALUES ('CA_CHIEU', N'Ca Chiều', '2026-05-22 13:30:00', '2026-05-22 17:30:00');
END;


-- 12. THÊM PHÂN CÔNG CA LÀM VIỆC (PhanCongCa)
IF NOT EXISTS (SELECT 1 FROM [PhanCongCa] WHERE [MaPhanCongCa] = 'PCC_TEST_01')
BEGIN
    INSERT INTO [PhanCongCa] ([MaPhanCongCa], [MaCa], [MaNhanVien], [NgayLam], [TrangThai], [GioVaoThucTe])
    VALUES ('PCC_TEST_01', 'CA_SANG', 'NV_SHIPPER_TEST', GETDATE(), N'Đã điểm danh', '2026-05-22 07:55:00');
END;

COMMIT TRANSACTION;
PRINT N'Đã chèn dữ liệu mẫu 16 bảng thành công!';
GO
