using System;
using System.Collections.Generic;

namespace QuanLyKhachSan.Models;

public partial class Khachhang
{
    public string IdKh { get; set; } = null!;

    public string HoTenKh { get; set; } = null!;

    public DateOnly NgaySinhKh { get; set; }

    public string SoCccd { get; set; } = null!;

    public string SdtKh { get; set; } = null!;

    public string EmailKh { get; set; } = null!;

    public string DiaChiKh { get; set; } = null!;

    public virtual ICollection<Phieuthuephong> Phieuthuephongs { get; set; } = new List<Phieuthuephong>();
}
