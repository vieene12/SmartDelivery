using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SDMS.Data;
using SDMS.Models;
using System.Security.Claims;

namespace SDMS.Controllers;

[Authorize]
public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.UserId == userId);

        if (khachHang == null)
        {
            var user = await _context.Users.FindAsync(userId);
            khachHang = new KhachHang
            {
                MaKhachHang = "KH" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                HoTen = user?.FullName ?? User.Identity?.Name ?? "Customer",
                SoDienThoai = user?.PhoneNumber ?? "0123456789",
                UserId = userId
            };
            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();
        }
        else if (khachHang.HoTen != null && khachHang.HoTen.Contains("@"))
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.FullName))
            {
                khachHang.HoTen = user.FullName;
                _context.Update(khachHang);
                await _context.SaveChangesAsync();
            }
        }

        var orders = await _context.DonHangs
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.HangHoa)
            .Where(d => d.MaKhachHang == khachHang.MaKhachHang)
            .OrderByDescending(d => d.ThoiGianTao)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Orders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await _context.DonHangs
            .Include(d => d.KhachHang)
            .Where(d => d.KhachHang.UserId == userId)
            .OrderByDescending(d => d.ThoiGianTao)
            .ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> CreateOrder()
    {
        ViewBag.HangHoas = await _context.HangHoas.ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(DonHang order, List<ManualItemInput>? manualItems)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.UserId == userId);

        if (khachHang == null)
        {
            var user = await _context.Users.FindAsync(userId);
            khachHang = new KhachHang
            {
                MaKhachHang = "KH" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                HoTen = user?.FullName ?? User.Identity?.Name ?? "Customer",
                SoDienThoai = user?.PhoneNumber ?? "0123456789",
                UserId = userId
            };
            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();
        }
        else if (khachHang.HoTen != null && khachHang.HoTen.Contains("@"))
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.FullName))
            {
                khachHang.HoTen = user.FullName;
                _context.Update(khachHang);
                await _context.SaveChangesAsync();
            }
        }

        order.MaDonHang = "DH" + DateTime.Now.Ticks.ToString().Substring(10);
        order.MaKhachHang = khachHang.MaKhachHang;
        order.ThoiGianTao = DateTime.Now;
        order.TrangThaiDonHang = "Mới tạo";
        
        // Automated logistics fee calculation: Base 20k VND + 5k VND/kg
        order.PhiGiaoHang = 20000 + (order.TongKhoiLuong * 5000);

        _context.DonHangs.Add(order);

        if (manualItems != null && manualItems.Count > 0)
        {
            // Ensure default custom category "KHAC" exists
            var nhomHangKhac = await _context.NhomHangs.FirstOrDefaultAsync(n => n.MaNhomHang == "KHAC");
            if (nhomHangKhac == null)
            {
                nhomHangKhac = new NhomHang
                {
                    MaNhomHang = "KHAC",
                    TenNhomHang = "Hàng hóa khác",
                    MoTa = "Khách hàng tự nhập"
                };
                _context.NhomHangs.Add(nhomHangKhac);
                await _context.SaveChangesAsync();
            }

            foreach (var item in manualItems)
            {
                if (string.IsNullOrWhiteSpace(item.TenHangHoa)) continue;

                // Create a unique database product record for this item
                var hangHoa = new HangHoa
                {
                    MaHangHoa = "HH" + Guid.NewGuid().ToString("N").Substring(0, 18).ToUpper(),
                    MaNhomHang = "KHAC",
                    TenHangHoa = item.TenHangHoa,
                    KhoiLuong = item.KhoiLuong,
                    DonViTinh = "Cái",
                    MoTaChiTiet = $"Tự nhập từ đơn hàng {order.MaDonHang}"
                };
                _context.HangHoas.Add(hangHoa);

                // Link the product to this order
                var detail = new ChiTietDonHang
                {
                    MaDonHang = order.MaDonHang,
                    MaHangHoa = hangHoa.MaHangHoa,
                    SoLuong = item.SoLuong > 0 ? item.SoLuong : 1,
                    TinhTrangHangHoa = "Mới"
                };
                _context.ChiTietDonHangs.Add(detail);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Orders));
    }

    public async Task<IActionResult> Track(string id)
    {
        var order = await _context.DonHangs
            .Include(d => d.KhachHang)
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.HangHoa)
            .Include(d => d.NhapKhos)
                .ThenInclude(n => n.KhoHang)
            .Include(d => d.NhapKhos)
                .ThenInclude(n => n.NhanVien)
            .Include(d => d.HanhTrinhDonHangs)
                .ThenInclude(h => h.NhanVien)
            .FirstOrDefaultAsync(d => d.MaDonHang == id);

        if (order == null) return NotFound();

        return View(order);
    }

    public async Task<IActionResult> PrintLabel(string id)
    {
        var order = await _context.DonHangs
            .Include(d => d.KhachHang)
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.HangHoa)
            .FirstOrDefaultAsync(d => d.MaDonHang == id);

        if (order == null) return NotFound();

        return View(order);
    }
}

public class ManualItemInput
{
    public string TenHangHoa { get; set; } = string.Empty;
    public decimal KhoiLuong { get; set; } // in kg
    public int SoLuong { get; set; }
}
