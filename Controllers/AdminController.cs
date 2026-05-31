using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDMS.Data;
using SDMS.Models;

namespace SDMS.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // High-level reports
        var stats = new
        {
            TotalRevenue = await _context.ThanhToans.SumAsync(t => t.SoTienThanhToan),
            ActiveShippers = await _context.NhanViens.CountAsync(n => n.ChucVu == "Shipper" && n.TrangThaiLamViec == "Đang làm việc"),
            SuccessRate = await GetDeliverySuccessRate(),
            PendingBacklog = await _context.DonHangs.CountAsync(d => d.TrangThaiDonHang == "Đã nhập kho" || d.TrangThaiDonHang == "Mới tạo")
        };
        ViewBag.Stats = stats;

        var recentLogs = await _context.NhatKyHeThongs
            .Include(l => l.NhanVien)
            .OrderByDescending(l => l.ThoiGian)
            .Take(10)
            .ToListAsync();

        return View(recentLogs);
    }

    private async Task<double> GetDeliverySuccessRate()
    {
        var total = await _context.HanhTrinhDonHangs.CountAsync(p => p.ThoiGianHoanThanh != null);
        if (total == 0) return 0;
        var success = await _context.HanhTrinhDonHangs.CountAsync(p => p.TrangThai == "Giao hàng thành công" || p.TrangThai == "Thành công");
        return Math.Round((double)success / total * 100, 1);
    }

    // --- Staff Management ---
    public async Task<IActionResult> Staff()
    {
        var staffList = await _context.NhanViens.Include(n => n.User).ToListAsync();
        return View(staffList);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff(string hoTen, string soDienThoai, string email, string chucVu, string password)
    {
        if (string.IsNullOrWhiteSpace(soDienThoai))
        {
            TempData["Error"] = "Số điện thoại nhân viên không được để trống.";
            return RedirectToAction(nameof(Staff));
        }

        // Validate that this phone number is unique
        var phoneExists = await _context.NhanViens.AnyAsync(n => n.SoDienThoai == soDienThoai) || 
                            await _context.KhachHangs.AnyAsync(k => k.SoDienThoai == soDienThoai);
        if (phoneExists)
        {
            TempData["Error"] = "Số điện thoại này đã tồn tại trong hệ thống.";
            return RedirectToAction(nameof(Staff));
        }

        var dummyEmail = string.IsNullOrWhiteSpace(email) ? $"{soDienThoai}@sdms.com" : email;
        var user = new ApplicationUser 
        { 
            UserName = soDienThoai, 
            PhoneNumber = soDienThoai,
            Email = dummyEmail, 
            FullName = hoTen 
        };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, chucVu);
            
            var nhanVien = new NhanVien
            {
                MaNhanVien = "NV" + DateTime.Now.Ticks.ToString().Substring(12),
                HoTen = hoTen,
                ChucVu = chucVu,
                SoDienThoai = soDienThoai,
                Email = dummyEmail,
                MatKhau = password,
                UserId = user.Id,
                TrangThaiLamViec = "Đang làm việc",
                NgaySinh = DateTime.Now.AddYears(-25) // Default
            };
            _context.NhanViens.Add(nhanVien);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Thêm nhân viên mới thành công! Số điện thoại đăng nhập: {soDienThoai}";
        }
        else
        {
            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return RedirectToAction(nameof(Staff));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStaffStatus(string id)
    {
        var staff = await _context.NhanViens.FindAsync(id);
        if (staff != null)
        {
            staff.TrangThaiLamViec = staff.TrangThaiLamViec == "Đang làm việc" ? "Đã khóa / Nghỉ việc" : "Đang làm việc";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Staff));
    }

    // --- Warehouse & Category Management ---
    public async Task<IActionResult> Warehouses()
    {
        var warehouses = await _context.KhoHangs.ToListAsync();
        return View(warehouses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(string maKhoHang, string tenKho, string diaChiKho, decimal dienTichKho, int sucChuaKho, string trangThai)
    {
        if (string.IsNullOrWhiteSpace(maKhoHang) || string.IsNullOrWhiteSpace(tenKho))
        {
            TempData["Error"] = "Mã kho và tên kho không được để trống.";
            return RedirectToAction(nameof(Warehouses));
        }

        var existing = await _context.KhoHangs.AnyAsync(w => w.MaKhoHang == maKhoHang);
        if (existing)
        {
            TempData["Error"] = "Mã kho hàng này đã tồn tại trong hệ thống.";
            return RedirectToAction(nameof(Warehouses));
        }

        var warehouse = new KhoHang
        {
            MaKhoHang = maKhoHang.Trim(),
            TenKho = tenKho.Trim(),
            DiaChiKho = diaChiKho?.Trim(),
            DienTichKho = dienTichKho,
            SucChuaKho = sucChuaKho,
            TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Hoạt động" : trangThai.Trim()
        };
        _context.KhoHangs.Add(warehouse);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Thêm kho hàng mới thành công!";
        return RedirectToAction(nameof(Warehouses));
    }

    public async Task<IActionResult> Categories()
    {
        var categories = await _context.NhomHangs.ToListAsync();
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(string maNhomHang, string tenNhomHang, string moTa)
    {
        if (string.IsNullOrWhiteSpace(maNhomHang) || string.IsNullOrWhiteSpace(tenNhomHang))
        {
            TempData["Error"] = "Mã nhóm hàng và tên nhóm hàng không được để trống.";
            return RedirectToAction(nameof(Categories));
        }

        var existing = await _context.NhomHangs.AnyAsync(c => c.MaNhomHang == maNhomHang);
        if (existing)
        {
            TempData["Error"] = "Mã nhóm hàng này đã tồn tại.";
            return RedirectToAction(nameof(Categories));
        }

        var category = new NhomHang
        {
            MaNhomHang = maNhomHang.Trim(),
            TenNhomHang = tenNhomHang.Trim(),
            MoTa = moTa?.Trim()
        };
        _context.NhomHangs.Add(category);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Thêm nhóm hàng mới thành công!";
        return RedirectToAction(nameof(Categories));
    }

    // --- Delivery Routes Management ---
    public async Task<IActionResult> Routes()
    {
        var routes = await _context.TuyenGiaos.ToListAsync();
        return View(routes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoute(string maTuyen, string tenTuyen, string khuVuc, string moTa)
    {
        if (string.IsNullOrWhiteSpace(maTuyen) || string.IsNullOrWhiteSpace(tenTuyen) || string.IsNullOrWhiteSpace(khuVuc))
        {
            TempData["Error"] = "Mã tuyến, tên tuyến và khu vực không được để trống.";
            return RedirectToAction(nameof(Routes));
        }

        var existing = await _context.TuyenGiaos.AnyAsync(r => r.MaTuyen == maTuyen);
        if (existing)
        {
            TempData["Error"] = "Mã tuyến đường này đã tồn tại trong hệ thống.";
            return RedirectToAction(nameof(Routes));
        }

        var route = new TuyenGiao
        {
            MaTuyen = maTuyen.Trim(),
            TenTuyen = tenTuyen.Trim(),
            KhuVuc = khuVuc.Trim(),
            MoTa = moTa?.Trim()
        };
        _context.TuyenGiaos.Add(route);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Thêm tuyến giao mới thành công!";
        return RedirectToAction(nameof(Routes));
    }

    // --- Work Shifts Management ---
    public async Task<IActionResult> Shifts()
    {
        var shifts = await _context.CaLamViecs.ToListAsync();
        return View(shifts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShift(string maCa, string tenCa, string gioBatDau, string gioKetThuc)
    {
        if (string.IsNullOrWhiteSpace(maCa) || string.IsNullOrWhiteSpace(tenCa) || string.IsNullOrWhiteSpace(gioBatDau) || string.IsNullOrWhiteSpace(gioKetThuc))
        {
            TempData["Error"] = "Mã ca, tên ca, giờ bắt đầu và giờ kết thúc không được để trống.";
            return RedirectToAction(nameof(Shifts));
        }

        var existing = await _context.CaLamViecs.AnyAsync(s => s.MaCa == maCa);
        if (existing)
        {
            TempData["Error"] = "Mã ca làm việc này đã tồn tại trong hệ thống.";
            return RedirectToAction(nameof(Shifts));
        }

        if (!DateTime.TryParse(gioBatDau, out DateTime parsedStart) || !DateTime.TryParse(gioKetThuc, out DateTime parsedEnd))
        {
            TempData["Error"] = "Định dạng thời gian không hợp lệ.";
            return RedirectToAction(nameof(Shifts));
        }

        var shift = new CaLamViec
        {
            MaCa = maCa.Trim(),
            TenCa = tenCa.Trim(),
            GioBatDau = parsedStart,
            GioKetThuc = parsedEnd
        };
        _context.CaLamViecs.Add(shift);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Thêm ca làm việc mới thành công!";
        return RedirectToAction(nameof(Shifts));
    }

    // --- Audit Logs ---
    public async Task<IActionResult> AuditLogs()
    {
        var logs = await _context.NhatKyHeThongs
            .Include(l => l.NhanVien)
            .OrderByDescending(l => l.ThoiGian)
            .ToListAsync();
        return View(logs);
    }

    // --- Order Statistics Report ---
    public async Task<IActionResult> OrderReport()
    {
        // Thống kê theo trạng thái
        var byStatus = await _context.DonHangs
            .GroupBy(d => d.TrangThaiDonHang)
            .Select(g => new { TrangThai = g.Key, SoLuong = g.Count(), TongPhi = g.Sum(d => d.PhiGiaoHang) })
            .OrderByDescending(g => g.SoLuong)
            .ToListAsync();

        // Thống kê theo tháng (12 tháng gần nhất)
        var twelveMonthsAgo = DateTime.Now.AddMonths(-11);
        var byMonth = await _context.DonHangs
            .Where(d => d.ThoiGianTao >= twelveMonthsAgo)
            .GroupBy(d => new { d.ThoiGianTao.Year, d.ThoiGianTao.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, SoLuong = g.Count(), TongPhi = g.Sum(d => d.PhiGiaoHang) })
            .OrderBy(g => g.Year).ThenBy(g => g.Month)
            .ToListAsync();

        // Thống kê theo shipper (top 10)
        var byShipper = await _context.HanhTrinhDonHangs
            .Include(h => h.NhanVien)
            .GroupBy(h => h.MaNhanVien)
            .Select(g => new
            {
                MaNhanVien = g.Key,
                TenNhanVien = g.First().NhanVien != null ? g.First().NhanVien!.HoTen : "N/A",
                TongDon = g.Count(),
                ThanhCong = g.Count(h => h.TrangThai == "Giao hàng thành công"),
                ThatBai = g.Count(h => h.TrangThai == "Giao hàng thất bại")
            })
            .OrderByDescending(g => g.TongDon)
            .Take(10)
            .ToListAsync();

        // Tổng hợp chung
        var tongDonHang = await _context.DonHangs.CountAsync();
        var tongDoanhThu = await _context.ThanhToans.SumAsync(t => t.SoTienThanhToan);
        var tongPhiGiao = await _context.DonHangs.SumAsync(d => d.PhiGiaoHang);
        var donThanhCong = await _context.HanhTrinhDonHangs.CountAsync(h => h.TrangThai == "Giao hàng thành công");
        var donThatBai = await _context.HanhTrinhDonHangs.CountAsync(h => h.TrangThai == "Giao hàng thất bại");

        ViewBag.ByStatus = byStatus.Select(x => new { x.TrangThai, x.SoLuong, x.TongPhi }).ToList<dynamic>();
        ViewBag.ByMonth = byMonth.Select(x => new { x.Year, x.Month, x.SoLuong, x.TongPhi }).ToList<dynamic>();
        ViewBag.ByShipper = byShipper.Select(x => new { x.TenNhanVien, x.TongDon, x.ThanhCong, x.ThatBai }).ToList<dynamic>();
        ViewBag.TongDonHang = tongDonHang;
        ViewBag.TongDoanhThu = tongDoanhThu;
        ViewBag.TongPhiGiao = tongPhiGiao;
        ViewBag.DonThanhCong = donThanhCong;
        ViewBag.DonThatBai = donThatBai;
        ViewBag.NgayXuatBaoCao = DateTime.Now;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EditStaff(string maNhanVien, string hoTen, string soDienThoai, string email, string chucVu, string password)
    {
        var staff = await _context.NhanViens.Include(n => n.User).FirstOrDefaultAsync(n => n.MaNhanVien == maNhanVien);
        if (staff == null)
        {
            TempData["Error"] = "Không tìm thấy nhân viên.";
            return RedirectToAction(nameof(Staff));
        }

        if (string.IsNullOrWhiteSpace(soDienThoai))
        {
            TempData["Error"] = "Số điện thoại không được để trống.";
            return RedirectToAction(nameof(Staff));
        }

        if (staff.SoDienThoai != soDienThoai)
        {
            var phoneExists = await _context.NhanViens.AnyAsync(n => n.SoDienThoai == soDienThoai) || 
                                await _context.KhachHangs.AnyAsync(k => k.SoDienThoai == soDienThoai);
            if (phoneExists)
            {
                TempData["Error"] = "Số điện thoại này đã tồn tại.";
                return RedirectToAction(nameof(Staff));
            }
        }

        staff.HoTen = hoTen;
        staff.SoDienThoai = soDienThoai;
        staff.Email = email;
        
        if (staff.User != null)
        {
            staff.User.FullName = hoTen;
            staff.User.PhoneNumber = soDienThoai;
            staff.User.Email = email;
            staff.User.NormalizedEmail = email?.ToUpper();
            staff.User.UserName = soDienThoai;
            staff.User.NormalizedUserName = soDienThoai.ToUpper();
            
            if (staff.ChucVu != chucVu)
            {
                await _userManager.RemoveFromRoleAsync(staff.User, staff.ChucVu);
                await _userManager.AddToRoleAsync(staff.User, chucVu);
            }

            if (!string.IsNullOrWhiteSpace(password) && password != staff.MatKhau)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(staff.User);
                await _userManager.ResetPasswordAsync(staff.User, token, password);
                staff.MatKhau = password;
            }

            await _userManager.UpdateAsync(staff.User);
        }

        staff.ChucVu = chucVu;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Cập nhật nhân viên thành công!";
        return RedirectToAction(nameof(Staff));
    }

    [HttpPost]
    public async Task<IActionResult> EditWarehouse(string maKhoHang, string tenKho, string diaChiKho, decimal dienTichKho, int sucChuaKho, string trangThai)
    {
        var warehouse = await _context.KhoHangs.FindAsync(maKhoHang);
        if (warehouse == null)
        {
            TempData["Error"] = "Không tìm thấy kho hàng.";
            return RedirectToAction(nameof(Warehouses));
        }

        if (string.IsNullOrWhiteSpace(tenKho))
        {
            TempData["Error"] = "Tên kho không được để trống.";
            return RedirectToAction(nameof(Warehouses));
        }

        warehouse.TenKho = tenKho.Trim();
        warehouse.DiaChiKho = diaChiKho?.Trim();
        warehouse.DienTichKho = dienTichKho;
        warehouse.SucChuaKho = sucChuaKho;
        warehouse.TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Hoạt động" : trangThai.Trim();

        await _context.SaveChangesAsync();
        TempData["Success"] = "Cập nhật kho hàng thành công!";
        return RedirectToAction(nameof(Warehouses));
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(string maNhomHang, string tenNhomHang, string moTa)
    {
        var category = await _context.NhomHangs.FindAsync(maNhomHang);
        if (category == null)
        {
            TempData["Error"] = "Không tìm thấy nhóm hàng.";
            return RedirectToAction(nameof(Categories));
        }

        if (string.IsNullOrWhiteSpace(tenNhomHang))
        {
            TempData["Error"] = "Tên nhóm hàng không được để trống.";
            return RedirectToAction(nameof(Categories));
        }

        category.TenNhomHang = tenNhomHang.Trim();
        category.MoTa = moTa?.Trim();

        await _context.SaveChangesAsync();
        TempData["Success"] = "Cập nhật nhóm hàng thành công!";
        return RedirectToAction(nameof(Categories));
    }

    [HttpPost]
    public async Task<IActionResult> EditRoute(string maTuyen, string tenTuyen, string khuVuc, string moTa)
    {
        var route = await _context.TuyenGiaos.FindAsync(maTuyen);
        if (route == null)
        {
            TempData["Error"] = "Không tìm thấy tuyến đường.";
            return RedirectToAction(nameof(Routes));
        }

        if (string.IsNullOrWhiteSpace(tenTuyen) || string.IsNullOrWhiteSpace(khuVuc))
        {
            TempData["Error"] = "Tên tuyến và khu vực không được để trống.";
            return RedirectToAction(nameof(Routes));
        }

        route.TenTuyen = tenTuyen.Trim();
        route.KhuVuc = khuVuc.Trim();
        route.MoTa = moTa?.Trim();

        await _context.SaveChangesAsync();
        TempData["Success"] = "Cập nhật tuyến đường thành công!";
        return RedirectToAction(nameof(Routes));
    }

    // --- Customer Management ---
    public async Task<IActionResult> Customers(string search)
    {
        var query = _context.KhachHangs
            .Include(k => k.User)
            .Include(k => k.DonHangs)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();
            query = query.Where(k => k.HoTen.ToLower().Contains(search) || 
                                     k.SoDienThoai.Contains(search) || 
                                     (k.Email != null && k.Email.ToLower().Contains(search)));
        }

        var customers = await query.ToListAsync();
        ViewBag.Search = search;
        return View(customers);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerDetails(string id)
    {
        var customer = await _context.KhachHangs
            .Include(k => k.User)
            .Include(k => k.DonHangs)
            .FirstOrDefaultAsync(k => k.MaKhachHang == id);

        if (customer == null)
        {
            return NotFound();
        }

        var isLocked = false;
        if (customer.User != null)
        {
            isLocked = await _userManager.IsLockedOutAsync(customer.User);
        }

        var orders = customer.DonHangs
            .OrderByDescending(d => d.ThoiGianTao)
            .Select(d => new
            {
                maDonHang = d.MaDonHang,
                thoiGianTao = d.ThoiGianTao.ToString("dd/MM/yyyy HH:mm"),
                ngayGiaoDuKien = d.NgayGiaoDuKien.HasValue ? d.NgayGiaoDuKien.Value.ToString("dd/MM/yyyy") : "Chưa xác định",
                tongKhoiLuong = d.TongKhoiLuong,
                phiGiaoHang = d.PhiGiaoHang.ToString("N0") + " đ",
                trangThaiDonHang = d.TrangThaiDonHang
            })
            .ToList();

        return Json(new
        {
            maKhachHang = customer.MaKhachHang,
            hoTen = customer.HoTen,
            soDienThoai = customer.SoDienThoai,
            email = customer.Email ?? "",
            diaChi = customer.DiaChi ?? "",
            isLocked = isLocked,
            hasUser = customer.UserId != null,
            orders = orders
        });
    }

    [HttpPost]
    public async Task<IActionResult> EditCustomer(string maKhachHang, string hoTen, string soDienThoai, string email, string diaChi)
    {
        var customer = await _context.KhachHangs.Include(k => k.User).FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);
        if (customer == null)
        {
            TempData["Error"] = "Không tìm thấy khách hàng.";
            return RedirectToAction(nameof(Customers));
        }

        if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(soDienThoai))
        {
            TempData["Error"] = "Họ tên và số điện thoại không được để trống.";
            return RedirectToAction(nameof(Customers));
        }

        // Validate phone number unique among NhanVien and other KhachHang
        if (customer.SoDienThoai != soDienThoai)
        {
            var phoneExists = await _context.NhanViens.AnyAsync(n => n.SoDienThoai == soDienThoai) || 
                                await _context.KhachHangs.AnyAsync(k => k.SoDienThoai == soDienThoai && k.MaKhachHang != maKhachHang);
            if (phoneExists)
            {
                TempData["Error"] = "Số điện thoại này đã được sử dụng bởi người dùng khác.";
                return RedirectToAction(nameof(Customers));
            }
        }

        customer.HoTen = hoTen.Trim();
        customer.SoDienThoai = soDienThoai.Trim();
        customer.Email = email?.Trim();
        customer.DiaChi = diaChi?.Trim();

        if (customer.User != null)
        {
            customer.User.FullName = customer.HoTen;
            customer.User.PhoneNumber = customer.SoDienThoai;
            customer.User.Address = customer.DiaChi;
            customer.User.Email = customer.Email;
            customer.User.NormalizedEmail = customer.Email?.ToUpper();
            customer.User.UserName = customer.SoDienThoai;
            customer.User.NormalizedUserName = customer.SoDienThoai.ToUpper();

            var result = await _userManager.UpdateAsync(customer.User);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Lỗi khi cập nhật tài khoản người dùng: " + string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Customers));
            }
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Cập nhật thông tin khách hàng thành công!";
        return RedirectToAction(nameof(Customers));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleCustomerStatus(string id)
    {
        var customer = await _context.KhachHangs.Include(k => k.User).FirstOrDefaultAsync(k => k.MaKhachHang == id);
        if (customer == null)
        {
            TempData["Error"] = "Không tìm thấy khách hàng.";
            return RedirectToAction(nameof(Customers));
        }

        if (customer.User == null)
        {
            TempData["Error"] = "Khách hàng này không có tài khoản đăng nhập để khóa.";
            return RedirectToAction(nameof(Customers));
        }

        var isLocked = await _userManager.IsLockedOutAsync(customer.User);
        if (isLocked)
        {
            // Unlock
            await _userManager.SetLockoutEndDateAsync(customer.User, null);
            TempData["Success"] = $"Đã mở khóa tài khoản khách hàng {customer.HoTen} thành công!";
        }
        else
        {
            // Lock
            await _userManager.SetLockoutEnabledAsync(customer.User, true);
            await _userManager.SetLockoutEndDateAsync(customer.User, DateTimeOffset.UtcNow.AddYears(100));
            TempData["Success"] = $"Đã khóa tài khoản khách hàng {customer.HoTen} thành công!";
        }

        return RedirectToAction(nameof(Customers));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(string hoTen, string soDienThoai, string email, string diaChi, string password)
    {
        if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(soDienThoai))
        {
            TempData["Error"] = "Họ tên và số điện thoại không được để trống.";
            return RedirectToAction(nameof(Customers));
        }

        // Validate phone number unique among NhanVien and KhachHang
        var phoneExists = await _context.NhanViens.AnyAsync(n => n.SoDienThoai == soDienThoai) || 
                            await _context.KhachHangs.AnyAsync(k => k.SoDienThoai == soDienThoai);
        if (phoneExists)
        {
            TempData["Error"] = "Số điện thoại này đã tồn tại trong hệ thống.";
            return RedirectToAction(nameof(Customers));
        }

        var dummyEmail = string.IsNullOrWhiteSpace(email) ? $"{soDienThoai}@sdms.com" : email;
        var user = new ApplicationUser 
        { 
            UserName = soDienThoai, 
            PhoneNumber = soDienThoai,
            Email = dummyEmail, 
            FullName = hoTen,
            Address = diaChi
        };
        
        var initPassword = string.IsNullOrWhiteSpace(password) ? "Customer@123" : password;
        var result = await _userManager.CreateAsync(user, initPassword);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            
            var khachHang = new KhachHang
            {
                MaKhachHang = "KH" + DateTime.Now.Ticks.ToString().Substring(12),
                HoTen = hoTen.Trim(),
                SoDienThoai = soDienThoai.Trim(),
                Email = dummyEmail,
                DiaChi = diaChi?.Trim(),
                UserId = user.Id
            };
            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Thêm khách hàng mới thành công! Số điện thoại đăng nhập: {soDienThoai}";
        }
        else
        {
            TempData["Error"] = "Lỗi tạo tài khoản: " + string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return RedirectToAction(nameof(Customers));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var customer = await _context.KhachHangs.Include(k => k.User).FirstOrDefaultAsync(k => k.MaKhachHang == id);
        if (customer == null)
        {
            TempData["Error"] = "Không tìm thấy khách hàng.";
            return RedirectToAction(nameof(Customers));
        }

        // Check if customer has any orders
        var hasOrders = await _context.DonHangs.AnyAsync(d => d.MaKhachHang == id);
        if (hasOrders)
        {
            TempData["Error"] = "Không thể xóa khách hàng vì đã có lịch sử đơn hàng trên hệ thống. Hãy sử dụng chức năng Khóa tài khoản để đảm bảo an toàn dữ liệu.";
            return RedirectToAction(nameof(Customers));
        }

        // If they have no orders, delete them
        _context.KhachHangs.Remove(customer);
        
        if (customer.User != null)
        {
            var result = await _userManager.DeleteAsync(customer.User);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Lỗi khi xóa tài khoản người dùng: " + string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Customers));
            }
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = $"Đã xóa khách hàng {customer.HoTen} thành công!";
        return RedirectToAction(nameof(Customers));
    }
}
