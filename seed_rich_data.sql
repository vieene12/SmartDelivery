-- =========================================================================
-- SCRIPT SEED DỰ LIỆU LOGISTICS ĐẦY ĐỦ ĐỂ KIỂM THỬ TOÀN DIỆN (16 BẢNG 3NF)
-- Dành cho hệ thống cơ sở dữ liệu Microsoft SQL Server (SDMS_DB)
-- =========================================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

USE [SDMS_DB];
GO

BEGIN TRANSACTION;

-- Lấy mã mật khẩu hash từ tài khoản Admin hiện có để gán đồng nhất (Admin@123)
DECLARE @VerifiedPasswordHash nvarchar(max);
SELECT TOP 1 @VerifiedPasswordHash = [MatKhauHash] FROM [NguoiDung] WHERE [Email] = 'admin@sdms.com';

IF @VerifiedPasswordHash IS NULL
BEGIN
    SET @VerifiedPasswordHash = 'AQAAAAIAAYagAAAAEG3g1H5qR+w1cTqI4lA5gO0h/8nQ+Z1p/0gA5rB7o4W/3kF6g9z5gG9g+u0G9g==';
END

-- Lấy mã vai trò hệ thống
DECLARE @ShipperRoleId nvarchar(450);
SELECT TOP 1 @ShipperRoleId = [MaVaiTro] FROM [VaiTro] WHERE [TenVaiTro] = 'Shipper';

DECLARE @WarehouseRoleId nvarchar(450);
SELECT TOP 1 @WarehouseRoleId = [MaVaiTro] FROM [VaiTro] WHERE [TenVaiTro] = 'WarehouseStaff';


-- A. TẠO THÊM NHÂN VIÊN MẪU (Warehouse & Shipper mới)

-- 1. Thủ kho mới: Phạm Thị Thủ Kho (warehouse@sdms.com)
DECLARE @WarehouseUserId nvarchar(450) = 'warehouse-staff-id-01';
IF NOT EXISTS (SELECT 1 FROM [NguoiDung] WHERE [MaNguoiDung] = @WarehouseUserId)
BEGIN
    INSERT INTO [NguoiDung] (
        [MaNguoiDung], [TenDangNhap], [TenDangNhapChuanHoa], [Email], [EmailChuanHoa], 
        [XacNhanEmail], [MatKhauHash], [DauBanMat], [DauDongThoi], [SoDienThoai], 
        [XacNhanSoDienThoai], [KichHoatHaiLop], [ThoiGianKhoa], [ChoPhepKhoa], [SoLanDangNhapSai],
        [HoTen], [DiaChi]
    )
    VALUES (
        @WarehouseUserId, 'warehouse@sdms.com', 'WAREHOUSE@SDMS.COM', 'warehouse@sdms.com', 'WAREHOUSE@SDMS.COM', 
        1, @VerifiedPasswordHash, 
        CAST(NEWID() AS nvarchar(36)), CAST(NEWID() AS nvarchar(36)), '0933333333', 
        0, 0, NULL, 1, 0,
        N'Phạm Thị Thủ Kho', N'456 Võ Văn Tần, Quận 3, TP. HCM'
    );

    IF @WarehouseRoleId IS NOT NULL
    BEGIN
        INSERT INTO [NguoiDung_VaiTro] ([UserId], [RoleId]) VALUES (@WarehouseUserId, @WarehouseRoleId);
    END
END

IF NOT EXISTS (SELECT 1 FROM [NhanVien] WHERE [MaNhanVien] = 'NV_WAREHOUSE_01')
BEGIN
    INSERT INTO [NhanVien] (
        [MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [ChucVu], 
        [SoDienThoai], [DiaChi], [Email], [TrangThaiLamViec], [UserId]
    )
    VALUES (
        'NV_WAREHOUSE_01', N'Phạm Thị Thủ Kho', N'Nữ', '1992-08-20', 'WarehouseStaff', 
        '0933333333', N'456 Võ Văn Tần, Quận 3, TP. HCM', 'warehouse@sdms.com', N'Đang làm việc', @WarehouseUserId
    );
END


-- 2. Shipper 2 mới: Trần Văn Bưu Tá (shipper2@sdms.com)
DECLARE @Shipper2UserId nvarchar(450) = 'shipper-staff-id-02';
IF NOT EXISTS (SELECT 1 FROM [NguoiDung] WHERE [MaNguoiDung] = @Shipper2UserId)
BEGIN
    INSERT INTO [NguoiDung] (
        [MaNguoiDung], [TenDangNhap], [TenDangNhapChuanHoa], [Email], [EmailChuanHoa], 
        [XacNhanEmail], [MatKhauHash], [DauBanMat], [DauDongThoi], [SoDienThoai], 
        [XacNhanSoDienThoai], [KichHoatHaiLop], [ThoiGianKhoa], [ChoPhepKhoa], [SoLanDangNhapSai],
        [HoTen], [DiaChi]
    )
    VALUES (
        @Shipper2UserId, 'shipper2@sdms.com', 'SHIPPER2@SDMS.COM', 'shipper2@sdms.com', 'SHIPPER2@SDMS.COM', 
        1, @VerifiedPasswordHash, 
        CAST(NEWID() AS nvarchar(36)), CAST(NEWID() AS nvarchar(36)), '0922222222', 
        0, 0, NULL, 1, 0,
        N'Trần Văn Bưu Tá', N'789 Cách Mạng Tháng 8, Quận 10, TP. HCM'
    );

    IF @ShipperRoleId IS NOT NULL
    BEGIN
        INSERT INTO [NguoiDung_VaiTro] ([UserId], [RoleId]) VALUES (@Shipper2UserId, @ShipperRoleId);
    END
END

IF NOT EXISTS (SELECT 1 FROM [NhanVien] WHERE [MaNhanVien] = 'NV_SHIPPER_02')
BEGIN
    INSERT INTO [NhanVien] (
        [MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [ChucVu], 
        [SoDienThoai], [DiaChi], [Email], [TrangThaiLamViec], [UserId]
    )
    VALUES (
        'NV_SHIPPER_02', N'Trần Văn Bưu Tá', N'Nam', '1997-11-02', 'Shipper', 
        '0922222222', N'789 Cách Mạng Tháng 8, Quận 10, TP. HCM', 'shipper2@sdms.com', N'Đang làm việc', @Shipper2UserId
    );
END

-- Phân công tuyến Quận 3 cho Shipper 2
IF NOT EXISTS (SELECT 1 FROM [PhanCongTuyen] WHERE [MaPhanCongTuyen] = 'PCT_TEST_02')
BEGIN
    INSERT INTO [PhanCongTuyen] ([MaPhanCongTuyen], [MaNhanVien], [MaTuyen], [NgayBatDau], [NgayKetThuc])
    VALUES ('PCT_TEST_02', 'NV_SHIPPER_02', 'TG_Q3', GETDATE(), DATEADD(year, 1, GETDATE()));
END


-- B. TẠO THÊM KHÁCH HÀNG MẪU (KhachHang)

IF NOT EXISTS (SELECT 1 FROM [KhachHang] WHERE [MaKhachHang] = 'KH_TEST_02')
BEGIN
    INSERT INTO [KhachHang] ([MaKhachHang], [HoTen], [SoDienThoai], [DiaChi], [Email], [UserId])
    VALUES ('KH_TEST_02', N'Nguyễn Văn An', '0901234567', N'22 Hàng Tre, Hoàn Kiếm, Hà Nội', 'an.nguyen@gmail.com', NULL);
END

IF NOT EXISTS (SELECT 1 FROM [KhachHang] WHERE [MaKhachHang] = 'KH_TEST_03')
BEGIN
    INSERT INTO [KhachHang] ([MaKhachHang], [HoTen], [SoDienThoai], [DiaChi], [Email], [UserId])
    VALUES ('KH_TEST_03', N'Trần Thị Bình', '0902345678', N'120 Hai Bà Trưng, Quận 1, TP. HCM', 'binh.tran@gmail.com', NULL);
END

IF NOT EXISTS (SELECT 1 FROM [KhachHang] WHERE [MaKhachHang] = 'KH_TEST_04')
BEGIN
    INSERT INTO [KhachHang] ([MaKhachHang], [HoTen], [SoDienThoai], [DiaChi], [Email], [UserId])
    VALUES ('KH_TEST_04', N'Lê Văn Cường', '0903456789', N'34 Nguyễn Thị Minh Khai, Quận 3, TP. HCM', 'cuong.le@gmail.com', NULL);
END


-- C. ĐẢM BẢO NHÓM HÀNG VÀ HÀNG HÓA TỒN TẠI (Để tránh lỗi khóa ngoại)

IF NOT EXISTS (SELECT 1 FROM [NhomHang] WHERE [MaNhomHang] = 'DT')
BEGIN
    INSERT INTO [NhomHang] ([MaNhomHang], [TenNhomHang], [MoTa])
    VALUES ('DT', N'Điện tử', N'Điện thoại, máy tính');
END;

IF NOT EXISTS (SELECT 1 FROM [NhomHang] WHERE [MaNhomHang] = 'DV')
BEGIN
    INSERT INTO [NhomHang] ([MaNhomHang], [TenNhomHang], [MoTa])
    VALUES ('DV', N'Dễ vỡ', N'Gốm sứ, thủy tinh');
END;

IF NOT EXISTS (SELECT 1 FROM [NhomHang] WHERE [MaNhomHang] = 'TC')
BEGIN
    INSERT INTO [NhomHang] ([MaNhomHang], [TenNhomHang], [MoTa])
    VALUES ('TC', N'Cồng kềnh', N'Tủ lạnh, tủ quần áo');
END;

-- Tạo các mặt hàng mẫu
IF NOT EXISTS (SELECT 1 FROM [HangHoa] WHERE [MaHangHoa] = 'HH_TEST_02')
BEGIN
    INSERT INTO [HangHoa] ([MaHangHoa], [MaNhomHang], [TenHangHoa], [DonViTinh], [KhoiLuong], [KichThuoc], [MoTaChiTiet])
    VALUES ('HH_TEST_02', 'DT', N'Laptop Dell XPS 13', N'Cái', 1.80, '30x20x2 cm', N'Máy tính xách tay cao cấp');
END

IF NOT EXISTS (SELECT 1 FROM [HangHoa] WHERE [MaHangHoa] = 'HH_TEST_03')
BEGIN
    INSERT INTO [HangHoa] ([MaHangHoa], [MaNhomHang], [TenHangHoa], [DonViTinh], [KhoiLuong], [KichThuoc], [MoTaChiTiet])
    VALUES ('HH_TEST_03', 'DV', N'Bộ Ly Thủy Tinh (6 cái)', N'Hộp', 0.60, '25x15x10 cm', N'Ly thủy tinh dễ vỡ');
END

IF NOT EXISTS (SELECT 1 FROM [HangHoa] WHERE [MaHangHoa] = 'HH_TEST_04')
BEGIN
    INSERT INTO [HangHoa] ([MaHangHoa], [MaNhomHang], [TenHangHoa], [DonViTinh], [KhoiLuong], [KichThuoc], [MoTaChiTiet])
    VALUES ('HH_TEST_04', 'TC', N'Tủ Lạnh LG Smart Inverter', N'Cái', 75.00, '180x80x75 cm', N'Thiết bị gia dụng cồng kềnh');
END


-- D. TẠO THÊM CÁC ĐƠN HÀNG MẪU ĐA DẠNG TRẠNG THÁI

-- Đơn hàng 3: Giao hàng thành công (Quận 1)
IF NOT EXISTS (SELECT 1 FROM [DonHang] WHERE [MaDonHang] = 'DH_TEST_03')
BEGIN
    INSERT INTO [DonHang] (
        [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
        [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], 
        [NgayHoanThanh], [ThoiGianTao]
    )
    VALUES (
        'DH_TEST_03', 'KH_TEST_02', N'Trần Thị Hoàng Yến', '0912233445', N'15 Nguyễn Trãi, Quận 1, TP. HCM',
        1.80, 45000.00, N'COD', N'Giao hàng thành công', DATEADD(day, -1, GETDATE()), GETDATE(), DATEADD(day, -3, GETDATE())
    );
    
    INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
    VALUES ('HH_TEST_02', 'DH_TEST_03', 1, N'Mới nguyên seal, nguyên thùng');
END;

-- Nhập kho của Đơn hàng 3
IF NOT EXISTS (SELECT 1 FROM [NhapKho] WHERE [MaNhapKho] = 'NK_TEST_03')
BEGIN
    INSERT INTO [NhapKho] (
        [MaNhapKho], [MaDonHang], [MaKhoHang], [MaNhanVien], [ThoiGianNhap], 
        [ViTriLuuTru], [TrangThaiKho], [KhoiLuongThucTe], [SoLuongKienHang], [TinhTrangDonHang]
    )
    VALUES (
        'NK_TEST_03', 'DH_TEST_03', 'K01', 'NV_WAREHOUSE_01', DATEADD(day, -2, GETDATE()), 
        N'Kệ điện tử A-12', N'Đã nhập kho', 1.85, 1, N'Nguyên seal vỏ bọc'
    );
END;

-- Hành trình vận chuyển Đơn hàng 3
IF NOT EXISTS (SELECT 1 FROM [HanhTrinhDonHang] WHERE [MaHanhTrinh] = 'HTGH_TEST_03')
BEGIN
    INSERT INTO [HanhTrinhDonHang] (
        [MaHanhTrinh], [MaDonHang], [MaNhanVien], [ThoiGianTiepNhan], [ThoiGianHoanThanh], 
        [TrangThai], [ViTriHienTai], [LyDoThatBai], [HinhAnhThucTe]
    )
    VALUES (
        'HTGH_TEST_03', 'DH_TEST_03', 'NV_SHIPPER_TEST', DATEADD(day, -1, GETDATE()), GETDATE(), 
        N'Giao hàng thành công', N'15 Nguyễn Trãi, Quận 1, TP. HCM', NULL, N'/images/proof_DH_TEST_03.jpg'
    );
END;

-- Thanh toán COD Đơn hàng 3
IF NOT EXISTS (SELECT 1 FROM [ThanhToan] WHERE [MaThanhToan] = 'PAY_TEST_03')
BEGIN
    INSERT INTO [ThanhToan] (
        [MaThanhToan], [MaDonHang], [MaShipper], [SoTienThanhToan], [PhuongThucThanhToan], 
        [ThoiGianThanhToan], [TrangThaiThanhToan]
    )
    VALUES (
        'PAY_TEST_03', 'DH_TEST_03', 'NV_SHIPPER_TEST', 25000000.00, N'Tiền mặt (COD)', 
        GETDATE(), N'Đã thu hộ'
    );
END;


-- Đơn hàng 4: Giao hàng thành công (Quận 3) - Phụ trách bởi Shipper 2
IF NOT EXISTS (SELECT 1 FROM [DonHang] WHERE [MaDonHang] = 'DH_TEST_04')
BEGIN
    INSERT INTO [DonHang] (
        [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
        [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], 
        [NgayHoanThanh], [ThoiGianTao]
    )
    VALUES (
        'DH_TEST_04', 'KH_TEST_03', N'Phạm Minh Hoàng', '0913344556', N'789 Điện Biên Phủ, Phường 10, Quận 3, TP. HCM',
        0.60, 30000.00, N'COD', N'Giao hàng thành công', DATEADD(day, -1, GETDATE()), GETDATE(), DATEADD(day, -2, GETDATE())
    );
    
    INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
    VALUES ('HH_TEST_03', 'DH_TEST_04', 1, N'Hộp nguyên vẹn, dễ vỡ');
END;

-- Nhập kho Đơn hàng 4
IF NOT EXISTS (SELECT 1 FROM [NhapKho] WHERE [MaNhapKho] = 'NK_TEST_04')
BEGIN
    INSERT INTO [NhapKho] (
        [MaNhapKho], [MaDonHang], [MaKhoHang], [MaNhanVien], [ThoiGianNhap], 
        [ViTriLuuTru], [TrangThaiKho], [KhoiLuongThucTe], [SoLuongKienHang], [TinhTrangDonHang]
    )
    VALUES (
        'NK_TEST_04', 'DH_TEST_04', 'K01', 'NV_WAREHOUSE_01', DATEADD(day, -1, GETDATE()), 
        N'Kệ dễ vỡ B-04', N'Đã nhập kho', 0.60, 1, N'Hộp bọc xốp chống shock'
    );
END;

-- Hành trình Đơn hàng 4
IF NOT EXISTS (SELECT 1 FROM [HanhTrinhDonHang] WHERE [MaHanhTrinh] = 'HTGH_TEST_04')
BEGIN
    INSERT INTO [HanhTrinhDonHang] (
        [MaHanhTrinh], [MaDonHang], [MaNhanVien], [ThoiGianTiepNhan], [ThoiGianHoanThanh], 
        [TrangThai], [ViTriHienTai], [LyDoThatBai], [HinhAnhThucTe]
    )
    VALUES (
        'HTGH_TEST_04', 'DH_TEST_04', 'NV_SHIPPER_02', DATEADD(day, -1, GETDATE()), GETDATE(), 
        N'Giao hàng thành công', N'789 Điện Biên Phủ, Quận 3, TP. HCM', NULL, N'/images/proof_DH_TEST_04.jpg'
    );
END;

-- Thanh toán Đơn hàng 4
IF NOT EXISTS (SELECT 1 FROM [ThanhToan] WHERE [MaThanhToan] = 'PAY_TEST_04')
BEGIN
    INSERT INTO [ThanhToan] (
        [MaThanhToan], [MaDonHang], [MaShipper], [SoTienThanhToan], [PhuongThucThanhToan], 
        [ThoiGianThanhToan], [TrangThaiThanhToan]
    )
    VALUES (
        'PAY_TEST_04', 'DH_TEST_04', 'NV_SHIPPER_02', 1200000.00, N'Tiền mặt (COD)', 
        GETDATE(), N'Đã thu hộ'
    );
END;


-- Đơn hàng 5: Giao hàng thất bại (Quận 1) - Trực thuộc Shipper 1
IF NOT EXISTS (SELECT 1 FROM [DonHang] WHERE [MaDonHang] = 'DH_TEST_05')
BEGIN
    INSERT INTO [DonHang] (
        [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
        [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], 
        [NgayHoanThanh], [ThoiGianTao]
    )
    VALUES (
        'DH_TEST_05', 'KH_TEST_04', N'Lê Minh Cường', '0914455667', N'12 Mạc Đĩnh Chi, Đa Kao, Quận 1, TP. HCM',
        75.00, 250000.00, N'Chuyển khoản', N'Giao hàng thất bại', DATEADD(day, -1, GETDATE()), GETDATE(), DATEADD(day, -4, GETDATE())
    );
    
    INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
    VALUES ('HH_TEST_04', 'DH_TEST_05', 1, N'Hàng cồng kềnh, nguyên thùng gỗ');
END;

-- Nhập kho Đơn hàng 5
IF NOT EXISTS (SELECT 1 FROM [NhapKho] WHERE [MaNhapKho] = 'NK_TEST_05')
BEGIN
    INSERT INTO [NhapKho] (
        [MaNhapKho], [MaDonHang], [MaKhoHang], [MaNhanVien], [ThoiGianNhap], 
        [ViTriLuuTru], [TrangThaiKho], [KhoiLuongThucTe], [SoLuongKienHang], [TinhTrangDonHang]
    )
    VALUES (
        'NK_TEST_05', 'DH_TEST_05', 'K01', 'NV_WAREHOUSE_01', DATEADD(day, -3, GETDATE()), 
        N'Kệ cồng kềnh C-01', N'Đã nhập kho', 76.50, 1, N'Hơi trầy xước bao bì carton nhẹ'
    );
END;

-- Hành trình Đơn hàng 5
IF NOT EXISTS (SELECT 1 FROM [HanhTrinhDonHang] WHERE [MaHanhTrinh] = 'HTGH_TEST_05')
BEGIN
    INSERT INTO [HanhTrinhDonHang] (
        [MaHanhTrinh], [MaDonHang], [MaNhanVien], [ThoiGianTiepNhan], [ThoiGianHoanThanh], 
        [TrangThai], [ViTriHienTai], [LyDoThatBai], [HinhAnhThucTe]
    )
    VALUES (
        'HTGH_TEST_05', 'DH_TEST_05', 'NV_SHIPPER_TEST', DATEADD(day, -1, GETDATE()), GETDATE(), 
        N'Giao hàng thất bại', N'12 Mạc Đĩnh Chi, Quận 1, TP. HCM', N'Khách đi công tác hẹn giao lại tuần sau', N'/images/proof_fail_DH_TEST_05.jpg'
    );
END;


-- Đơn hàng 6: Mới tạo (Chờ thủ kho xử lý nhập)
IF NOT EXISTS (SELECT 1 FROM [DonHang] WHERE [MaDonHang] = 'DH_TEST_06')
BEGIN
    INSERT INTO [DonHang] (
        [MaDonHang], [MaKhachHang], [TenNguoiNhan], [SoDienThoaiNguoiNhan], [DiaChiNguoiNhan], 
        [TongKhoiLuong], [PhiGiaoHang], [HinhThucThanhToan], [TrangThaiDonHang], [NgayGiaoDuKien], 
        [NgayHoanThanh], [ThoiGianTao]
    )
    VALUES (
        'DH_TEST_06', 'KH_TEST_02', N'Vũ Hoàng Nam', '0915566778', N'45 Hai Bà Trưng, Đa Kao, Quận 1, TP. HCM',
        0.25, 30000.00, N'COD', N'Mới tạo', DATEADD(day, 2, GETDATE()), NULL, GETDATE()
    );
    
    INSERT INTO [ChiTietDonHang] ([MaHangHoa], [MaDonHang], [SoLuong], [TinhTrangHangHoa])
    VALUES ('HH_TEST_01', 'DH_TEST_06', 1, N'Hộp nguyên vẹn');
END;


-- E. TẠO NHẬT KÝ HỆ THỐNG MẪU (NhatKyHeThong)

IF NOT EXISTS (SELECT 1 FROM [NhatKyHeThong] WHERE [MaNhatKy] = 'LOG_TEST_01')
BEGIN
    INSERT INTO [NhatKyHeThong] ([MaNhatKy], [HanhDong], [DuLieuTacDong], [ThoiGian], [MaNhanVien])
    VALUES ('LOG_TEST_01', N'Khởi tạo bưu cục bãi', N'Kho Quận 1 (K01)', DATEADD(day, -5, GETDATE()), 'NV001');
END;

IF NOT EXISTS (SELECT 1 FROM [NhatKyHeThong] WHERE [MaNhatKy] = 'LOG_TEST_02')
BEGIN
    INSERT INTO [NhatKyHeThong] ([MaNhatKy], [HanhDong], [DuLieuTacDong], [ThoiGian], [MaNhanVien])
    VALUES ('LOG_TEST_02', N'Tạo tài khoản Shipper mới', N'Nguyễn Văn Shipper (NV_SHIPPER_TEST)', DATEADD(day, -4, GETDATE()), 'NV001');
END;

IF NOT EXISTS (SELECT 1 FROM [NhatKyHeThong] WHERE [MaNhatKy] = 'LOG_TEST_03')
BEGIN
    INSERT INTO [NhatKyHeThong] ([MaNhatKy], [HanhDong], [DuLieuTacDong], [ThoiGian], [MaNhanVien])
    VALUES ('LOG_TEST_03', N'Kiểm định & Nhập kho đơn hàng', N'Nhập kho đơn hàng DH_TEST_03', DATEADD(day, -2, GETDATE()), 'NV_WAREHOUSE_01');
END;

IF NOT EXISTS (SELECT 1 FROM [NhatKyHeThong] WHERE [MaNhatKy] = 'LOG_TEST_04')
BEGIN
    INSERT INTO [NhatKyHeThong] ([MaNhatKy], [HanhDong], [DuLieuTacDong], [ThoiGian], [MaNhanVien])
    VALUES ('LOG_TEST_04', N'Điểm danh ca làm việc', N'Check-in ca sáng Shipper 1', DATEADD(hour, -2, GETDATE()), 'NV_SHIPPER_TEST');
END;

COMMIT TRANSACTION;
PRINT N'Đã chèn dữ liệu bưu gửi phong phú thành công!';
GO
