using System;
using System.Collections.Generic;

namespace QuanLyKhachSan.Models;

public partial class Nhanvien
{
    public string IdNv { get; set; } = null!;

    public string HoTenNv { get; set; } = null!;

    public DateOnly NgaySinhNv { get; set; }

    public string Cccd { get; set; } = null!;

    public string SdtNv { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string ChucVu { get; set; } = null!;

    public string? Email { get; set; }

    public string? Pass { get; set; }

    public virtual ICollection<Phieuthuephong> Phieuthuephongs { get; set; } = new List<Phieuthuephong>();
}
