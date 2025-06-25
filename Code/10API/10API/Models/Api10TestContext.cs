using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _10API.Models;

public partial class Api10TestContext : DbContext
{
    public Api10TestContext()
    {
    }

    public Api10TestContext(DbContextOptions<Api10TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetService> PetServices { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC25EB46539");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(255);

            entity.HasOne(d => d.Pet).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK__Appointme__PetId__5441852A");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owner__819385B8761D0971");

            entity.ToTable("Owner");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__Pet__48E53862774F771E");

            entity.ToTable("Pet");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.Owner).WithMany(p => p.Pets)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Pet__OwnerId__4BAC3F29");
        });

        modelBuilder.Entity<PetService>(entity =>
        {
            entity.HasKey(e => new { e.PetId, e.ServiceId, e.ServiceDate }).HasName("PK__PetServi__823F2E93F731BBE9");

            entity.ToTable("PetService");

            entity.Property(e => e.Note).HasMaxLength(255);

            entity.HasOne(d => d.Pet).WithMany(p => p.PetServices)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PetServic__PetId__5070F446");

            entity.HasOne(d => d.Service).WithMany(p => p.PetServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PetServic__Servi__5165187F");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB00A5570078A");

            entity.ToTable("Service");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
