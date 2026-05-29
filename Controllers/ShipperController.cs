using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDMS.Data;
using SDMS.Models;
using System.Security.Claims;

namespace SDMS.Controllers;

[Authorize(Roles = "Admin,Shipper")]
public class ShipperController : Controller
{
    private readonly ApplicationDbContext _context;

    public ShipperController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var shipper = await _context.NhanViens.FirstOrDefaultAsync(n => n.UserId == userId);
        
        if (shipper == null) return NotFound("Shipper profile not found.");

        var stats = new
        {
            AssignedCount = await _context.HanhTrinhDonHangs.CountAsync(p => p.MaNhanVien == shipper.MaNhanVien && p.TrangThai == "Chờ shipper lấy hàng"),
            DeliveringCount = await _context.HanhTrinhDonHangs.CountAsync(p => p.MaNhanVien == shipper.MaNhanVien && p.TrangThai == "Đang giao"),
            CompletedToday = await _context.HanhTrinhDonHangs.CountAsync(p => p.MaNhanVien == shipper.MaNhanVien && p.TrangThai == "Giao hàng thành công" && p.ThoiGianHoanThanh >= DateTime.Today)
        };
        ViewBag.Stats = stats;

        return View();
    }

    public async Task<IActionResult> MyDeliveries()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var shipper = await _context.NhanViens.FirstOrDefaultAsync(n => n.UserId == userId);
        
        var deliveries = await _context.HanhTrinhDonHangs
            .Include(p => p.DonHang)
            .ThenInclude(d => d.KhachHang)
            .Where(p => p.MaNhanVien == shipper.MaNhanVien && (p.TrangThai == "Chờ shipper lấy hàng" || p.TrangThai == "Đang giao"))
            .OrderByDescending(p => p.ThoiGianTiepNhan)
            .ToListAsync();

        return View(deliveries);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ScanToPickUp(string maHanhTrinh)
    {
        var assignment = await _context.HanhTrinhDonHangs
            .Include(p => p.DonHang)
            .FirstOrDefaultAsync(p => p.MaHanhTrinh == maHanhTrinh);

        if (assignment == null) return NotFound();

        // 1. Tìm phân công tuyến đang hoạt động của Shipper này
        var activeRoute = await _context.PhanCongTuyens
            .Include(p => p.TuyenGiao)
            .FirstOrDefaultAsync(p => p.MaNhanVien == assignment.MaNhanVien 
                                 && p.NgayBatDau <= DateTime.Now 
                                 && (p.NgayKetThuc == null || p.NgayKetThuc >= DateTime.Now));

        if (activeRoute == null)
        {
            TempData["ErrorMessage"] = "Lỗi: Bạn chưa được phân công tuyến giao hàng nào cho ca làm việc hiện tại.";
            return RedirectToAction(nameof(MyDeliveries));
        }

        // 2. Tìm vị trí lưu kho của Đơn hàng trong bảng NhapKho
        var storageRecord = await _context.NhapKhos
            .Where(q => q.MaDonHang == assignment.MaDonHang && q.TrangThaiKho == "Đã nhập kho")
            .OrderByDescending(q => q.ThoiGianNhap)
            .FirstOrDefaultAsync();

        if (storageRecord == null || string.IsNullOrEmpty(storageRecord.ViTriLuuTru))
        {
            TempData["ErrorMessage"] = "Lỗi: Không tìm thấy thông tin vị trí lưu trữ (kệ hàng) của đơn hàng này.";
            return RedirectToAction(nameof(MyDeliveries));
        }

        // 3. Nghiệp vụ: Đối chiếu Kệ hàng (ViTriLuuTru) hoặc Địa chỉ giao nhận với Tuyến giao của Shipper (Case-insensitive)
        string viTriKhu = storageRecord.ViTriLuuTru?.ToLower() ?? "";
        string tenTuyen = activeRoute.TuyenGiao.TenTuyen.ToLower();
        string diaChiGiao = assignment.DonHang?.DiaChiNguoiNhan?.ToLower() ?? "";

        if (!viTriKhu.Contains(tenTuyen) && !diaChiGiao.Contains(tenTuyen))
        {
            TempData["ErrorMessage"] = $"Lỗi: Đơn hàng này đang nằm tại '{storageRecord.ViTriLuuTru}' và giao đến '{assignment.DonHang?.DiaChiNguoiNhan}', không thuộc Tuyến '{activeRoute.TuyenGiao.TenTuyen}' của bạn!";
            return RedirectToAction(nameof(MyDeliveries));
        }

        // 4. Nếu hợp lệ, tiến hành tiếp nhận đơn giao hàng
        assignment.TrangThai = "Đang giao";
        assignment.ViTriHienTai = storageRecord.ViTriLuuTru;
        if (assignment.DonHang != null)
        {
            assignment.DonHang.TrangThaiDonHang = "Đang giao hàng";
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Tiếp nhận đơn hàng thành công!";
        return RedirectToAction(nameof(MyDeliveries));
    }

    public async Task<IActionResult> UpdateStatus(string id)
    {
        var delivery = await _context.HanhTrinhDonHangs
            .Include(p => p.DonHang)
            .FirstOrDefaultAsync(p => p.MaHanhTrinh == id);
        return View(delivery);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelivery(string maHanhTrinh, bool isSuccess, string note, decimal codAmount, string viTriHienTai, string hinhAnhThucTe)
    {
        var assignment = await _context.HanhTrinhDonHangs
            .Include(p => p.DonHang)
            .FirstOrDefaultAsync(p => p.MaHanhTrinh == maHanhTrinh);

        if (assignment == null) return NotFound();

        // Business validation: Failed deliveries MUST have note and a proof image
        if (!isSuccess)
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                TempData["ErrorMessage"] = "Lỗi: Bạn phải nhập Lý do khi giao hàng thất bại!";
                return RedirectToAction(nameof(UpdateStatus), new { id = maHanhTrinh });
            }
            if (string.IsNullOrWhiteSpace(hinhAnhThucTe) || hinhAnhThucTe == "/images/proof_placeholder.jpg" || hinhAnhThucTe.Contains("placeholder"))
            {
                TempData["ErrorMessage"] = "Lỗi: Bạn phải cung cấp Ảnh minh chứng thực tế khi giao hàng thất bại!";
                return RedirectToAction(nameof(UpdateStatus), new { id = maHanhTrinh });
            }
        }

        assignment.TrangThai = isSuccess ? "Giao hàng thành công" : "Giao hàng thất bại";
        assignment.ThoiGianHoanThanh = DateTime.Now;
        assignment.LyDoThatBai = isSuccess ? null : note;
        assignment.ViTriHienTai = string.IsNullOrWhiteSpace(viTriHienTai) ? "Địa chỉ người nhận" : viTriHienTai;
        assignment.HinhAnhThucTe = string.IsNullOrWhiteSpace(hinhAnhThucTe) ? "/images/proof_placeholder.jpg" : hinhAnhThucTe;

        if (assignment.DonHang != null)
        {
            assignment.DonHang.TrangThaiDonHang = assignment.TrangThai;
            assignment.DonHang.NgayHoanThanh = DateTime.Now;
        }

        // Add Payment record if COD
        if (isSuccess && codAmount > 0)
        {
            var payment = new ThanhToan
            {
                MaThanhToan = "PAY" + DateTime.Now.Ticks.ToString().Substring(10),
                MaDonHang = assignment.MaDonHang,
                MaShipper = assignment.MaNhanVien,
                SoTienThanhToan = codAmount,
                PhuongThucThanhToan = "Tiền mặt (COD)",
                ThoiGianThanhToan = DateTime.Now,
                TrangThaiThanhToan = "Đã thu hộ"
            };
            _context.ThanhToans.Add(payment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(MyDeliveries));
    }
}
