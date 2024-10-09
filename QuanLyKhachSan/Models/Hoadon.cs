using System;
using System.Collections.Generic;

namespace QuanLyKhachSan.Models;

public partial class Hoadon
{
    public string IdHd { get; set; } = null!;

    public string? IdPtp { get; set; }

    public decimal ThanhTien { get; set; }

    public DateOnly NgayThanhToan { get; set; }

    public string? GhiChu { get; set; }

    public virtual Phieuthuephong? IdPtpNavigation { get; set; }
}
