using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public class ProjectDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NewsTag> NewsTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ánh xạ bảng Accounts thành AspNetUsers
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Accounts");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Email).HasColumnName("Email");
            entity.Property(e => e.PasswordHash).HasColumnName("Password");
        });

        // Ánh xạ bảng Roles thành AspNetRoles
        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasColumnName("Name");
        });

        // Ánh xạ bảng News
        modelBuilder.Entity<News>(entity =>
        {
            entity.ToTable("News");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Title).HasColumnName("Title");
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.Content).HasColumnName("Content");
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
            entity.Property(e => e.CreateBy).HasColumnName("CreateBy");
            entity.Property(e => e.Disable).HasColumnName("Disable").HasDefaultValue(false);

            entity.HasOne(n => n.Category)
                  .WithMany()
                  .HasForeignKey(n => n.CategoryId);

            entity.HasOne(n => n.CreatedBy)
                  .WithMany()
                  .HasForeignKey(n => n.CreateBy);
        });

        // Ánh xạ bảng Categories
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasColumnName("Name");
        });

        // Ánh xạ bảng Tags
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasColumnName("Name");
        });

        // Ánh xạ bảng NewsTags
        modelBuilder.Entity<NewsTag>(entity =>
        {
            entity.ToTable("NewsTags");
            entity.HasKey(nt => new { nt.Id_Tags, nt.Id_News });
            entity.Property(nt => nt.Id_Tags).HasColumnName("Id_Tags");
            entity.Property(nt => nt.Id_News).HasColumnName("Id_News");

            entity.HasOne(nt => nt.Tag)
                  .WithMany(t => t.NewsTags)
                  .HasForeignKey(nt => nt.Id_Tags)
                  .HasConstraintName("FK_NewsTags_Tags");

            entity.HasOne(nt => nt.News)
                  .WithMany(n => n.NewsTags)
                  .HasForeignKey(nt => nt.Id_News)
                  .HasConstraintName("FK_NewsTags_News");
        });
    }
}