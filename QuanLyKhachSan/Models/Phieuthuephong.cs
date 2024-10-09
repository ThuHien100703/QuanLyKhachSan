using System;
using System.Collections.Generic;

namespace QuanLyKhachSan.Models;

public partial class Phieuthuephong
{
    public string IdPtp { get; set; } = null!;

    public string? IdKh { get; set; }

    public int? IdP { get; set; }

    public string? IdNv { get; set; }

    public DateOnly NgayThuePhong { get; set; }

    public DateOnly NgayTraPhong { get; set; }

    public decimal TongTienPhong { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual Khachhang? IdKhNavigation { get; set; }

    public virtual Nhanvien? IdNvNavigation { get; set; }

    public virtual Phong? IdPNavigation { get; set; }
}
