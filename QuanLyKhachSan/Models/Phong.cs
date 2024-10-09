using System;
using System.Collections.Generic;

namespace QuanLyKhachSan.Models;

public partial class Phong
{
    public int IdP { get; set; }

    public string? IdLp { get; set; }

    public string TenPhong { get; set; } = null!;

    public int? GiaP { get; set; }

    public string ChiTietP { get; set; } = null!;

    public bool TinhTrang { get; set; } = false;

    public virtual Loaiphong? IdLpNavigation { get; set; }

    public virtual ICollection<Phieuthuephong> Phieuthuephongs { get; set; } = new List<Phieuthuephong>();
}
