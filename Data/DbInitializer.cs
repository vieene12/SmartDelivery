using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SDMS.Data;
using SDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDMS.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // 1. Seed Roles
        string[] roles = { "Admin", "WarehouseStaff", "Shipper", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Seed default Identity users
        var defaultUsers = new[]
        {
            new { Email = "admin@sdms.com", Role = "Admin", FullName = "System Administrator", Id = "admin-test-id", MaNhanVien = "NV001", ChucVu = "Admin" },
            new { Email = "warehouse@sdms.com", Role = "WarehouseStaff", FullName = "Phạm Thị Thủ Kho", Id = "warehouse-staff-id-01", MaNhanVien = "NV002", ChucVu = "WarehouseStaff" },
            new { Email = "shipper@sdms.com", Role = "Shipper", FullName = "Nguyễn Văn Shipper", Id = "shipper-test-id", MaNhanVien = "NV_SHIPPER_TEST", ChucVu = "Shipper" },
            new { Email = "shipper2@sdms.com", Role = "Shipper", FullName = "Trần Văn Bưu Tá", Id = "shipper-staff-id-02", MaNhanVien = "NV004", ChucVu = "Shipper" },
            new { Email = "customer@sdms.com", Role = "Customer", FullName = "Lê Thị Khách Hàng", Id = "customer-test-id", MaNhanVien = (string)null, ChucVu = (string)null },
            new { Email = "longnv@gmail.com", Role = "Customer", FullName = "Nguyễn Văn Long", Id = "customer-long-id", MaNhanVien = (string)null, ChucVu = (string)null },
            new { Email = "hoangpm@gmail.com", Role = "Customer", FullName = "Phạm Minh Hoàng", Id = "customer-hoang-id", MaNhanVien = (string)null, ChucVu = (string)null },
            new { Email = "sondt@gmail.com", Role = "Customer", FullName = "Đỗ Thanh Sơn", Id = "customer-son-id", MaNhanVien = (string)null, ChucVu = (string)null },
            new { Email = "ngochb@gmail.com", Role = "Customer", FullName = "Hoàng Bích Ngọc", Id = "customer-ngoc-id", MaNhanVien = (string)null, ChucVu = (string)null }
        };

        foreach (var u in defaultUsers)
        {
            var user = await userManager.FindByEmailAsync(u.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = u.Id,
                    UserName = u.Email,
                    Email = u.Email,
                    FullName = u.FullName,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, u.Role);
            }
            else
            {
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "123456");
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                if (!await userManager.IsInRoleAsync(user, u.Role))
                {
                    await userManager.AddToRoleAsync(user, u.Role);
                }
            }

            // Seed/sync associated NhanVien staff records
            if (u.MaNhanVien != null && !context.NhanViens.Any(n => n.MaNhanVien == u.MaNhanVien))
            {
                var nhanVien = new NhanVien
                {
                    MaNhanVien = u.MaNhanVien,
                    HoTen = u.FullName,
                    ChucVu = u.ChucVu,
                    UserId = user.Id,
                    TrangThaiLamViec = "Đang làm việc",
                    NgaySinh = new DateTime(1990, 1, 1)
                };
                context.NhanViens.Add(nhanVien);
                await context.SaveChangesAsync();
            }
        }

        // ===========================================================================================
        // DATABASE RESET & BULK RICH LOGISTICS DATA SEEDING
        // ===========================================================================================
        // To ensure the user gets a fully populated, extremely consistent, production-grade 
        // demo database of 32+ orders, we clean out any existing demo data first to prevent PK conflicts.
        
        // Remove existing transaction/log records
        context.ThanhToans.RemoveRange(context.ThanhToans);
        context.HanhTrinhDonHangs.RemoveRange(context.HanhTrinhDonHangs);
        context.NhapKhos.RemoveRange(context.NhapKhos);
        context.ChiTietDonHangs.RemoveRange(context.ChiTietDonHangs);
        context.DonHangs.RemoveRange(context.DonHangs);
        
        // Remove operational setup records
        context.KhachHangs.RemoveRange(context.KhachHangs);
        context.PhanCongCas.RemoveRange(context.PhanCongCas);
        context.PhanCongTuyens.RemoveRange(context.PhanCongTuyens);
        context.TuyenGiaos.RemoveRange(context.TuyenGiaos);
        context.CaLamViecs.RemoveRange(context.CaLamViecs);
        context.HangHoas.RemoveRange(context.HangHoas);
        context.NhomHangs.RemoveRange(context.NhomHangs);
        context.KhoHangs.RemoveRange(context.KhoHangs);
        context.NhatKyHeThongs.RemoveRange(context.NhatKyHeThongs);
        
        await context.SaveChangesAsync();

        // 3. Seed Category Groups (NhomHang)
        context.NhomHangs.AddRange(
            new NhomHang { MaNhomHang = "DT", TenNhomHang = "Hàng Điện Tử", MoTa = "Điện thoại, Laptop, Máy tính bảng, Phụ kiện công nghệ cao" },
            new NhomHang { MaNhomHang = "DV", TenNhomHang = "Hàng Dễ Vỡ", MoTa = "Gốm sứ trang trí, Đồ thủy tinh pha lê nghệ thuật" },
            new NhomHang { MaNhomHang = "TC", TenNhomHang = "Hàng Cồng Kềnh", MoTa = "Thiết bị điện gia dụng kích thước lớn như Tủ lạnh, Máy giặt" }
        );
        await context.SaveChangesAsync();

        // 4. Seed Hub Warehouses (KhoHang)
        context.KhoHangs.AddRange(
            new KhoHang { MaKhoHang = "K01", TenKho = "Kho Trung Chuyển Miền Nam", DiaChiKho = "55 Lê Lợi, Bến Nghé, Quận 1, TP. HCM", DienTichKho = 5000, SucChuaKho = 10000, TrangThai = "Hoạt động" },
            new KhoHang { MaKhoHang = "K02", TenKho = "Kho Trung Chuyển Miền Bắc", DiaChiKho = "202 Cầu Giấy, Quan Hoa, Cầu Giấy, Hà Nội", DienTichKho = 4500, SucChuaKho = 9000, TrangThai = "Hoạt động" }
        );
        await context.SaveChangesAsync();

        // 5. Seed Customer profiles (KhachHang)
        context.KhachHangs.AddRange(
            new KhachHang { MaKhachHang = "KH_TEST_01", HoTen = "Lê Thị Khách Hàng", SoDienThoai = "0912345678", DiaChi = "456 Nguyễn Huệ, Quận 1, TP. HCM", Email = "customer@sdms.com", UserId = "customer-test-id" },
            new KhachHang { MaKhachHang = "KH_002", HoTen = "Nguyễn Văn Long", SoDienThoai = "0938472910", DiaChi = "18 Bis Tôn Đức Thắng, Quận 1, TP. HCM", Email = "longnv@gmail.com", UserId = "customer-long-id" },
            new KhachHang { MaKhachHang = "KH_003", HoTen = "Phạm Minh Hoàng", SoDienThoai = "0908123456", DiaChi = "45 Nguyễn Trãi, Quận 5, TP. HCM", Email = "hoangpm@gmail.com", UserId = "customer-hoang-id" },
            new KhachHang { MaKhachHang = "KH_004", HoTen = "Đỗ Thanh Sơn", SoDienThoai = "0989098909", DiaChi = "102 Trần Hưng Đạo, Quận 1, TP. HCM", Email = "sondt@gmail.com", UserId = "customer-son-id" },
            new KhachHang { MaKhachHang = "KH_005", HoTen = "Hoàng Bích Ngọc", SoDienThoai = "0977112233", DiaChi = "12 Xuân Thủy, Quận Cầu Giấy, Hà Nội", Email = "ngochb@gmail.com", UserId = "customer-ngoc-id" }
        );
        await context.SaveChangesAsync();

        // 6. Seed Shift Setup (CaLamViec)
        context.CaLamViecs.AddRange(
            new CaLamViec { MaCa = "CA_SANG", TenCa = "Ca Sáng (08:00 - 12:00)", GioBatDau = new DateTime(2026, 1, 1, 8, 0, 0), GioKetThuc = new DateTime(2026, 1, 1, 12, 0, 0) },
            new CaLamViec { MaCa = "CA_CHIEU", TenCa = "Ca Chiều (13:00 - 17:00)", GioBatDau = new DateTime(2026, 1, 1, 13, 0, 0), GioKetThuc = new DateTime(2026, 1, 1, 17, 0, 0) },
            new CaLamViec { MaCa = "CA_TOI", TenCa = "Ca Tối (18:00 - 22:00)", GioBatDau = new DateTime(2026, 1, 1, 18, 0, 0), GioKetThuc = new DateTime(2026, 1, 1, 22, 0, 0) }
        );
        await context.SaveChangesAsync();

        // 7. Seed Operational Shifts (PhanCongCa)
        var today = DateTime.Today;
        context.PhanCongCas.AddRange(
            new PhanCongCa { MaPhanCongCa = "PC_CA_01", MaCa = "CA_SANG", MaNhanVien = "NV002", NgayLam = today.AddDays(-2), TrangThai = "Đã điểm danh", GioVaoThucTe = today.AddDays(-2).AddHours(7).AddMinutes(55) },
            new PhanCongCa { MaPhanCongCa = "PC_CA_02", MaCa = "CA_CHIEU", MaNhanVien = "NV002", NgayLam = today.AddDays(-1), TrangThai = "Đã điểm danh", GioVaoThucTe = today.AddDays(-1).AddHours(12).AddMinutes(50) },
            new PhanCongCa { MaPhanCongCa = "PC_CA_03", MaCa = "CA_SANG", MaNhanVien = "NV_SHIPPER_TEST", NgayLam = today.AddDays(-2), TrangThai = "Đã điểm danh", GioVaoThucTe = today.AddDays(-2).AddHours(8).AddMinutes(2) },
            new PhanCongCa { MaPhanCongCa = "PC_CA_04", MaCa = "CA_CHIEU", MaNhanVien = "NV_SHIPPER_TEST", NgayLam = today.AddDays(-1), TrangThai = "Đã điểm danh", GioVaoThucTe = today.AddDays(-1).AddHours(13).AddMinutes(0) },
            new PhanCongCa { MaPhanCongCa = "PC_CA_05", MaCa = "CA_SANG", MaNhanVien = "NV004", NgayLam = today.AddDays(-2), TrangThai = "Đã điểm danh", GioVaoThucTe = today.AddDays(-2).AddHours(7).AddMinutes(58) },
            new PhanCongCa { MaPhanCongCa = "PC_CA_06", MaCa = "CA_CHIEU", MaNhanVien = "NV004", NgayLam = today.AddDays(-1), TrangThai = "Đã điểm danh", GioVaoThucTe = today.AddDays(-1).AddHours(12).AddMinutes(55) }
        );
        await context.SaveChangesAsync();

        // 8. Seed Delivery Routes (TuyenGiao)
        context.TuyenGiaos.AddRange(
            new TuyenGiao { MaTuyen = "TG01", TenTuyen = "Tuyến Giao Nội Thành Q1 TP.HCM", KhuVuc = "Quận 1, TP. HCM", MoTa = "Các tuyến đường chính tại Quận 1, bao gồm Lê Lợi, Nguyễn Huệ, Đồng Khởi" },
            new TuyenGiao { MaTuyen = "TG02", TenTuyen = "Tuyến Giao Cầu Giấy Hà Nội", KhuVuc = "Cầu Giấy, Hà Nội", MoTa = "Tuyến giao chính tại khu vực Cầu Giấy, Hà Nội" },
            new TuyenGiao { MaTuyen = "TG03", TenTuyen = "Tuyến Liên Tỉnh Bắc Nam", KhuVuc = "Hà Nội - TP. HCM", MoTa = "Tuyến vận chuyển đường dài dọc Quốc lộ 1A kết nối hai kho tổng" }
        );
        await context.SaveChangesAsync();

        // 9. Seed Route Allocations (PhanCongTuyen)
        context.PhanCongTuyens.AddRange(
            new PhanCongTuyen { MaPhanCongTuyen = "PC_TG_01", MaNhanVien = "NV_SHIPPER_TEST", MaTuyen = "TG01", NgayBatDau = DateTime.Today.AddDays(-30) },
            new PhanCongTuyen { MaPhanCongTuyen = "PC_TG_02", MaNhanVien = "NV004", MaTuyen = "TG02", NgayBatDau = DateTime.Today.AddDays(-30) }
        );
        await context.SaveChangesAsync();

        // 10. Seed Product Catalog (HangHoa)
        context.HangHoas.AddRange(
            new HangHoa { MaHangHoa = "HH_DT_01", MaNhomHang = "DT", TenHangHoa = "iPhone 15 Pro Max", DonViTinh = "Cái", KhoiLuong = 0.22m, KichThuoc = "159.9 x 76.7 x 8.3 mm", MoTaChiTiet = "Điện thoại Apple iPhone 15 Pro Max 256GB chính hãng" },
            new HangHoa { MaHangHoa = "HH_DT_02", MaNhomHang = "DT", TenHangHoa = "Laptop Dell XPS 15", DonViTinh = "Cái", KhoiLuong = 1.96m, KichThuoc = "344.7 x 230.1 x 18 mm", MoTaChiTiet = "Laptop cao cấp Dell XPS 15 9530 Core i7 16GB 512GB" },
            new HangHoa { MaHangHoa = "HH_DT_03", MaNhomHang = "DT", TenHangHoa = "iPad Pro 11 inch", DonViTinh = "Cái", KhoiLuong = 0.46m, KichThuoc = "247.6 x 178.5 x 5.9 mm", MoTaChiTiet = "Máy tính bảng Apple iPad Pro M2 11-inch 128GB" },
            new HangHoa { MaHangHoa = "HH_DT_04", MaNhomHang = "DT", TenHangHoa = "Tai nghe AirPods Pro", DonViTinh = "Cái", KhoiLuong = 0.05m, KichThuoc = "45.2 x 60.6 x 21.7 mm", MoTaChiTiet = "Tai nghe Bluetooth Apple AirPods Pro 2" },
            new HangHoa { MaHangHoa = "HH_DV_01", MaNhomHang = "DV", TenHangHoa = "Bình hoa thủy tinh", DonViTinh = "Cái", KhoiLuong = 0.80m, KichThuoc = "Đường kính 12cm, cao 25cm", MoTaChiTiet = "Bình hoa thủy tinh trang trí cao cấp dễ vỡ" },
            new HangHoa { MaHangHoa = "HH_DV_02", MaNhomHang = "DV", TenHangHoa = "Bộ ấm chén Bát Tràng", DonViTinh = "Bộ", KhoiLuong = 1.20m, KichThuoc = "Hộp 30x20x15cm", MoTaChiTiet = "Bộ ấm trà gốm sứ Bát Tràng men rạn cao cấp" },
            new HangHoa { MaHangHoa = "HH_DV_03", MaNhomHang = "DV", TenHangHoa = "Tranh kính phong cảnh", DonViTinh = "Bức", KhoiLuong = 3.50m, KichThuoc = "50 x 70 cm", MoTaChiTiet = "Tranh kính nghệ thuật trang trí dễ vỡ" },
            new HangHoa { MaHangHoa = "HH_DV_04", MaNhomHang = "DV", TenHangHoa = "Bộ ly thủy tinh pha lê", DonViTinh = "Bộ", KhoiLuong = 1.80m, KichThuoc = "Hộp 6 ly", MoTaChiTiet = "Bộ ly uống rượu vang thủy tinh pha lê cao cấp" },
            new HangHoa { MaHangHoa = "HH_TC_01", MaNhomHang = "TC", TenHangHoa = "Tủ lạnh Panasonic", DonViTinh = "Chiếc", KhoiLuong = 65.00m, KichThuoc = "180 x 68 x 70 cm", MoTaChiTiet = "Tủ lạnh Panasonic Inverter 400 lít" },
            new HangHoa { MaHangHoa = "HH_TC_02", MaNhomHang = "TC", TenHangHoa = "Máy giặt LG", DonViTinh = "Chiếc", KhoiLuong = 45.00m, KichThuoc = "85 x 60 x 56 cm", MoTaChiTiet = "Máy giặt lồng ngang LG AI DD 9kg" },
            new HangHoa { MaHangHoa = "HH_TC_03", MaNhomHang = "TC", TenHangHoa = "Lò vi sóng Sharp", DonViTinh = "Chiếc", KhoiLuong = 15.00m, KichThuoc = "51 x 38 x 30 cm", MoTaChiTiet = "Lò vi sóng có nướng Sharp 23 lít" },
            new HangHoa { MaHangHoa = "HH_TC_04", MaNhomHang = "TC", TenHangHoa = "Quạt điều hòa Sunhouse", DonViTinh = "Chiếc", KhoiLuong = 22.50m, KichThuoc = "95 x 50 x 40 cm", MoTaChiTiet = "Quạt điều hòa Sunhouse 50 lít công suất lớn" }
        );
        await context.SaveChangesAsync();

        // 11. Seed 32+ Rich Orders (DonHang)
        var baseDate = DateTime.Today;
        context.DonHangs.AddRange(
            // --- Completed Orders (16) ---
            new DonHang { MaDonHang = "DH10001", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Nguyễn Văn A", SoDienThoaiNguoiNhan = "0901234567", DiaChiNguoiNhan = "123 Lê Lợi, Quận 1, TP. HCM", TongKhoiLuong = 0.22m, PhiGiaoHang = 21100, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-28), NgayGiaoDuKien = baseDate.AddDays(-26), NgayHoanThanh = baseDate.AddDays(-26) },
            new DonHang { MaDonHang = "DH10002", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Trần Thị B", SoDienThoaiNguoiNhan = "0918765432", DiaChiNguoiNhan = "456 Trần Hưng Đạo, Quận 1, TP. HCM", TongKhoiLuong = 1.96m, PhiGiaoHang = 29800, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-25), NgayGiaoDuKien = baseDate.AddDays(-23), NgayHoanThanh = baseDate.AddDays(-23) },
            new DonHang { MaDonHang = "DH10003", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Phạm Văn C", SoDienThoaiNguoiNhan = "0934567890", DiaChiNguoiNhan = "789 Nguyễn Đình Chiểu, Quận 3, TP. HCM", TongKhoiLuong = 0.80m, PhiGiaoHang = 24000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-20), NgayGiaoDuKien = baseDate.AddDays(-18), NgayHoanThanh = baseDate.AddDays(-18) },
            new DonHang { MaDonHang = "DH10004", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Lê Văn D", SoDienThoaiNguoiNhan = "0945678901", DiaChiNguoiNhan = "101 Hàm Nghi, Quận 1, TP. HCM", TongKhoiLuong = 1.20m, PhiGiaoHang = 26000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-15), NgayGiaoDuKien = baseDate.AddDays(-13), NgayHoanThanh = baseDate.AddDays(-13) },
            new DonHang { MaDonHang = "DH10013", MaKhachHang = "KH_002", TenNguoiNhan = "Nguyễn Hữu Chiến", SoDienThoaiNguoiNhan = "0912123456", DiaChiNguoiNhan = "55 Đồng Khởi, Quận 1, TP. HCM", TongKhoiLuong = 0.10m, PhiGiaoHang = 22000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-22), NgayGiaoDuKien = baseDate.AddDays(-20), NgayHoanThanh = baseDate.AddDays(-20) },
            new DonHang { MaDonHang = "DH10014", MaKhachHang = "KH_003", TenNguoiNhan = "Trần Bích Thủy", SoDienThoaiNguoiNhan = "0934112233", DiaChiNguoiNhan = "120 Nguyễn Trãi, Quận 5, TP. HCM", TongKhoiLuong = 1.80m, PhiGiaoHang = 28000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-19), NgayGiaoDuKien = baseDate.AddDays(-17), NgayHoanThanh = baseDate.AddDays(-17) },
            new DonHang { MaDonHang = "DH10015", MaKhachHang = "KH_004", TenNguoiNhan = "Đặng Hồng Nam", SoDienThoaiNguoiNhan = "0908765432", DiaChiNguoiNhan = "74 Pasteur, Quận 1, TP. HCM", TongKhoiLuong = 3.50m, PhiGiaoHang = 36000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-17), NgayGiaoDuKien = baseDate.AddDays(-15), NgayHoanThanh = baseDate.AddDays(-15) },
            new DonHang { MaDonHang = "DH10016", MaKhachHang = "KH_005", TenNguoiNhan = "Lê Thị Lan", SoDienThoaiNguoiNhan = "0989012903", DiaChiNguoiNhan = "45 Nguyễn Phong Sắc, Cầu Giấy, Hà Nội", TongKhoiLuong = 15.00m, PhiGiaoHang = 145000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-14), NgayGiaoDuKien = baseDate.AddDays(-11), NgayHoanThanh = baseDate.AddDays(-11) },
            new DonHang { MaDonHang = "DH10017", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Phạm Đình Trọng", SoDienThoaiNguoiNhan = "0966554433", DiaChiNguoiNhan = "18 Điện Biên Phủ, Quận 1, TP. HCM", TongKhoiLuong = 22.50m, PhiGiaoHang = 185000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-12), NgayGiaoDuKien = baseDate.AddDays(-10), NgayHoanThanh = baseDate.AddDays(-10) },
            new DonHang { MaDonHang = "DH10018", MaKhachHang = "KH_002", TenNguoiNhan = "Vũ Thị Hương", SoDienThoaiNguoiNhan = "0977889900", DiaChiNguoiNhan = "250 Lý Tự Trọng, Quận 1, TP. HCM", TongKhoiLuong = 0.05m, PhiGiaoHang = 20000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-9), NgayGiaoDuKien = baseDate.AddDays(-7), NgayHoanThanh = baseDate.AddDays(-7) },
            new DonHang { MaDonHang = "DH10019", MaKhachHang = "KH_003", TenNguoiNhan = "Hoàng Kim Liên", SoDienThoaiNguoiNhan = "0905123987", DiaChiNguoiNhan = "88 Lê Hồng Phong, Quận 5, TP. HCM", TongKhoiLuong = 1.20m, PhiGiaoHang = 24500, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-8), NgayGiaoDuKien = baseDate.AddDays(-6), NgayHoanThanh = baseDate.AddDays(-6) },
            new DonHang { MaDonHang = "DH10020", MaKhachHang = "KH_004", TenNguoiNhan = "Ngô Quốc Việt", SoDienThoaiNguoiNhan = "0932123789", DiaChiNguoiNhan = "190 Hàm Nghi, Quận 1, TP. HCM", TongKhoiLuong = 0.22m, PhiGiaoHang = 21000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-7), NgayGiaoDuKien = baseDate.AddDays(-5), NgayHoanThanh = baseDate.AddDays(-5) },
            new DonHang { MaDonHang = "DH10021", MaKhachHang = "KH_005", TenNguoiNhan = "Bùi Đình Tú", SoDienThoaiNguoiNhan = "0981999888", DiaChiNguoiNhan = "20 Xuân Thủy, Cầu Giấy, Hà Nội", TongKhoiLuong = 1.96m, PhiGiaoHang = 28000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-6), NgayGiaoDuKien = baseDate.AddDays(-4), NgayHoanThanh = baseDate.AddDays(-4) },
            new DonHang { MaDonHang = "DH10022", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Đỗ Trọng Nghĩa", SoDienThoaiNguoiNhan = "0944111222", DiaChiNguoiNhan = "300 Tôn Đức Thắng, Quận 1, TP. HCM", TongKhoiLuong = 0.46m, PhiGiaoHang = 21500, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-5), NgayGiaoDuKien = baseDate.AddDays(-3), NgayHoanThanh = baseDate.AddDays(-3) },
            new DonHang { MaDonHang = "DH10023", MaKhachHang = "KH_002", TenNguoiNhan = "Trịnh Minh Đức", SoDienThoaiNguoiNhan = "0909112233", DiaChiNguoiNhan = "415 Đề Thám, Quận 1, TP. HCM", TongKhoiLuong = 0.80m, PhiGiaoHang = 23000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-4), NgayGiaoDuKien = baseDate.AddDays(-2), NgayHoanThanh = baseDate.AddDays(-2) },
            new DonHang { MaDonHang = "DH10024", MaKhachHang = "KH_003", TenNguoiNhan = "Lê Hồng Sơn", SoDienThoaiNguoiNhan = "0933445566", DiaChiNguoiNhan = "99 Nguyễn Trãi, Quận 5, TP. HCM", TongKhoiLuong = 1.20m, PhiGiaoHang = 25000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã hoàn thành", ThoiGianTao = baseDate.AddDays(-3), NgayGiaoDuKien = baseDate.AddDays(-1), NgayHoanThanh = baseDate.AddDays(-1) },

            // --- Delivering Orders (6) ---
            new DonHang { MaDonHang = "DH10005", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Hoàng Văn E", SoDienThoaiNguoiNhan = "0956789012", DiaChiNguoiNhan = "202 Cầu Giấy, Quận Cầu Giấy, Hà Nội", TongKhoiLuong = 65.00m, PhiGiaoHang = 345000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đang giao", ThoiGianTao = baseDate.AddDays(-10), NgayGiaoDuKien = baseDate.AddDays(-7) },
            new DonHang { MaDonHang = "DH10006", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Vũ Thị F", SoDienThoaiNguoiNhan = "0967890123", DiaChiNguoiNhan = "303 Xuân Thủy, Quận Cầu Giấy, Hà Nội", TongKhoiLuong = 45.00m, PhiGiaoHang = 245000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đang giao", ThoiGianTao = baseDate.AddDays(-8), NgayGiaoDuKien = baseDate.AddDays(-5) },
            new DonHang { MaDonHang = "DH10007", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Đỗ Văn G", SoDienThoaiNguoiNhan = "0978901234", DiaChiNguoiNhan = "12 Bis Tôn Đức Thắng, Quận 1, TP. HCM", TongKhoiLuong = 2.18m, PhiGiaoHang = 30900, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đang giao", ThoiGianTao = baseDate.AddDays(-5), NgayGiaoDuKien = baseDate.AddDays(-2) },
            new DonHang { MaDonHang = "DH10008", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Bùi Thị H", SoDienThoaiNguoiNhan = "0989012345", DiaChiNguoiNhan = "15 Nguyễn Du, Quận 1, TP. HCM", TongKhoiLuong = 0.22m, PhiGiaoHang = 21100, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đang giao", ThoiGianTao = baseDate.AddDays(-3), NgayGiaoDuKien = baseDate.AddDays(-1) },
            new DonHang { MaDonHang = "DH10025", MaKhachHang = "KH_004", TenNguoiNhan = "Dương Quốc Bảo", SoDienThoaiNguoiNhan = "0911776655", DiaChiNguoiNhan = "45 Lê Lợi, Quận 1, TP. HCM", TongKhoiLuong = 1.96m, PhiGiaoHang = 29500, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đang giao", ThoiGianTao = baseDate.AddDays(-2), NgayGiaoDuKien = baseDate.AddDays(1) },
            new DonHang { MaDonHang = "DH10026", MaKhachHang = "KH_005", TenNguoiNhan = "Lê Hải Yến", SoDienThoaiNguoiNhan = "0982554433", DiaChiNguoiNhan = "77 Xuân Thủy, Cầu Giấy, Hà Nội", TongKhoiLuong = 3.50m, PhiGiaoHang = 35000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đang giao", ThoiGianTao = baseDate.AddDays(-2), NgayGiaoDuKien = baseDate.AddDays(1) },

            // --- In Stock / Warehoused Orders (4) ---
            new DonHang { MaDonHang = "DH10009", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Ngô Văn I", SoDienThoaiNguoiNhan = "0990123456", DiaChiNguoiNhan = "88 Lý Tự Trọng, Quận 1, TP. HCM", TongKhoiLuong = 1.96m, PhiGiaoHang = 29800, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã nhập kho", ThoiGianTao = baseDate.AddDays(-2) },
            new DonHang { MaDonHang = "DH10010", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Lý Văn J", SoDienThoaiNguoiNhan = "0909988776", DiaChiNguoiNhan = "145 Đề Thám, Quận 1, TP. HCM", TongKhoiLuong = 0.80m, PhiGiaoHang = 24000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã nhập kho", ThoiGianTao = baseDate.AddDays(-1) },
            new DonHang { MaDonHang = "DH10027", MaKhachHang = "KH_002", TenNguoiNhan = "Vũ Đình Phong", SoDienThoaiNguoiNhan = "0907111222", DiaChiNguoiNhan = "150 Nguyễn Trãi, Quận 5, TP. HCM", TongKhoiLuong = 15.00m, PhiGiaoHang = 135000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Đã nhập kho", ThoiGianTao = baseDate.AddDays(-2) },
            new DonHang { MaDonHang = "DH10028", MaKhachHang = "KH_003", TenNguoiNhan = "Hoàng Bách", SoDienThoaiNguoiNhan = "0935888999", DiaChiNguoiNhan = "115 Nguyễn Huệ, Quận 1, TP. HCM", TongKhoiLuong = 0.46m, PhiGiaoHang = 21000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Đã nhập kho", ThoiGianTao = baseDate.AddDays(-1) },

            // --- Failed / Returned Orders (2) ---
            new DonHang { MaDonHang = "DH10029", MaKhachHang = "KH_004", TenNguoiNhan = "Đỗ Hoàng Long", SoDienThoaiNguoiNhan = "0984000111", DiaChiNguoiNhan = "105 Hàm Nghi, Quận 1, TP. HCM", TongKhoiLuong = 1.20m, PhiGiaoHang = 26000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Giao hàng thất bại", ThoiGianTao = baseDate.AddDays(-6), NgayGiaoDuKien = baseDate.AddDays(-4), NgayHoanThanh = baseDate.AddDays(-4) },
            new DonHang { MaDonHang = "DH10030", MaKhachHang = "KH_005", TenNguoiNhan = "Trần Bích Hà", SoDienThoaiNguoiNhan = "0977222333", DiaChiNguoiNhan = "88 Cầu Giấy, Cầu Giấy, Hà Nội", TongKhoiLuong = 1.80m, PhiGiaoHang = 27500, HinhThucThanhToan = "COD", TrangThaiDonHang = "Giao hàng thất bại", ThoiGianTao = baseDate.AddDays(-5), NgayGiaoDuKien = baseDate.AddDays(-3), NgayHoanThanh = baseDate.AddDays(-3) },

            // --- New / Pending Warehousing Orders (4) ---
            new DonHang { MaDonHang = "DH10011", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Dương Thị K", SoDienThoaiNguoiNhan = "0911223344", DiaChiNguoiNhan = "99 Pasteur, Quận 1, TP. HCM", TongKhoiLuong = 1.20m, PhiGiaoHang = 26000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Mới tạo", ThoiGianTao = DateTime.Now.AddHours(-6) },
            new DonHang { MaDonHang = "DH10012", MaKhachHang = "KH_TEST_01", TenNguoiNhan = "Trịnh Văn L", SoDienThoaiNguoiNhan = "0922334455", DiaChiNguoiNhan = "20 Điện Biên Phủ, Quận Bình Thạnh, TP. HCM", TongKhoiLuong = 65.00m, PhiGiaoHang = 345000, HinhThucThanhToan = "Chuyển khoản", TrangThaiDonHang = "Mới tạo", ThoiGianTao = DateTime.Now.AddHours(-2) },
            new DonHang { MaDonHang = "DH10031", MaKhachHang = "KH_002", TenNguoiNhan = "Phạm Gia Bảo", SoDienThoaiNguoiNhan = "0903000999", DiaChiNguoiNhan = "18 Tôn Đức Thắng, Quận 1, TP. HCM", TongKhoiLuong = 0.05m, PhiGiaoHang = 20000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Mới tạo", ThoiGianTao = DateTime.Now.AddMinutes(-30) },
            new DonHang { MaDonHang = "DH10032", MaKhachHang = "KH_003", TenNguoiNhan = "Nguyễn Ngọc Anh", SoDienThoaiNguoiNhan = "0938123000", DiaChiNguoiNhan = "415 Đề Thám, Quận 1, TP. HCM", TongKhoiLuong = 3.50m, PhiGiaoHang = 35000, HinhThucThanhToan = "COD", TrangThaiDonHang = "Mới tạo", ThoiGianTao = DateTime.Now.AddMinutes(-10) }
        );
        await context.SaveChangesAsync();

        // 12. Seed Order Details (ChiTietDonHang)
        context.ChiTietDonHangs.AddRange(
            new ChiTietDonHang { MaDonHang = "DH10001", MaHangHoa = "HH_DT_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10002", MaHangHoa = "HH_DT_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10003", MaHangHoa = "HH_DV_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn, bọc kĩ" },
            new ChiTietDonHang { MaDonHang = "DH10004", MaHangHoa = "HH_DV_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn, bọc kĩ" },
            new ChiTietDonHang { MaDonHang = "DH10005", MaHangHoa = "HH_TC_01", SoLuong = 1, TinhTrangHangHoa = "Thùng móp nhẹ" },
            new ChiTietDonHang { MaDonHang = "DH10006", MaHangHoa = "HH_TC_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10007", MaHangHoa = "HH_DV_02", SoLuong = 1, TinhTrangHangHoa = "Bọc chống sốc" },
            new ChiTietDonHang { MaDonHang = "DH10007", MaHangHoa = "HH_DT_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10008", MaHangHoa = "HH_DT_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10009", MaHangHoa = "HH_DT_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10010", MaHangHoa = "HH_DV_01", SoLuong = 1, TinhTrangHangHoa = "Bọc chống sốc" },
            new ChiTietDonHang { MaDonHang = "DH10011", MaHangHoa = "HH_DV_02", SoLuong = 1, TinhTrangHangHoa = "Bọc chống sốc" },
            new ChiTietDonHang { MaDonHang = "DH10012", MaHangHoa = "HH_TC_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10013", MaHangHoa = "HH_DT_04", SoLuong = 2, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10014", MaHangHoa = "HH_DV_04", SoLuong = 1, TinhTrangHangHoa = "Hộp nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10015", MaHangHoa = "HH_DV_03", SoLuong = 1, TinhTrangHangHoa = "Bọc gỗ bảo vệ" },
            new ChiTietDonHang { MaDonHang = "DH10016", MaHangHoa = "HH_TC_03", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10017", MaHangHoa = "HH_TC_04", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10018", MaHangHoa = "HH_DT_04", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10019", MaHangHoa = "HH_DV_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10020", MaHangHoa = "HH_DT_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10021", MaHangHoa = "HH_DT_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10022", MaHangHoa = "HH_DT_03", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10023", MaHangHoa = "HH_DV_01", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10024", MaHangHoa = "HH_DV_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10025", MaHangHoa = "HH_DT_02", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10026", MaHangHoa = "HH_DV_03", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10027", MaHangHoa = "HH_TC_03", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10028", MaHangHoa = "HH_DT_03", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10029", MaHangHoa = "HH_DV_02", SoLuong = 1, TinhTrangHangHoa = "Thùng hỏng nhẹ" },
            new ChiTietDonHang { MaDonHang = "DH10030", MaHangHoa = "HH_DV_04", SoLuong = 1, TinhTrangHangHoa = "Thùng hỏng nhẹ" },
            new ChiTietDonHang { MaDonHang = "DH10031", MaHangHoa = "HH_DT_04", SoLuong = 1, TinhTrangHangHoa = "Nguyên vẹn" },
            new ChiTietDonHang { MaDonHang = "DH10032", MaHangHoa = "HH_DV_03", SoLuong = 1, TinhTrangHangHoa = "Bọc chống sốc" }
        );
        await context.SaveChangesAsync();

        // 13. Seed Warehouse Entries (NhapKho)
        context.NhapKhos.AddRange(
            new NhapKho { MaNhapKho = "NK10001", MaDonHang = "DH10001", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-28), ViTriLuuTru = "Khu A - Kệ 03", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.22m, SoLuongKienHang = 1, TinhTrangDonHang = "Hàng nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10002", MaDonHang = "DH10002", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-25), ViTriLuuTru = "Khu A - Kệ 03", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.96m, SoLuongKienHang = 1, TinhTrangDonHang = "Hàng nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10003", MaDonHang = "DH10003", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-20), ViTriLuuTru = "Khu B - Kệ 01", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.80m, SoLuongKienHang = 1, TinhTrangDonHang = "Hộp nguyên vẹn, bọc kĩ" },
            new NhapKho { MaNhapKho = "NK10004", MaDonHang = "DH10004", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-15), ViTriLuuTru = "Khu B - Kệ 01", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.20m, SoLuongKienHang = 1, TinhTrangDonHang = "Hộp nguyên vẹn, bọc kĩ" },
            new NhapKho { MaNhapKho = "NK10005", MaDonHang = "DH10005", MaKhoHang = "K02", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-10), ViTriLuuTru = "Khu C - Kệ 05", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 65.00m, SoLuongKienHang = 1, TinhTrangDonHang = "Hàng cồng kềnh, bao bọc góc đầy đủ" },
            
            new NhapKho { MaNhapKho = "NK10013", MaDonHang = "DH10013", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-22), ViTriLuuTru = "Khu A - Kệ 01", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.10m, SoLuongKienHang = 1, TinhTrangDonHang = "Hộp nhỏ nguyên vẹn" },
            new NhapKho { MaNhapKho = "NK10014", MaDonHang = "DH10014", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-19), ViTriLuuTru = "Khu B - Kệ 02", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.80m, SoLuongKienHang = 1, TinhTrangDonHang = "Bọc chống sốc nguyên đai" },
            new NhapKho { MaNhapKho = "NK10015", MaDonHang = "DH10015", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-17), ViTriLuuTru = "Khu B - Kệ 05", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 3.50m, SoLuongKienHang = 1, TinhTrangDonHang = "Đóng khung gỗ chắc chắn" },
            new NhapKho { MaNhapKho = "NK10016", MaDonHang = "DH10016", MaKhoHang = "K02", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-14), ViTriLuuTru = "Khu C - Kệ 01", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 15.00m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10017", MaDonHang = "DH10017", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-12), ViTriLuuTru = "Khu C - Kệ 02", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 22.50m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10018", MaDonHang = "DH10018", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-9), ViTriLuuTru = "Khu A - Kệ 04", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.05m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10019", MaDonHang = "DH10019", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-8), ViTriLuuTru = "Khu B - Kệ 03", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.20m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10020", MaDonHang = "DH10020", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-7), ViTriLuuTru = "Khu A - Kệ 02", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.22m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10021", MaDonHang = "DH10021", MaKhoHang = "K02", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-6), ViTriLuuTru = "Khu B - Kệ 04", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.96m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10022", MaDonHang = "DH10022", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-5), ViTriLuuTru = "Khu B - Kệ 05", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.46m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10023", MaDonHang = "DH10023", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-4), ViTriLuuTru = "Khu A - Kệ 05", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 0.80m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên đai nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10024", MaDonHang = "DH10024", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-3), ViTriLuuTru = "Khu B - Kệ 01", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.20m, SoLuongKienHang = 1, TinhTrangDonHang = "Hàng nguyên vẹn" },

            new NhapKho { MaNhapKho = "NK10025", MaDonHang = "DH10025", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-2), ViTriLuuTru = "Khu A - Kệ 02", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 1.96m, SoLuongKienHang = 1, TinhTrangDonHang = "Thùng gỗ nguyên kiện" },
            new NhapKho { MaNhapKho = "NK10026", MaDonHang = "DH10026", MaKhoHang = "K02", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-2), ViTriLuuTru = "Khu B - Kệ 04", TrangThaiKho = "Đã xuất kho", KhoiLuongThucTe = 3.50m, SoLuongKienHang = 1, TinhTrangDonHang = "Khung gỗ nguyên kiện" },

            new NhapKho { MaNhapKho = "NK10009", MaDonHang = "DH10009", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-2), ViTriLuuTru = "Khu A - Kệ 03", TrangThaiKho = "Đang lưu kho", KhoiLuongThucTe = 1.96m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên vẹn" },
            new NhapKho { MaNhapKho = "NK10010", MaDonHang = "DH10010", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-1), ViTriLuuTru = "Khu B - Kệ 02", TrangThaiKho = "Đang lưu kho", KhoiLuongThucTe = 0.80m, SoLuongKienHang = 1, TinhTrangDonHang = "Nguyên vẹn" },
            new NhapKho { MaNhapKho = "NK10027", MaDonHang = "DH10027", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-2), ViTriLuuTru = "Khu C - Kệ 03", TrangThaiKho = "Đang lưu kho", KhoiLuongThucTe = 15.00m, SoLuongKienHang = 1, TinhTrangDonHang = "Hàng to cồng kềnh bọc tốt" },
            new NhapKho { MaNhapKho = "NK10028", MaDonHang = "DH10028", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-1), ViTriLuuTru = "Khu A - Kệ 05", TrangThaiKho = "Đang lưu kho", KhoiLuongThucTe = 0.46m, SoLuongKienHang = 1, TinhTrangDonHang = "Bao bì nguyên vẹn" },

            new NhapKho { MaNhapKho = "NK10029", MaDonHang = "DH10029", MaKhoHang = "K01", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-6), ViTriLuuTru = "Khu Hàng Hoàn - Kệ 01", TrangThaiKho = "Đang lưu kho", KhoiLuongThucTe = 1.20m, SoLuongKienHang = 1, TinhTrangDonHang = "Khách từ chối nhận - Hộp móp nhẹ" },
            new NhapKho { MaNhapKho = "NK10030", MaDonHang = "DH10030", MaKhoHang = "K02", MaNhanVien = "NV002", ThoiGianNhap = baseDate.AddDays(-5), ViTriLuuTru = "Khu Hàng Hoàn - Kệ 02", TrangThaiKho = "Đang lưu kho", KhoiLuongThucTe = 1.80m, SoLuongKienHang = 1, TinhTrangDonHang = "Khách từ chối nhận - Hộp nguyên vẹn" }
        );
        await context.SaveChangesAsync();

        // 13. Seed Delivery Journeys (HanhTrinhDonHang)
        context.HanhTrinhDonHangs.AddRange(
            // --- DH10001 (Completed) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10001", MaDonHang = "DH10001", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-28), ThoiGianHoanThanh = baseDate.AddDays(-28), TrangThai = "Giao hàng thành công", ViTriHienTai = "Kho Trung Chuyển Miền Nam" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10002", MaDonHang = "DH10001", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-27), ThoiGianHoanThanh = baseDate.AddDays(-27), TrangThai = "Giao hàng thành công", ViTriHienTai = "Đang vận chuyển trên đường Lê Lợi, Q1" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10003", MaDonHang = "DH10001", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-26), ThoiGianHoanThanh = baseDate.AddDays(-26), TrangThai = "Giao hàng thành công", ViTriHienTai = "123 Lê Lợi, Quận 1, TP. HCM", HinhAnhThucTe = "/img/deliveries/dh10001.jpg" },

            // --- DH10002 (Completed) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10004", MaDonHang = "DH10002", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-25), ThoiGianHoanThanh = baseDate.AddDays(-25), TrangThai = "Giao hàng thành công", ViTriHienTai = "Kho Trung Chuyển Miền Nam" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10005", MaDonHang = "DH10002", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-24), ThoiGianHoanThanh = baseDate.AddDays(-24), TrangThai = "Giao hàng thành công", ViTriHienTai = "Đang vận chuyển trên đường Trần Hưng Đạo, Q1" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10006", MaDonHang = "DH10002", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-23), ThoiGianHoanThanh = baseDate.AddDays(-23), TrangThai = "Giao hàng thành công", ViTriHienTai = "456 Trần Hưng Đạo, Quận 1, TP. HCM", HinhAnhThucTe = "/img/deliveries/dh10002.jpg" },

            // --- DH10003 (Completed) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10021", MaDonHang = "DH10003", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-20), ThoiGianHoanThanh = baseDate.AddDays(-18), TrangThai = "Giao hàng thành công", ViTriHienTai = "789 Nguyễn Đình Chiểu, Quận 3, TP. HCM" },

            // --- DH10004 (Completed) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10022", MaDonHang = "DH10004", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-15), ThoiGianHoanThanh = baseDate.AddDays(-13), TrangThai = "Giao hàng thành công", ViTriHienTai = "101 Hàm Nghi, Quận 1, TP. HCM" },

            // --- DH10013 to DH10024 (Completed Extra Orders) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10031", MaDonHang = "DH10013", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-22), ThoiGianHoanThanh = baseDate.AddDays(-20), TrangThai = "Giao hàng thành công", ViTriHienTai = "55 Đồng Khởi, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10032", MaDonHang = "DH10014", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-19), ThoiGianHoanThanh = baseDate.AddDays(-17), TrangThai = "Giao hàng thành công", ViTriHienTai = "120 Nguyễn Trãi, Quận 5, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10033", MaDonHang = "DH10015", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-17), ThoiGianHoanThanh = baseDate.AddDays(-15), TrangThai = "Giao hàng thành công", ViTriHienTai = "74 Pasteur, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10034", MaDonHang = "DH10016", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-14), ThoiGianHoanThanh = baseDate.AddDays(-11), TrangThai = "Giao hàng thành công", ViTriHienTai = "45 Nguyễn Phong Sắc, Cầu Giấy, Hà Nội" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10035", MaDonHang = "DH10017", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-12), ThoiGianHoanThanh = baseDate.AddDays(-10), TrangThai = "Giao hàng thành công", ViTriHienTai = "18 Điện Biên Phủ, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10036", MaDonHang = "DH10018", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-9), ThoiGianHoanThanh = baseDate.AddDays(-7), TrangThai = "Giao hàng thành công", ViTriHienTai = "250 Lý Tự Trọng, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10037", MaDonHang = "DH10019", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-8), ThoiGianHoanThanh = baseDate.AddDays(-6), TrangThai = "Giao hàng thành công", ViTriHienTai = "88 Lê Hồng Phong, Quận 5, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10038", MaDonHang = "DH10020", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-7), ThoiGianHoanThanh = baseDate.AddDays(-5), TrangThai = "Giao hàng thành công", ViTriHienTai = "190 Hàm Nghi, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10039", MaDonHang = "DH10021", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-6), ThoiGianHoanThanh = baseDate.AddDays(-4), TrangThai = "Giao hàng thành công", ViTriHienTai = "20 Xuân Thủy, Cầu Giấy, Hà Nội" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10040", MaDonHang = "DH10022", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-5), ThoiGianHoanThanh = baseDate.AddDays(-3), TrangThai = "Giao hàng thành công", ViTriHienTai = "300 Tôn Đức Thắng, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10041", MaDonHang = "DH10023", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-4), ThoiGianHoanThanh = baseDate.AddDays(-2), TrangThai = "Giao hàng thành công", ViTriHienTai = "415 Đề Thám, Quận 1, TP. HCM" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10042", MaDonHang = "DH10024", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-3), ThoiGianHoanThanh = baseDate.AddDays(-1), TrangThai = "Giao hàng thành công", ViTriHienTai = "99 Nguyễn Trãi, Quận 5, TP. HCM" },

            // --- Ongoing Delivery Journeys (6 Active Delivering) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10007", MaDonHang = "DH10005", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-10), ThoiGianHoanThanh = baseDate.AddDays(-9), TrangThai = "Giao hàng thành công", ViTriHienTai = "Kho Trung Chuyển Miền Bắc" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10008", MaDonHang = "DH10005", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-9), TrangThai = "Đang giao", ViTriHienTai = "Đang vận chuyển khu vực Cầu Giấy, Hà Nội" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10012", MaDonHang = "DH10006", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-8), TrangThai = "Giao hàng thành công", ViTriHienTai = "Kho Trung Chuyển Miền Bắc" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10043", MaDonHang = "DH10006", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-7), TrangThai = "Đang giao", ViTriHienTai = "Đang vận chuyển trên đường Xuân Thủy, Hà Nội" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10011", MaDonHang = "DH10007", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-5), TrangThai = "Giao hàng thành công", ViTriHienTai = "Kho Trung Chuyển Miền Nam" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10044", MaDonHang = "DH10007", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-4), TrangThai = "Đang giao", ViTriHienTai = "Đang vận chuyển khu vực Tôn Đức Thắng, Q1" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10009", MaDonHang = "DH10008", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-3), ThoiGianHoanThanh = baseDate.AddDays(-2), TrangThai = "Giao hàng thành công", ViTriHienTai = "Kho Trung Chuyển Miền Nam" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10010", MaDonHang = "DH10008", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-2), TrangThai = "Đang giao", ViTriHienTai = "Đang vận chuyển trên đường Nguyễn Du, Q1" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10045", MaDonHang = "DH10025", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-2), TrangThai = "Đang giao", ViTriHienTai = "Đang vận chuyển trên đường Lê Lợi, Quận 1" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10046", MaDonHang = "DH10026", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-2), TrangThai = "Đang giao", ViTriHienTai = "Đang vận chuyển khu vực Xuân Thủy, Cầu Giấy" },

            // --- Failed / Returned Journeys (2) ---
            new HanhTrinhDonHang { MaHanhTrinh = "HT10047", MaDonHang = "DH10029", MaNhanVien = "NV_SHIPPER_TEST", ThoiGianTiepNhan = baseDate.AddDays(-6), ThoiGianHoanThanh = baseDate.AddDays(-4), TrangThai = "Giao hàng thất bại", ViTriHienTai = "105 Hàm Nghi, Quận 1, TP. HCM", LyDoThatBai = "Khách từ chối nhận do bao bì móp nhẹ" },
            new HanhTrinhDonHang { MaHanhTrinh = "HT10048", MaDonHang = "DH10030", MaNhanVien = "NV004", ThoiGianTiepNhan = baseDate.AddDays(-5), ThoiGianHoanThanh = baseDate.AddDays(-3), TrangThai = "Giao hàng thất bại", ViTriHienTai = "88 Cầu Giấy, Cầu Giấy, Hà Nội", LyDoThatBai = "Khách thay đổi ý định không nhận hàng" }
        );
        await context.SaveChangesAsync();

        // 14. Seed Payments (ThanhToan)
        context.ThanhToans.AddRange(
            new ThanhToan { MaThanhToan = "TT10001", MaDonHang = "DH10001", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 21100, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-26), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10002", MaDonHang = "DH10002", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 29800, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-23), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10003", MaDonHang = "DH10003", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 24000, PhuongThucThanhToan = "Chuyển khoản", ThoiGianThanhToan = baseDate.AddDays(-18), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10004", MaDonHang = "DH10004", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 26000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-13), TrangThaiThanhToan = "Đã thanh toán" },
            
            new ThanhToan { MaThanhToan = "TT10013", MaDonHang = "DH10013", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 22000, PhuongThucThanhToan = "Chuyển khoản", ThoiGianThanhToan = baseDate.AddDays(-20), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10014", MaDonHang = "DH10014", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 28000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-17), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10015", MaDonHang = "DH10015", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 36000, PhuongThucThanhToan = "Chuyển khoản", ThoiGianThanhToan = baseDate.AddDays(-15), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10016", MaDonHang = "DH10016", MaShipper = "NV004", SoTienThanhToan = 145000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-11), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10017", MaDonHang = "DH10017", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 185000, PhuongThucThanhToan = "Chuyển khoản", ThoiGianThanhToan = baseDate.AddDays(-10), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10018", MaDonHang = "DH10018", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 20000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-7), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10019", MaDonHang = "DH10019", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 24500, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-6), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10020", MaDonHang = "DH10020", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 21000, PhuongThucThanhToan = "Chuyển khoản", ThoiGianThanhToan = baseDate.AddDays(-5), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10021", MaDonHang = "DH10021", MaShipper = "NV004", SoTienThanhToan = 28000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-4), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10022", MaDonHang = "DH10022", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 21500, PhuongThucThanhToan = "Chuyển khoản", ThoiGianThanhToan = baseDate.AddDays(-3), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10023", MaDonHang = "DH10023", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 23000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-2), TrangThaiThanhToan = "Đã thanh toán" },
            new ThanhToan { MaThanhToan = "TT10024", MaDonHang = "DH10024", MaShipper = "NV_SHIPPER_TEST", SoTienThanhToan = 25000, PhuongThucThanhToan = "COD", ThoiGianThanhToan = baseDate.AddDays(-1), TrangThaiThanhToan = "Đã thanh toán" }
        );
        await context.SaveChangesAsync();

        // 15. Seed System Audit Logs (NhatKyHeThong)
        context.NhatKyHeThongs.AddRange(
            new NhatKyHeThong { MaNhatKy = "NKHT0001", MaNhanVien = "NV001", HanhDong = "Đăng nhập hệ thống", DuLieuTacDong = "IP: 192.168.1.10", ThoiGian = DateTime.Now.AddDays(-5) },
            new NhatKyHeThong { MaNhatKy = "NKHT0002", MaNhanVien = "NV001", HanhDong = "Cập nhật thông tin tuyến giao", DuLieuTacDong = "MaTuyen: TG01", ThoiGian = DateTime.Now.AddDays(-5).AddHours(1) },
            new NhatKyHeThong { MaNhatKy = "NKHT0003", MaNhanVien = "NV002", HanhDong = "Nhập kho sản phẩm", DuLieuTacDong = "MaNhapKho: NK10001", ThoiGian = DateTime.Now.AddDays(-4) },
            new NhatKyHeThong { MaNhatKy = "NKHT0004", MaNhanVien = "NV002", HanhDong = "Cập nhật vị trí lưu kho", DuLieuTacDong = "MaNhapKho: NK10002", ThoiGian = DateTime.Now.AddDays(-4).AddHours(2) },
            new NhatKyHeThong { MaNhatKy = "NKHT0005", MaNhanVien = "NV_SHIPPER_TEST", HanhDong = "Tiếp nhận đơn hàng", DuLieuTacDong = "MaDonHang: DH10008", ThoiGian = DateTime.Now.AddDays(-3) },
            new NhatKyHeThong { MaNhatKy = "NKHT0006", MaNhanVien = "NV_SHIPPER_TEST", HanhDong = "Cập nhật hành trình", DuLieuTacDong = "MaDonHang: DH10008", ThoiGian = DateTime.Now.AddDays(-2) },
            new NhatKyHeThong { MaNhatKy = "NKHT0007", MaNhanVien = "NV004", HanhDong = "Tiếp nhận đơn hàng", DuLieuTacDong = "MaDonHang: DH10005", ThoiGian = DateTime.Now.AddDays(-2).AddHours(4) },
            new NhatKyHeThong { MaNhatKy = "NKHT0008", MaNhanVien = "NV001", HanhDong = "Xem báo cáo doanh thu tháng", DuLieuTacDong = "Thang: 05-2026", ThoiGian = DateTime.Now.AddDays(-1) },
            new NhatKyHeThong { MaNhatKy = "NKHT0009", MaNhanVien = "NV002", HanhDong = "Kiểm kê hàng tồn kho", DuLieuTacDong = "KhoHang: K01", ThoiGian = DateTime.Now.AddHours(-5) },
            new NhatKyHeThong { MaNhatKy = "NKHT0010", MaNhanVien = "NV_SHIPPER_TEST", HanhDong = "Hoàn thành ca làm việc", DuLieuTacDong = "Ca: CA_CHIEU", ThoiGian = DateTime.Now.AddHours(-1) }
        );
        await context.SaveChangesAsync();
    }
}
