using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLyKhachSan.Models;

public partial class QuanLyKhachSanContext : DbContext
{
    public QuanLyKhachSanContext()
    {
    }

    public QuanLyKhachSanContext(DbContextOptions<QuanLyKhachSanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Loaiphong> Loaiphongs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Phieuthuephong> Phieuthuephongs { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=THUHIEN\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.IdHd).HasName("PK__HOADON__8B62D721263C2746");

            entity.ToTable("HOADON");

            entity.Property(e => e.IdHd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_HD");
            entity.Property(e => e.GhiChu).HasMaxLength(100);
            entity.Property(e => e.IdPtp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_PTP");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdPtpNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.IdPtp)
                .HasConstraintName("FK__HOADON__ID_PTP__6B24EA82");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.IdKh).HasName("PK__KHACHHAN__8B62EC89B46CA893");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.IdKh)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_KH");
            entity.Property(e => e.DiaChiKh)
                .HasMaxLength(50)
                .HasColumnName("DiaChiKH");
            entity.Property(e => e.EmailKh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Email_KH");
            entity.Property(e => e.HoTenKh)
                .HasMaxLength(100)
                .HasColumnName("HoTen_KH");
            entity.Property(e => e.NgaySinhKh).HasColumnName("NgaySinh_KH");
            entity.Property(e => e.SdtKh)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("Sdt_KH");
            entity.Property(e => e.SoCccd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SoCCCD");
        });

        modelBuilder.Entity<Loaiphong>(entity =>
        {
            entity.HasKey(e => e.IdLp).HasName("PK__LOAIPHON__8B62F4B024C448FB");

            entity.ToTable("LOAIPHONG");

            entity.Property(e => e.IdLp)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_LP");
            entity.Property(e => e.GiaLp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Gia_LP");
            entity.Property(e => e.MoTaLp)
                .HasMaxLength(100)
                .HasColumnName("MoTa_LP");
            entity.Property(e => e.TenLp)
                .HasMaxLength(100)
                .HasColumnName("Ten_LP");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.IdNv).HasName("PK__NHANVIEN__8B63E063B3B39303");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.IdNv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_NV");
            entity.Property(e => e.Cccd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CCCD");
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HoTenNv)
                .HasMaxLength(100)
                .HasColumnName("HoTen_NV");
            entity.Property(e => e.NgaySinhNv).HasColumnName("NgaySinh_NV");
            entity.Property(e => e.Pass)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SdtNv)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("Sdt_NV");
        });

        modelBuilder.Entity<Phieuthuephong>(entity =>
        {
            entity.HasKey(e => e.IdPtp).HasName("PK__PHIEUTHU__20AF893EFEDF47FA");

            entity.ToTable("PHIEUTHUEPHONG");

            entity.Property(e => e.IdPtp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_PTP");
            entity.Property(e => e.IdKh)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_KH");
            entity.Property(e => e.IdNv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_NV");
            entity.Property(e => e.IdP).HasColumnName("ID_P");
            entity.Property(e => e.TongTienPhong).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.Phieuthuephongs)
                .HasForeignKey(d => d.IdKh)
                .HasConstraintName("FK__PHIEUTHUE__ID_KH__66603565");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.Phieuthuephongs)
                .HasForeignKey(d => d.IdNv)
                .HasConstraintName("FK__PHIEUTHUE__ID_NV__68487DD7");

            entity.HasOne(d => d.IdPNavigation).WithMany(p => p.Phieuthuephongs)
                .HasForeignKey(d => d.IdP)
                .HasConstraintName("FK__PHIEUTHUEP__ID_P__6754599E");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.IdP).HasName("PK__PHONG__B87EA51C6E3C79BA");

            entity.ToTable("PHONG");

            entity.Property(e => e.IdP)
                .ValueGeneratedNever()
                .HasColumnName("ID_P");
            entity.Property(e => e.ChiTietP)
                .HasMaxLength(100)
                .HasColumnName("ChiTiet_P");
            entity.Property(e => e.GiaP).HasColumnName("Gia_P");
            entity.Property(e => e.IdLp)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID_LP");
            entity.Property(e => e.TenPhong)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TinhTrang).HasDefaultValue(true);

            entity.HasOne(d => d.IdLpNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.IdLp)
                .HasConstraintName("FK__PHONG__ID_LP__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
