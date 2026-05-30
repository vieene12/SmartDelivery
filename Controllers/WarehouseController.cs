using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDMS.Data;
using SDMS.Models;
using System.Security.Claims;

namespace SDMS.Controllers;

[Authorize(Roles = "Admin,WarehouseStaff")]
public class WarehouseController : Controller
{
    private readonly ApplicationDbContext _context;

    public WarehouseController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Dashboard summary for Warehouse
        var stats = new
        {
            PendingOrders = await _context.DonHangs.CountAsync(d => d.TrangThaiDonHang == "Mới tạo"),
            InStock = await _context.DonHangs.CountAsync(d => d.TrangThaiDonHang == "Đã nhập kho"),
            Returns = await _context.DonHangs.CountAsync(d => d.TrangThaiDonHang == "Giao hàng thất bại")
        };
        ViewBag.Stats = stats;

        var recentOrders = await _context.DonHangs
            .Include(d => d.KhachHang)
            .OrderByDescending(d => d.ThoiGianTao)
            .Take(10)
            .ToListAsync();

        return View(recentOrders);
    }

    public async Task<IActionResult> Inventory()
    {
        var inventory = await _context.NhapKhos
            .Include(q => q.DonHang)
            .Include(q => q.KhoHang)
            .OrderByDescending(q => q.ThoiGianNhap)
            .ToListAsync();
        return View(inventory);
    }

    public async Task<IActionResult> ReceiveOrder(string id)
    {
        var order = await _context.DonHangs
            .Include(d => d.KhachHang)
            .Include(d => d.ChiTietDonHangs)
            .ThenInclude(c => c.HangHoa)
            .FirstOrDefaultAsync(d => d.MaDonHang == id);

        if (order == null) return NotFound();

        ViewBag.Warehouses = await _context.KhoHangs.ToListAsync();
        return View(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderDetails(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Json(new { success = false, message = "Mã đơn hàng không hợp lệ." });
        }

        var order = await _context.DonHangs
            .Include(d => d.KhachHang)
            .Include(d => d.ChiTietDonHangs)
            .ThenInclude(c => c.HangHoa)
            .FirstOrDefaultAsync(d => d.MaDonHang == id.Trim());

        if (order == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đơn hàng trong hệ thống." });
        }

        return Json(new
        {
            success = true,
            maDonHang = order.MaDonHang,
            nguoiGui = order.KhachHang?.HoTen ?? "N/A",
            nguoiNhan = order.TenNguoiNhan,
            diaChiNhan = order.DiaChiNguoiNhan,
            tongKhoiLuong = order.TongKhoiLuong,
            trangThai = order.TrangThaiDonHang,
            items = order.ChiTietDonHangs.Select(c => new
            {
                tenHangHoa = c.HangHoa?.TenHangHoa ?? "N/A",
                soLuong = c.SoLuong,
                khoiLuong = c.HangHoa?.KhoiLuong ?? 0
            }).ToList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmReceipt(string maDonHang, string maKhoHang, string viTri, string ghiChu, decimal? khoiLuongThucTe, int? soLuongKienHang)
    {
        var order = await _context.DonHangs.FindAsync(maDonHang);
        if (order == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(n => n.UserId == userId);

        // Update Order Status
        order.TrangThaiDonHang = "Đã nhập kho";

        // Add to Inventory Management
        var inventoryLog = new NhapKho
        {
            MaNhapKho = "NK" + DateTime.Now.Ticks.ToString().Substring(10),
            MaDonHang = maDonHang,
            MaKhoHang = maKhoHang,
            MaNhanVien = nhanVien?.MaNhanVien ?? "NV_SYSTEM",
            ThoiGianNhap = DateTime.Now,
            ViTriLuuTru = viTri,
            TrangThaiKho = "Đã nhập kho",
            TinhTrangDonHang = ghiChu,
            KhoiLuongThucTe = khoiLuongThucTe,
            SoLuongKienHang = soLuongKienHang
        };

        _context.NhapKhos.Add(inventoryLog);

        // Smart logistics logic: Automatically assign Shipper by matching delivery address to configured shipper routes
        var routes = await _context.TuyenGiaos.ToListAsync();
        TuyenGiao? matchedRoute = null;
        if (order != null && !string.IsNullOrEmpty(order.DiaChiNguoiNhan))
        {
            matchedRoute = routes.FirstOrDefault(r => order.DiaChiNguoiNhan.ToLower().Contains(r.TenTuyen.ToLower()));
        }

        NhanVien? shipper = null;
        if (matchedRoute != null)
        {
            // Find an active shipper assigned to this route
            shipper = await _context.PhanCongTuyens
                .Include(p => p.NhanVien)
                .Where(p => p.MaTuyen == matchedRoute.MaTuyen 
                            && p.NhanVien.ChucVu == "Shipper" 
                            && p.NhanVien.TrangThaiLamViec == "Đang làm việc"
                            && p.NgayBatDau <= DateTime.Now 
                            && (p.NgayKetThuc == null || p.NgayKetThuc >= DateTime.Now))
                .Select(p => p.NhanVien)
                .FirstOrDefaultAsync();
        }

        // Fallback to any active shipper if no route-specific shipper is found
        if (shipper == null)
        {
            shipper = await _context.NhanViens
                .FirstOrDefaultAsync(n => n.ChucVu == "Shipper" && n.TrangThaiLamViec == "Đang làm việc");
        }

        if (shipper != null && order != null)
        {
            var assignment = new HanhTrinhDonHang
            {
                MaHanhTrinh = "HT" + DateTime.Now.Ticks.ToString().Substring(10),
                MaDonHang = maDonHang,
                MaNhanVien = shipper.MaNhanVien,
                ThoiGianTiepNhan = DateTime.Now,
                TrangThai = "Chờ shipper lấy hàng"
            };
            _context.HanhTrinhDonHangs.Add(assignment);
            order.TrangThaiDonHang = "Đã phân công";
        }

        await _context.SaveChangesAsync();

        // Audit Log
        var audit = new NhatKyHeThong
        {
            MaNhatKy = "LOG" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
            MaNhanVien = nhanVien?.MaNhanVien ?? "NV_SYSTEM",
            HanhDong = "Nhập kho và phân công",
            DuLieuTacDong = $"Đơn hàng: {maDonHang}",
            ThoiGian = DateTime.Now
        };
        _context.NhatKyHeThongs.Add(audit);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
