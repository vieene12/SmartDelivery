using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SDMS.Data;
using SDMS.Models;
using System.Diagnostics;

namespace SDMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("WarehouseStaff"))
                {
                    return RedirectToAction("Index", "Warehouse");
                }
                else if (User.IsInRole("Shipper"))
                {
                    return RedirectToAction("Index", "Shipper");
                }
                else if (User.IsInRole("Customer"))
                {
                    return RedirectToAction("Orders", "Customer");
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Pricing(string tab)
        {
            ViewBag.ActiveTab = tab ?? "hcm-hn";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var customer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.UserId == userId);
            var staff = await _context.NhanViens.FirstOrDefaultAsync(n => n.UserId == userId);

            ViewBag.Customer = customer;
            ViewBag.Staff = staff;

            return View(user);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string fullName, string email, string phoneNumber, string address, string birthDate, string taxId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.FullName = fullName ?? string.Empty;
            user.Email = email;
            user.NormalizedEmail = email?.ToUpper();
            user.PhoneNumber = phoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Also update customer profile if exists
                var customer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.UserId == userId);
                if (customer != null)
                {
                    customer.HoTen = fullName ?? string.Empty;
                    customer.SoDienThoai = phoneNumber ?? string.Empty;
                    customer.Email = email;
                    customer.DiaChi = address;
                }

                // Also update staff profile if exists
                var staff = await _context.NhanViens.FirstOrDefaultAsync(n => n.UserId == userId);
                if (staff != null)
                {
                    staff.HoTen = fullName ?? string.Empty;
                    staff.SoDienThoai = phoneNumber;
                    staff.Email = email;
                    staff.DiaChi = address;
                    if (DateTime.TryParse(birthDate, out DateTime dob))
                    {
                        staff.NgaySinh = dob;
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thông tin cấu hình tài khoản thành công!";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Profile));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
