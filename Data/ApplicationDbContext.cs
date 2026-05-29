    using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SDMS.Models;

namespace SDMS.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NhomHang> NhomHangs { get; set; }
    public DbSet<HangHoa> HangHoas { get; set; }
    public DbSet<KhachHang> KhachHangs { get; set; }
    public DbSet<DonHang> DonHangs { get; set; }
    public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
    public DbSet<KhoHang> KhoHangs { get; set; }
    public DbSet<NhapKho> NhapKhos { get; set; }
    public DbSet<NhanVien> NhanViens { get; set; }
    public DbSet<HanhTrinhDonHang> HanhTrinhDonHangs { get; set; }
    public DbSet<ThanhToan> ThanhToans { get; set; }
    public DbSet<NhatKyHeThong> NhatKyHeThongs { get; set; }
    public DbSet<TuyenGiao> TuyenGiaos { get; set; }
    public DbSet<PhanCongTuyen> PhanCongTuyens { get; set; }
    public DbSet<CaLamViec> CaLamViecs { get; set; }
    public DbSet<PhanCongCa> PhanCongCas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Many-to-Many: DonHang and HangHoa via ChiTietDonHang
        builder.Entity<ChiTietDonHang>()
            .HasKey(c => new { c.MaHangHoa, c.MaDonHang });

        builder.Entity<ChiTietDonHang>()
            .HasOne(c => c.HangHoa)
            .WithMany(h => h.ChiTietDonHangs)
            .HasForeignKey(c => c.MaHangHoa);

        builder.Entity<ChiTietDonHang>()
            .HasOne(c => c.DonHang)
            .WithMany(d => d.ChiTietDonHangs)
            .HasForeignKey(c => c.MaDonHang);

        // Decimal Precision
        builder.Entity<HangHoa>()
            .Property(h => h.KhoiLuong)
            .HasPrecision(10, 2);

        builder.Entity<DonHang>()
            .Property(d => d.TongKhoiLuong)
            .HasPrecision(10, 2);

        builder.Entity<DonHang>()
            .Property(d => d.PhiGiaoHang)
            .HasPrecision(18, 2);

        builder.Entity<KhoHang>()
            .Property(k => k.DienTichKho)
            .HasPrecision(10, 2);

        builder.Entity<ThanhToan>()
            .Property(t => t.SoTienThanhToan)
            .HasPrecision(18, 2);

        builder.Entity<NhapKho>()
            .Property(q => q.KhoiLuongThucTe)
            .HasPrecision(10, 2);

        // Relationships and Cascade Rules
        builder.Entity<NhapKho>()
            .HasOne(q => q.DonHang)
            .WithMany(d => d.NhapKhos)
            .HasForeignKey(q => q.MaDonHang)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<HanhTrinhDonHang>()
            .HasOne(h => h.DonHang)
            .WithMany(d => d.HanhTrinhDonHangs)
            .HasForeignKey(h => h.MaDonHang)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.Entity<ThanhToan>()
            .HasOne(t => t.DonHang)
            .WithMany(d => d.ThanhToans)
            .HasForeignKey(t => t.MaDonHang)
            .OnDelete(DeleteBehavior.Restrict);

        // Rename Identity Tables
        builder.Entity<ApplicationUser>().ToTable("NguoiDung");
        builder.Entity<IdentityRole>().ToTable("VaiTro");
        builder.Entity<IdentityUserRole<string>>().ToTable("NguoiDung_VaiTro");
        builder.Entity<IdentityUserClaim<string>>().ToTable("NguoiDung_Claim");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("VaiTro_Claim");
        builder.Entity<IdentityUserLogin<string>>().ToTable("NguoiDung_DangNhap");
        builder.Entity<IdentityUserToken<string>>().ToTable("NguoiDung_Token");

        // Rename Identity Columns for NguoiDung (AspNetUsers)
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.Id).HasColumnName("MaNguoiDung");
            entity.Property(u => u.FullName).HasColumnName("HoTen");
            entity.Property(u => u.Address).HasColumnName("DiaChi");
            entity.Property(u => u.UserName).HasColumnName("TenDangNhap");
            entity.Property(u => u.NormalizedUserName).HasColumnName("TenDangNhapChuanHoa");
            entity.Property(u => u.Email).HasColumnName("Email");
            entity.Property(u => u.NormalizedEmail).HasColumnName("EmailChuanHoa");
            entity.Property(u => u.EmailConfirmed).HasColumnName("XacNhanEmail");
            entity.Property(u => u.PasswordHash).HasColumnName("MatKhauHash");
            entity.Property(u => u.SecurityStamp).HasColumnName("DauBanMat");
            entity.Property(u => u.ConcurrencyStamp).HasColumnName("DauDongThoi");
            entity.Property(u => u.PhoneNumber).HasColumnName("SoDienThoai");
            entity.Property(u => u.PhoneNumberConfirmed).HasColumnName("XacNhanSoDienThoai");
            entity.Property(u => u.TwoFactorEnabled).HasColumnName("KichHoatHaiLop");
            entity.Property(u => u.LockoutEnd).HasColumnName("ThoiGianKhoa");
            entity.Property(u => u.LockoutEnabled).HasColumnName("ChoPhepKhoa");
            entity.Property(u => u.AccessFailedCount).HasColumnName("SoLanDangNhapSai");
        });

        // Rename Identity Columns for VaiTro (AspNetRoles)
        builder.Entity<IdentityRole>(entity =>
        {
            entity.Property(r => r.Id).HasColumnName("MaVaiTro");
            entity.Property(r => r.Name).HasColumnName("TenVaiTro");
            entity.Property(r => r.NormalizedName).HasColumnName("TenVaiTroChuanHoa");
            entity.Property(r => r.ConcurrencyStamp).HasColumnName("DauDongThoi");
        });

        // Config PhanCongTuyen
        builder.Entity<PhanCongTuyen>()
            .HasOne(p => p.NhanVien)
            .WithMany()
            .HasForeignKey(p => p.MaNhanVien)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PhanCongTuyen>()
            .HasOne(p => p.TuyenGiao)
            .WithMany(t => t.PhanCongTuyens)
            .HasForeignKey(p => p.MaTuyen)
            .OnDelete(DeleteBehavior.Cascade);

        // Config PhanCongCa
        builder.Entity<PhanCongCa>()
            .HasOne(p => p.CaLamViec)
            .WithMany(c => c.PhanCongCas)
            .HasForeignKey(p => p.MaCa)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PhanCongCa>()
            .HasOne(p => p.NhanVien)
            .WithMany(n => n.PhanCongCas)
            .HasForeignKey(p => p.MaNhanVien)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
