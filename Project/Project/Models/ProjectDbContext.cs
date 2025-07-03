using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class ProjectDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
                : base(options)
        {
        }

        // DbSet cho các bảng 
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Đặt tên bảng để khớp với database 
            builder.Entity<ApplicationUser>().ToTable("Accounts");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            // Cấu hình khóa chính cho NewsTags
            builder.Entity<NewsTag>()
                .HasKey(nt => new { nt.Id_Tags, nt.Id_News });

            // Cấu hình quan hệ cho News
            builder.Entity<News>()
                .HasOne(n => n.Category)
                .WithMany()
                .HasForeignKey(n => n.CategoryId);

            builder.Entity<News>()
                .HasOne(n => n.CreateByUser)
                .WithMany()
                .HasForeignKey(n => n.CreateBy);

            // Cấu hình quan hệ cho NewsTags
            builder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany()
                .HasForeignKey(nt => nt.Id_Tags);

            builder.Entity<NewsTag>()
                .HasOne(nt => nt.News)
                .WithMany()
                .HasForeignKey(nt => nt.Id_News);
        }
    }

}
