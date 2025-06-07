using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoOData.Models;

public partial class DemoOdataContext : DbContext
{
    public DemoOdataContext()
    {
    }

    public DemoOdataContext(DbContextOptions<DemoOdataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-BI4NRT5;database=DemoODATA;uid=sa;pwd=123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0771466126");

            entity.Property(e => e.Brand).IsUnicode(false);
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImageName)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductName).IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(1238)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
