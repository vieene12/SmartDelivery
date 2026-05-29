using Microsoft.AspNetCore.Identity;
using SDMS.Data;
using SDMS.Models;

namespace SDMS.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        string[] roles = { "Admin", "WarehouseStaff", "Shipper", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed Admin, WarehouseStaff, Shipper, Customer and align password hashes with local cryptography
        var defaultUsers = new[]
        {
            new { Email = "admin@sdms.com", Role = "Admin", FullName = "System Administrator", Id = "admin-test-id", MaNhanVien = "NV001", ChucVu = "Admin" },
            new { Email = "warehouse@sdms.com", Role = "WarehouseStaff", FullName = "Phạm Thị Thủ Kho", Id = "warehouse-staff-id-01", MaNhanVien = "NV002", ChucVu = "WarehouseStaff" },
            new { Email = "shipper@sdms.com", Role = "Shipper", FullName = "Nguyễn Văn Shipper", Id = "shipper-test-id", MaNhanVien = "NV003", ChucVu = "Shipper" },
            new { Email = "shipper2@sdms.com", Role = "Shipper", FullName = "Trần Văn Bưu Tá", Id = "shipper-staff-id-02", MaNhanVien = "NV004", ChucVu = "Shipper" },
            new { Email = "customer@sdms.com", Role = "Customer", FullName = "Lê Thị Khách Hàng", Id = "customer-test-id", MaNhanVien = (string)null, ChucVu = (string)null }
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

                // If it is a staff member, create NhanVien record
                if (u.MaNhanVien != null)
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
            else
            {
                // Align password hash and security stamp to match local runtime parameters
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "123456");
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                // Ensure role assignment
                if (!await userManager.IsInRoleAsync(user, u.Role))
                {
                    await userManager.AddToRoleAsync(user, u.Role);
                }

                // If NhanVien record is missing in SQL preseed, seed it
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
        }

        // Seed Categories
        if (!context.NhomHangs.Any())
        {
            context.NhomHangs.AddRange(
                new NhomHang { MaNhomHang = "DT", TenNhomHang = "Hàng Điện Tử", MoTa = "Điện thoại, Laptop, vv." },
                new NhomHang { MaNhomHang = "DV", TenNhomHang = "Hàng Dễ Vỡ", MoTa = "Gốm sứ, Thủy tinh" },
                new NhomHang { MaNhomHang = "TC", TenNhomHang = "Hàng Cồng Kềnh", MoTa = "Tủ lạnh, Máy giặt" }
            );
            await context.SaveChangesAsync();
        }

        // Seed Warehouses
        if (!context.KhoHangs.Any())
        {
            context.KhoHangs.AddRange(
                new KhoHang { MaKhoHang = "K01", TenKho = "Kho Trung Chuyển Miền Nam", DiaChiKho = "TP. HCM", DienTichKho = 5000, SucChuaKho = 10000 },
                new KhoHang { MaKhoHang = "K02", TenKho = "Kho Trung Chuyển Miền Bắc", DiaChiKho = "Hà Nội", DienTichKho = 4500, SucChuaKho = 9000 }
            );
            await context.SaveChangesAsync();
        }
    }
}
