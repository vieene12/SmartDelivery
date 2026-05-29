-- ============================================================
-- SDMS_DB - SEED DATA BỔ SUNG v3.0 (Đúng schema)
-- ============================================================
USE [SDMS_DB];
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ============================
-- BƯỚC 1: BỔ SUNG KHO HÀNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM KhoHang WHERE MaKhoHang = N'K03')
    INSERT INTO KhoHang (MaKhoHang, TenKho, DiaChiKho, DienTichKho, SucChuaKho, TrangThai)
    VALUES (N'K03', N'Kho Quận 7 - Phú Mỹ Hưng', N'18 Nguyễn Lương Bằng, Tân Phú, Quận 7, TP. HCM', 850.0, 4200, N'Hoạt động');

IF NOT EXISTS (SELECT 1 FROM KhoHang WHERE MaKhoHang = N'K04')
    INSERT INTO KhoHang (MaKhoHang, TenKho, DiaChiKho, DienTichKho, SucChuaKho, TrangThai)
    VALUES (N'K04', N'Kho Bình Dương - Thuận An', N'KCN Việt Hương, Thuận An, Bình Dương', 2500.0, 12000, N'Hoạt động');

IF NOT EXISTS (SELECT 1 FROM KhoHang WHERE MaKhoHang = N'K05')
    INSERT INTO KhoHang (MaKhoHang, TenKho, DiaChiKho, DienTichKho, SucChuaKho, TrangThai)
    VALUES (N'K05', N'Kho Hà Nội - Long Biên', N'KCN Đài Tư, Phường Đài Tư, Long Biên, Hà Nội', 1800.0, 8500, N'Hoạt động');

IF NOT EXISTS (SELECT 1 FROM KhoHang WHERE MaKhoHang = N'K06')
    INSERT INTO KhoHang (MaKhoHang, TenKho, DiaChiKho, DienTichKho, SucChuaKho, TrangThai)
    VALUES (N'K06', N'Kho Đà Nẵng - Hải Châu', N'15 Lê Duẩn, Thạch Thang, Hải Châu, Đà Nẵng', 600.0, 3000, N'Bảo trì');

-- ============================
-- BƯỚC 2: BỔ SUNG NHÓM HÀNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhomHang = N'TP')
    INSERT INTO NhomHang (MaNhomHang, TenNhomHang, MoTa)
    VALUES (N'TP', N'Thực phẩm', N'Hàng thực phẩm khô, đóng gói, không yêu cầu làm lạnh');

IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhomHang = N'MY')
    INSERT INTO NhomHang (MaNhomHang, TenNhomHang, MoTa)
    VALUES (N'MY', N'Mỹ phẩm & Chăm sóc cá nhân', N'Son môi, kem dưỡng, nước hoa, sữa tắm và các sản phẩm làm đẹp');

IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhomHang = N'QA')
    INSERT INTO NhomHang (MaNhomHang, TenNhomHang, MoTa)
    VALUES (N'QA', N'Quần áo & Thời trang', N'Hàng may mặc, giày dép, phụ kiện thời trang');

IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhomHang = N'GS')
    INSERT INTO NhomHang (MaNhomHang, TenNhomHang, MoTa)
    VALUES (N'GS', N'Gia dụng & Nội thất', N'Đồ dùng gia đình, nội thất, dụng cụ bếp');

IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhomHang = N'YT')
    INSERT INTO NhomHang (MaNhomHang, TenNhomHang, MoTa)
    VALUES (N'YT', N'Y tế & Dược phẩm', N'Thiết bị y tế, thuốc, thực phẩm chức năng - cần xử lý đặc biệt');

-- ============================
-- BƯỚC 3: BỔ SUNG TUYẾN ĐƯỜNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM TuyenGiao WHERE MaTuyen = N'TG_Q7')
    INSERT INTO TuyenGiao (MaTuyen, TenTuyen, KhuVuc, MoTa)
    VALUES (N'TG_Q7', N'Quận 7 - Phú Mỹ Hưng', N'TP. Hồ Chí Minh', N'Khu đô thị Phú Mỹ Hưng, đường Nguyễn Lương Bằng, Nguyễn Văn Linh');

IF NOT EXISTS (SELECT 1 FROM TuyenGiao WHERE MaTuyen = N'TG_Q5')
    INSERT INTO TuyenGiao (MaTuyen, TenTuyen, KhuVuc, MoTa)
    VALUES (N'TG_Q5', N'Quận 5 - Chợ Lớn', N'TP. Hồ Chí Minh', N'Khu vực Chợ Lớn, Trần Hưng Đạo, Nguyễn Trãi');

IF NOT EXISTS (SELECT 1 FROM TuyenGiao WHERE MaTuyen = N'TG_BD')
    INSERT INTO TuyenGiao (MaTuyen, TenTuyen, KhuVuc, MoTa)
    VALUES (N'TG_BD', N'Bình Dương - Thuận An', N'Bình Dương', N'KCN Việt Hương, các phường Thuận An, Dĩ An');

IF NOT EXISTS (SELECT 1 FROM TuyenGiao WHERE MaTuyen = N'TG_DN')
    INSERT INTO TuyenGiao (MaTuyen, TenTuyen, KhuVuc, MoTa)
    VALUES (N'TG_DN', N'Đà Nẵng - Hải Châu', N'Đà Nẵng', N'Trung tâm thành phố Đà Nẵng, quận Hải Châu và Thanh Khê');

IF NOT EXISTS (SELECT 1 FROM TuyenGiao WHERE MaTuyen = N'TG_HN_HK')
    INSERT INTO TuyenGiao (MaTuyen, TenTuyen, KhuVuc, MoTa)
    VALUES (N'TG_HN_HK', N'Hà Nội - Hoàn Kiếm', N'Hà Nội', N'Quận Hoàn Kiếm, phố cổ Hà Nội, khu vực trung tâm');

-- ============================
-- BƯỚC 4: BỔ SUNG CA LÀM VIỆC
-- ============================
IF NOT EXISTS (SELECT 1 FROM CaLamViec WHERE MaCa = N'CA_TOI')
    INSERT INTO CaLamViec (MaCa, TenCa, GioBatDau, GioKetThuc)
    VALUES (N'CA_TOI', N'Ca Tối', '2026-01-01 17:30:00', '2026-01-01 22:00:00');

IF NOT EXISTS (SELECT 1 FROM CaLamViec WHERE MaCa = N'CA_DEM')
    INSERT INTO CaLamViec (MaCa, TenCa, GioBatDau, GioKetThuc)
    VALUES (N'CA_DEM', N'Ca Đêm', '2026-01-01 22:00:00', '2026-01-02 06:00:00');

-- ============================
-- BƯỚC 5: BỔ SUNG KHÁCH HÀNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhachHang = N'KH005')
    INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi)
    VALUES (N'KH005', N'Phạm Thanh Hoa', N'0904567890', N'hoa.pham@gmail.com', N'55 Đinh Tiên Hoàng, Đa Kao, Quận 1, TP. HCM');

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhachHang = N'KH006')
    INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi)
    VALUES (N'KH006', N'Nguyễn Minh Tuấn', N'0905678901', N'tuan.nguyen@company.vn', N'102 Hai Bà Trưng, Phường 6, Quận 3, TP. HCM');

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhachHang = N'KH007')
    INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi)
    VALUES (N'KH007', N'Trần Thị Thu Hiền', N'0906789012', N'thuhien.tran@outlook.com', N'34 Lê Lợi, Hải Châu, Đà Nẵng');

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhachHang = N'KH008')
    INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi)
    VALUES (N'KH008', N'Lê Hoàng Phúc', N'0907890123', N'phuc.le@yahoo.com', N'88 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội');

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhachHang = N'KH009')
    INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi)
    VALUES (N'KH009', N'Võ Thị Mỹ Linh', N'0908901234', N'mylinh.vo@gmail.com', N'12 Nguyễn Huệ, Bến Nghé, Quận 1, TP. HCM');

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhachHang = N'KH010')
    INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi)
    VALUES (N'KH010', N'Đặng Quốc Hùng', N'0909012345', N'hung.dang@business.vn', N'45 Cách Mạng Tháng 8, Phường 6, Quận 3, TP. HCM');

-- ============================
-- BƯỚC 6: BỔ SUNG HÀNG HÓA
-- ============================
IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH005')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH005', N'Túi xách da thời trang', 0.45, N'30x20x10 cm', N'QA');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH006')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH006', N'Bộ mỹ phẩm dưỡng da SK-II', 0.80, N'25x20x15 cm', N'MY');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH007')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH007', N'Thực phẩm khô Hải sản sấy', 2.50, N'40x30x20 cm', N'TP');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH008')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH008', N'Đèn LED thông minh Philips Hue', 1.20, N'20x15x10 cm', N'GS');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH009')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH009', N'Giày Sneaker Nike Air Force 1', 1.10, N'35x20x15 cm', N'QA');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH010')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH010', N'Nồi chiên không khí Philips', 5.50, N'38x34x31 cm', N'GS');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH011')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH011', N'Vitamin C 1000mg Nature Made (300 viên)', 0.65, N'15x10x10 cm', N'YT');

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE MaHangHoa = N'HH012')
    INSERT INTO HangHoa (MaHangHoa, TenHangHoa, KhoiLuong, KichThuoc, MaNhomHang)
    VALUES (N'HH012', N'Đồng hồ Garmin Forerunner 255', 0.30, N'15x12x8 cm', N'DT');

-- ============================
-- BƯỚC 7: BỔ SUNG ĐƠN HÀNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026001')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026001',N'KH_TEST_01',N'Nguyễn Bảo Châu',N'0911111111',N'15 Bùi Thị Xuân, Bến Thành, Quận 1, TP. HCM',0.45,29000,N'COD',N'Mới tạo','2026-05-01 08:30:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026002')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026002',N'KH_TEST_02',N'Trần Hữu Nghĩa',N'0922222222',N'88 Lý Tự Trọng, Bến Nghé, Quận 1, TP. HCM',1.80,35000,N'Đã thanh toán online',N'Mới tạo','2026-05-02 09:15:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026003')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026003',N'KH005',N'Lê Thị Diễm Trinh',N'0933333333',N'22 Võ Văn Tần, Phường 5, Quận 3, TP. HCM',5.50,55000,N'COD',N'Mới tạo','2026-05-02 10:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026004')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026004',N'KH_TEST_03',N'Phạm Quốc Bảo',N'0944444444',N'45 Phan Văn Trị, Phường 10, Bình Thạnh, TP. HCM',0.80,30000,N'COD',N'Chờ shipper lấy hàng','2026-05-05 07:45:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026005')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026005',N'KH006',N'Nguyễn Thị Thu Thảo',N'0955555555',N'110 Đinh Bộ Lĩnh, Phường 26, Bình Thạnh, TP. HCM',2.50,42000,N'Đã thanh toán online',N'Chờ shipper lấy hàng','2026-05-06 08:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026006')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026006',N'KH007',N'Đỗ Minh Khôi',N'0966666666',N'78 Trần Phú, Hải Châu, Đà Nẵng',1.10,48000,N'COD',N'Đã nhập kho','2026-05-07 06:30:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026007')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026007',N'KH008',N'Lý Thị Bảo Ngân',N'0977777777',N'23 Đinh Tiên Hoàng, Hoàn Kiếm, Hà Nội',0.30,150000,N'COD',N'Đã nhập kho','2026-05-08 09:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026008')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026008',N'KH009',N'Võ Hoàng Long',N'0988888888',N'56 Lê Thị Hồng Gấm, Quận 1, TP. HCM',0.65,29000,N'Đã thanh toán online',N'Đã nhập kho','2026-05-09 10:15:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026009')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026009',N'KH010',N'Hồ Sỹ Hùng',N'0999999999',N'12 Phan Chu Trinh, Hoàn Kiếm, Hà Nội',1.20,130000,N'COD',N'Đang giao hàng','2026-05-10 07:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026010')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026010',N'KH_TEST_04',N'Đinh Thùy Dương',N'0900111222',N'99 Nguyễn Cư Trinh, Quận 1, TP. HCM',0.80,35000,N'COD',N'Đang giao hàng','2026-05-11 08:30:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026011')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026011',N'KH005',N'Bùi Thị Lan Anh',N'0900222333',N'67 Lê Hồng Phong, Quận 10, TP. HCM',5.50,45000,N'Đã thanh toán online',N'Đang giao hàng','2026-05-12 09:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026012')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026012',N'KH006',N'Chu Thị Ngọc Bích',N'0900333444',N'34 Đinh Lễ, Hoàn Kiếm, Hà Nội',0.45,120000,N'COD',N'Giao hàng thành công','2026-05-13 07:30:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026013')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026013',N'KH007',N'Phan Trung Hiếu',N'0900444555',N'190 Nguyễn Tri Phương, Quận 5, TP. HCM',1.10,35000,N'COD',N'Giao hàng thành công','2026-05-14 08:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026014')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026014',N'KH008',N'Dương Thị Huyền My',N'0900555666',N'28 Trần Quý Cáp, Bình Thạnh, TP. HCM',2.50,38000,N'Đã thanh toán online',N'Giao hàng thành công','2026-05-14 09:30:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026015')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026015',N'KH009',N'Lưu Bá Hải',N'0900666777',N'55 Lý Thường Kiệt, Hoàn Kiếm, Hà Nội',0.30,100000,N'COD',N'Giao hàng thành công','2026-05-15 07:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026016')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026016',N'KH010',N'Mạc Thị Phương Thảo',N'0900777888',N'33 Lê Lai, Bến Thành, Quận 1, TP. HCM',1.20,32000,N'COD',N'Giao hàng thành công','2026-05-16 10:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026017')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026017',N'KH_TEST_01',N'Tống Ngọc Hân',N'0900888999',N'78 Nguyễn Bỉnh Khiêm, Đa Kao, Quận 1, TP. HCM',0.65,29000,N'Đã thanh toán online',N'Giao hàng thành công','2026-05-17 08:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026018')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026018',N'KH_TEST_02',N'Ngô Thị Kim Cúc',N'0900900011',N'12 Tôn Đức Thắng, Bến Nghé, Quận 1, TP. HCM',5.50,50000,N'COD',N'Giao hàng thành công','2026-05-18 11:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026019')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026019',N'KH_TEST_03',N'Cao Đình Trọng',N'0900011022',N'45 Bà Huyện Thanh Quan, Quận 3, TP. HCM',0.80,35000,N'COD',N'Giao hàng thất bại','2026-05-19 09:00:00');

IF NOT EXISTS (SELECT 1 FROM DonHang WHERE MaDonHang = N'DH2026020')
    INSERT INTO DonHang (MaDonHang,MaKhachHang,TenNguoiNhan,SoDienThoaiNguoiNhan,DiaChiNguoiNhan,TongKhoiLuong,PhiGiaoHang,HinhThucThanhToan,TrangThaiDonHang,ThoiGianTao)
    VALUES (N'DH2026020',N'KH_TEST_04',N'Trịnh Thị Bảo Châu',N'0900122033',N'200 Lý Chính Thắng, Phường 9, Quận 3, TP. HCM',1.10,32000,N'COD',N'Giao hàng thất bại','2026-05-19 14:00:00');

-- ============================
-- BƯỚC 8: CHI TIẾT ĐƠN HÀNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026001')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH006',N'DH2026001',1,N'Nguyên vẹn');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026002')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH005',N'DH2026002',1,N'Nguyên vẹn');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026003')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH010',N'DH2026003',1,N'Nguyên vẹn');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026006')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH009',N'DH2026006',1,N'Nguyên vẹn');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026007')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH012',N'DH2026007',1,N'Nguyên vẹn - hàng giá trị cao');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026008')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH011',N'DH2026008',2,N'Nguyên vẹn');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026009')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH008',N'DH2026009',3,N'Nguyên vẹn');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026012')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH006',N'DH2026012',1,N'Nguyên vẹn - đã giao');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026013')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH009',N'DH2026013',1,N'Nguyên vẹn - đã giao');
IF NOT EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang=N'DH2026015')
    INSERT INTO ChiTietDonHang (MaHangHoa,MaDonHang,SoLuong,TinhTrangHangHoa)
    VALUES (N'HH012',N'DH2026015',1,N'Nguyên vẹn - đã giao');

-- ============================
-- BƯỚC 9: NHẬP KHO
-- ============================
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026006')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026006',N'DH2026006',N'K06',N'NV_WAREHOUSE_01','2026-05-08 07:00:00',N'A3-R5-S2',1.10,1,N'Tốt - Nguyên đai nguyên kiện',N'Đang lưu kho');
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026007')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026007',N'DH2026007',N'K05',N'NV_WAREHOUSE_01','2026-05-09 06:30:00',N'B1-R2-S1',0.30,1,N'Tốt - Hàng giá trị cao - đã niêm phong',N'Đang lưu kho');
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026008')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026008',N'DH2026008',N'K01',N'NV_WAREHOUSE_01','2026-05-10 09:00:00',N'C2-R1-S4',0.65,2,N'Tốt',N'Đang lưu kho');
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026009')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026009',N'DH2026009',N'K05',N'NV_WAREHOUSE_01','2026-05-10 10:00:00',N'A1-R1-S1',1.20,1,N'Tốt - Đã bàn giao shipper',N'Đã xuất kho');
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026010')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026010',N'DH2026010',N'K01',N'NV_WAREHOUSE_01','2026-05-11 07:00:00',N'C1-R3-S2',0.80,1,N'Tốt - Đã bàn giao shipper',N'Đã xuất kho');
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026012')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026012',N'DH2026012',N'K05',N'NV_WAREHOUSE_01','2026-05-13 06:00:00',N'B2-R2-S3',0.45,1,N'Tốt - Đã giao thành công',N'Đã xuất kho');
IF NOT EXISTS (SELECT 1 FROM NhapKho WHERE MaNhapKho=N'NK2026015')
    INSERT INTO NhapKho (MaNhapKho,MaDonHang,MaKhoHang,MaNhanVien,ThoiGianNhap,ViTriLuuTru,KhoiLuongThucTe,SoLuongKienHang,TinhTrangDonHang,TrangThaiKho)
    VALUES (N'NK2026015',N'DH2026015',N'K05',N'NV_WAREHOUSE_01','2026-05-15 06:00:00',N'A2-R1-S2',0.30,1,N'Tốt - Đã giao thành công',N'Đã xuất kho');

-- ============================
-- BƯỚC 10: HÀNH TRÌNH GIAO HÀNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026009')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,TrangThai,ViTriHienTai)
    VALUES (N'HT2026009',N'DH2026009',N'NV_SHIPPER_02','2026-05-10 11:00:00',N'Đang giao hàng',N'Đang trên đường - Hoàn Kiếm, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026010')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,TrangThai,ViTriHienTai)
    VALUES (N'HT2026010',N'DH2026010',N'NV_SHIPPER_TEST','2026-05-11 08:00:00',N'Đang giao hàng',N'Đang trên đường - Quận 1, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026011')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,TrangThai,ViTriHienTai)
    VALUES (N'HT2026011',N'DH2026011',N'NV_SHIPPER_TEST','2026-05-12 09:30:00',N'Đang giao hàng',N'Đang trên đường - Quận 10, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026012')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026012',N'DH2026012',N'NV_SHIPPER_02','2026-05-13 07:00:00','2026-05-13 10:30:00',N'Giao hàng thành công',N'34 Đinh Lễ, Hoàn Kiếm, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026013')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026013',N'DH2026013',N'NV_SHIPPER_TEST','2026-05-14 08:30:00','2026-05-14 11:00:00',N'Giao hàng thành công',N'190 Nguyễn Tri Phương, Quận 5, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026014')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026014',N'DH2026014',N'NV_SHIPPER_TEST','2026-05-14 10:00:00','2026-05-14 14:00:00',N'Giao hàng thành công',N'28 Trần Quý Cáp, Bình Thạnh, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026015')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026015',N'DH2026015',N'NV_SHIPPER_02','2026-05-15 07:30:00','2026-05-15 10:00:00',N'Giao hàng thành công',N'55 Lý Thường Kiệt, Hoàn Kiếm, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026016')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026016',N'DH2026016',N'NV_SHIPPER_TEST','2026-05-16 10:30:00','2026-05-16 13:00:00',N'Giao hàng thành công',N'33 Lê Lai, Bến Thành, Quận 1, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026017')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026017',N'DH2026017',N'NV_SHIPPER_TEST','2026-05-17 08:30:00','2026-05-17 11:30:00',N'Giao hàng thành công',N'78 Nguyễn Bỉnh Khiêm, Quận 1, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026018')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai)
    VALUES (N'HT2026018',N'DH2026018',N'NV_SHIPPER_02','2026-05-18 11:30:00','2026-05-18 14:30:00',N'Giao hàng thành công',N'12 Tôn Đức Thắng, Quận 1, TP. HCM');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026019')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai,LyDoThatBai)
    VALUES (N'HT2026019',N'DH2026019',N'NV_SHIPPER_TEST','2026-05-19 09:30:00','2026-05-19 11:00:00',N'Giao hàng thất bại',N'45 Bà Huyện Thanh Quan, Quận 3, TP. HCM',N'Gọi điện 3 lần không có người nghe. Không có người nhận tại địa chỉ.');
IF NOT EXISTS (SELECT 1 FROM HanhTrinhDonHang WHERE MaDonHang=N'DH2026020')
    INSERT INTO HanhTrinhDonHang (MaHanhTrinh,MaDonHang,MaNhanVien,ThoiGianTiepNhan,ThoiGianHoanThanh,TrangThai,ViTriHienTai,LyDoThatBai)
    VALUES (N'HT2026020',N'DH2026020',N'NV_SHIPPER_02','2026-05-19 14:30:00','2026-05-19 16:00:00',N'Giao hàng thất bại',N'200 Lý Chính Thắng, Quận 3, TP. HCM',N'Địa chỉ không chính xác - tòa nhà không tồn tại tại số này.');

-- ============================
-- BƯỚC 11: THANH TOÁN
-- ============================
IF NOT EXISTS (SELECT 1 FROM ThanhToan WHERE MaThanhToan=N'TT2026012')
    INSERT INTO ThanhToan (MaThanhToan,MaDonHang,MaShipper,SoTienThanhToan,PhuongThucThanhToan,ThoiGianThanhToan,TrangThaiThanhToan)
    VALUES (N'TT2026012',N'DH2026012',N'NV_SHIPPER_02',120000,N'Tiền mặt COD','2026-05-13 10:30:00',N'Đã thanh toán');
IF NOT EXISTS (SELECT 1 FROM ThanhToan WHERE MaThanhToan=N'TT2026013')
    INSERT INTO ThanhToan (MaThanhToan,MaDonHang,MaShipper,SoTienThanhToan,PhuongThucThanhToan,ThoiGianThanhToan,TrangThaiThanhToan)
    VALUES (N'TT2026013',N'DH2026013',N'NV_SHIPPER_TEST',35000,N'Tiền mặt COD','2026-05-14 11:00:00',N'Đã thanh toán');
IF NOT EXISTS (SELECT 1 FROM ThanhToan WHERE MaThanhToan=N'TT2026015')
    INSERT INTO ThanhToan (MaThanhToan,MaDonHang,MaShipper,SoTienThanhToan,PhuongThucThanhToan,ThoiGianThanhToan,TrangThaiThanhToan)
    VALUES (N'TT2026015',N'DH2026015',N'NV_SHIPPER_02',100000,N'Tiền mặt COD','2026-05-15 10:00:00',N'Đã thanh toán');
IF NOT EXISTS (SELECT 1 FROM ThanhToan WHERE MaThanhToan=N'TT2026016')
    INSERT INTO ThanhToan (MaThanhToan,MaDonHang,MaShipper,SoTienThanhToan,PhuongThucThanhToan,ThoiGianThanhToan,TrangThaiThanhToan)
    VALUES (N'TT2026016',N'DH2026016',N'NV_SHIPPER_TEST',32000,N'Tiền mặt COD','2026-05-16 13:00:00',N'Đã thanh toán');
IF NOT EXISTS (SELECT 1 FROM ThanhToan WHERE MaThanhToan=N'TT2026018')
    INSERT INTO ThanhToan (MaThanhToan,MaDonHang,MaShipper,SoTienThanhToan,PhuongThucThanhToan,ThoiGianThanhToan,TrangThaiThanhToan)
    VALUES (N'TT2026018',N'DH2026018',N'NV_SHIPPER_02',50000,N'Tiền mặt COD','2026-05-18 14:30:00',N'Đã thanh toán');

-- ============================
-- BƯỚC 12: NHẬT KÝ HỆ THỐNG
-- ============================
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026001')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026001',N'NV001',N'Thêm kho hàng',N'Thêm mới: Kho Quận 7 - Phú Mỹ Hưng (K03)','2026-05-01 09:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026002')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026002',N'NV001',N'Thêm tuyến giao',N'Thêm mới tuyến: Bình Dương - Thuận An (TG_BD)','2026-05-02 08:30:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026003')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026003',N'NV_WAREHOUSE_01',N'Nhập kho',N'Nhập kho thành công 8 kiện hàng - Kho K01','2026-05-05 08:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026004')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026004',N'NV_SHIPPER_TEST',N'Giao hàng thành công',N'Hoàn thành giao DH2026013 - Quận 5, TP. HCM','2026-05-14 11:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026005')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026005',N'NV_SHIPPER_02',N'Giao hàng thành công',N'Hoàn thành giao DH2026015 - Hoàn Kiếm, Hà Nội','2026-05-15 10:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026006')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026006',N'NV001',N'Thêm nhóm hàng',N'Thêm nhóm: Y tế & Dược phẩm (YT)','2026-05-16 08:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026007')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026007',N'NV_SHIPPER_TEST',N'Giao hàng thất bại',N'Thất bại DH2026019 - Khách không nghe máy - Quận 3','2026-05-19 11:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026008')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026008',N'NV001',N'Cập nhật kho',N'Cập nhật trạng thái: Kho Đà Nẵng (K06) -> Bảo trì','2026-05-20 09:00:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026009')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026009',N'NV_WAREHOUSE_01',N'Xuất kho',N'Bàn giao 5 kiện hàng cho shipper Nguyễn Văn Shipper - Ca sáng','2026-05-22 08:30:00');
IF NOT EXISTS (SELECT 1 FROM NhatKyHeThong WHERE MaNhatKy=N'LOG2026010')
    INSERT INTO NhatKyHeThong (MaNhatKy,MaNhanVien,HanhDong,DuLieuTacDong,ThoiGian)
    VALUES (N'LOG2026010',N'NV001',N'Thêm nhân viên',N'Đăng ký nhân viên mới: Phạm Thị Thủ Kho (NV_WAREHOUSE_01)','2026-05-28 10:00:00');

-- ============================
-- THỐNG KÊ KẾT QUẢ
-- ============================
SELECT 'KhoHang'           AS [Bảng], COUNT(*) AS [Số bản ghi] FROM KhoHang
UNION ALL SELECT 'NhomHang',          COUNT(*) FROM NhomHang
UNION ALL SELECT 'TuyenGiao',         COUNT(*) FROM TuyenGiao
UNION ALL SELECT 'CaLamViec',         COUNT(*) FROM CaLamViec
UNION ALL SELECT 'KhachHang',         COUNT(*) FROM KhachHang
UNION ALL SELECT 'HangHoa',           COUNT(*) FROM HangHoa
UNION ALL SELECT 'DonHang',           COUNT(*) FROM DonHang
UNION ALL SELECT 'ChiTietDonHang',    COUNT(*) FROM ChiTietDonHang
UNION ALL SELECT 'NhapKho',           COUNT(*) FROM NhapKho
UNION ALL SELECT 'HanhTrinhDonHang',  COUNT(*) FROM HanhTrinhDonHang
UNION ALL SELECT 'ThanhToan',         COUNT(*) FROM ThanhToan
UNION ALL SELECT 'NhatKyHeThong',     COUNT(*) FROM NhatKyHeThong;
GO
