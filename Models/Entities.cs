using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SDMS.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Address { get; set; }
}

[Table("NhomHang")]
public class NhomHang
{
    [Key]
    [StringLength(20)]
    public string MaNhomHang { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenNhomHang { get; set; } = string.Empty;

    [StringLength(200)]
    public string? MoTa { get; set; }

    public ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}

[Table("HangHoa")]
public class HangHoa
{
    [Key]
    [StringLength(20)]
    public string MaHangHoa { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaNhomHang { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenHangHoa { get; set; } = string.Empty;

    [StringLength(50)]
    public string? DonViTinh { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal KhoiLuong { get; set; }

    [StringLength(100)]
    public string? KichThuoc { get; set; }

    [StringLength(255)]
    public string? MoTaChiTiet { get; set; }

    [ForeignKey("MaNhomHang")]
    public NhomHang? NhomHang { get; set; }

    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
}

[Table("KhachHang")]
public class KhachHang
{
    [Key]
    [StringLength(20)]
    public string MaKhachHang { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string SoDienThoai { get; set; } = string.Empty;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public string? UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}

[Table("DonHang")]
public class DonHang
{
    [Key]
    [StringLength(20)]
    public string MaDonHang { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaKhachHang { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenNguoiNhan { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string SoDienThoaiNguoiNhan { get; set; } = string.Empty;

    [StringLength(255)]
    public string? DiaChiNguoiNhan { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TongKhoiLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PhiGiaoHang { get; set; }

    [StringLength(50)]
    public string? HinhThucThanhToan { get; set; }

    [StringLength(50)]
    public string TrangThaiDonHang { get; set; } = "Mới tạo";

    public DateTime? NgayGiaoDuKien { get; set; }
    public DateTime? NgayHoanThanh { get; set; }
    public DateTime ThoiGianTao { get; set; } = DateTime.Now;

    [ForeignKey("MaKhachHang")]
    public KhachHang? KhachHang { get; set; }

    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public ICollection<NhapKho> NhapKhos { get; set; } = new List<NhapKho>();
    public ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    public ICollection<HanhTrinhDonHang> HanhTrinhDonHangs { get; set; } = new List<HanhTrinhDonHang>();
}

[Table("ChiTietDonHang")]
public class ChiTietDonHang
{
    [StringLength(20)]
    public string MaHangHoa { get; set; } = string.Empty;

    [StringLength(20)]
    public string MaDonHang { get; set; } = string.Empty;

    public int SoLuong { get; set; }

    [StringLength(255)]
    public string? TinhTrangHangHoa { get; set; }

    [ForeignKey("MaHangHoa")]
    public HangHoa? HangHoa { get; set; }

    [ForeignKey("MaDonHang")]
    public DonHang? DonHang { get; set; }
}

[Table("KhoHang")]
public class KhoHang
{
    [Key]
    [StringLength(20)]
    public string MaKhoHang { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenKho { get; set; } = string.Empty;

    [StringLength(255)]
    public string? DiaChiKho { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal DienTichKho { get; set; }

    public int SucChuaKho { get; set; }

    [Required]
    [StringLength(50)]
    public string TrangThai { get; set; } = "Hoạt động";

    public ICollection<NhapKho> NhapKhos { get; set; } = new List<NhapKho>();
}

[Table("NhapKho")]
public class NhapKho
{
    [Key]
    [StringLength(20)]
    public string MaNhapKho { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaDonHang { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaKhoHang { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    public DateTime ThoiGianNhap { get; set; } = DateTime.Now;

    [StringLength(100)]
    public string? ViTriLuuTru { get; set; }

    [StringLength(100)]
    public string? TrangThaiKho { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? KhoiLuongThucTe { get; set; }

    public int? SoLuongKienHang { get; set; }

    [StringLength(255)]
    public string? TinhTrangDonHang { get; set; }

    [ForeignKey("MaDonHang")]
    public DonHang? DonHang { get; set; }

    [ForeignKey("MaKhoHang")]
    public KhoHang? KhoHang { get; set; }

    [ForeignKey("MaNhanVien")]
    public NhanVien? NhanVien { get; set; }
}

[Table("NhanVien")]
public class NhanVien
{
    [Key]
    [StringLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    public DateTime NgaySinh { get; set; }

    [Required]
    [StringLength(50)]
    public string ChucVu { get; set; } = string.Empty;

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(150)]
    public string? TrangThaiLamViec { get; set; }

    [Required]
    [StringLength(255)]
    public string MatKhau { get; set; } = "123456";

    public string? UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    public ICollection<NhapKho> NhapKhos { get; set; } = new List<NhapKho>();
    public ICollection<HanhTrinhDonHang> HanhTrinhDonHangs { get; set; } = new List<HanhTrinhDonHang>();
    public ICollection<NhatKyHeThong> NhatKyHeThongs { get; set; } = new List<NhatKyHeThong>();
    public ICollection<PhanCongCa> PhanCongCas { get; set; } = new List<PhanCongCa>();
}

[Table("HanhTrinhDonHang")]
public class HanhTrinhDonHang
{
    [Key]
    [StringLength(20)]
    public string MaHanhTrinh { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaDonHang { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    public DateTime ThoiGianTiepNhan { get; set; } = DateTime.Now;
    public DateTime? ThoiGianHoanThanh { get; set; }

    [Required]
    [StringLength(50)]
    public string TrangThai { get; set; } = "Chờ shipper lấy";

    [StringLength(255)]
    public string? ViTriHienTai { get; set; }

    [StringLength(255)]
    public string? LyDoThatBai { get; set; }

    [StringLength(255)]
    public string? HinhAnhThucTe { get; set; }

    [ForeignKey("MaDonHang")]
    public DonHang? DonHang { get; set; }

    [ForeignKey("MaNhanVien")]
    public NhanVien? NhanVien { get; set; }
}

[Table("ThanhToan")]
public class ThanhToan
{
    [Key]
    [StringLength(20)]
    public string MaThanhToan { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaDonHang { get; set; } = string.Empty;

    [StringLength(20)]
    public string? MaShipper { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SoTienThanhToan { get; set; }

    [StringLength(50)]
    public string? PhuongThucThanhToan { get; set; }

    public DateTime ThoiGianThanhToan { get; set; } = DateTime.Now;

    [StringLength(50)]
    public string? TrangThaiThanhToan { get; set; }

    [ForeignKey("MaDonHang")]
    public DonHang? DonHang { get; set; }
}

[Table("NhatKyHeThong")]
public class NhatKyHeThong
{
    [Key]
    [StringLength(20)]
    public string MaNhatKy { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string HanhDong { get; set; } = string.Empty;

    [StringLength(100)]
    public string? DuLieuTacDong { get; set; }

    public DateTime ThoiGian { get; set; } = DateTime.Now;

    [ForeignKey("MaNhanVien")]
    public NhanVien? NhanVien { get; set; }
}

[Table("TuyenGiao")]
public class TuyenGiao
{
    [Key]
    [StringLength(20)]
    public string MaTuyen { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenTuyen { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string KhuVuc { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public ICollection<PhanCongTuyen> PhanCongTuyens { get; set; } = new List<PhanCongTuyen>();
}

[Table("PhanCongTuyen")]
public class PhanCongTuyen
{
    [Key]
    [StringLength(20)]
    public string MaPhanCongTuyen { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaTuyen { get; set; } = string.Empty;

    public DateTime NgayBatDau { get; set; } = DateTime.Now;
    public DateTime? NgayKetThuc { get; set; }

    [ForeignKey("MaNhanVien")]
    public NhanVien? NhanVien { get; set; }

    [ForeignKey("MaTuyen")]
    public TuyenGiao? TuyenGiao { get; set; }
}

[Table("CaLamViec")]
public class CaLamViec
{
    [Key]
    [StringLength(20)]
    public string MaCa { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenCa { get; set; } = string.Empty;

    [Required]
    public DateTime GioBatDau { get; set; }

    [Required]
    public DateTime GioKetThuc { get; set; }

    public ICollection<PhanCongCa> PhanCongCas { get; set; } = new List<PhanCongCa>();
}

[Table("PhanCongCa")]
public class PhanCongCa
{
    [Key]
    [StringLength(20)]
    public string MaPhanCongCa { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaCa { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaNhanVien { get; set; } = string.Empty;

    [Required]
    public DateTime NgayLam { get; set; }

    [Required]
    [StringLength(100)]
    public string TrangThai { get; set; } = string.Empty;

    public DateTime? GioVaoThucTe { get; set; }

    [ForeignKey("MaCa")]
    public CaLamViec? CaLamViec { get; set; }

    [ForeignKey("MaNhanVien")]
    public NhanVien? NhanVien { get; set; }
}
