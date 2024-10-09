using System;
using System.Collections.Generic;

namespace QuanLyKhachSan.Models;

public partial class Loaiphong
{
    public string IdLp { get; set; } = null!;

    public string TenLp { get; set; } = null!;

    public string GiaLp { get; set; } = null!;

    public string MoTaLp { get; set; } = null!;

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
