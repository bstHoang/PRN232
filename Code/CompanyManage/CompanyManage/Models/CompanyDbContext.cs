using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyManage.Models
{
    public class CompanyDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public CompanyDbContext()
        {
        }
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
       : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dữ liệu mẫu cho Department
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "Board of Directors" },
                new Department { Id = 3, Name = "IT" }
            );

            // Dữ liệu mẫu cho Position
            modelBuilder.Entity<Position>().HasData(
                new Position { Id = 1, Name = "Manager" },
                new Position { Id = 2, Name = "CEO" },
                new Position { Id = 3, Name = "Employee" }
            );

            // Dữ liệu mẫu cho Role
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" }
            );

            // Dữ liệu mẫu cho ApplicationUser
            modelBuilder.Entity<ApplicationUser>().HasData(
                // Tài khoản 1: Role = Admin
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@company.com",
                    NormalizedEmail = "ADMIN@COMPANY.COM",
                    Name = "Admin User",
                    DepartmentId = 2, // Board of Directors
                    PositionId = 2,   // CEO
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Admin123!"),
                    SecurityStamp = Guid.NewGuid().ToString() // Thêm SecurityStamp
                },
                // Tài khoản 2: Role = User, Department = HR, Position = Manager
                new ApplicationUser
                {
                    Id = 2,
                    UserName = "hrmanager",
                    NormalizedUserName = "HRMANAGER",
                    Email = "hrmanager@company.com",
                    NormalizedEmail = "HRMANAGER@COMPANY.COM",
                    Name = "HR Manager",
                    DepartmentId = 1, // HR
                    PositionId = 1,   // Manager
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "HRManager123!"),
                    SecurityStamp = Guid.NewGuid().ToString() // Thêm SecurityStamp
                },
                // Tài khoản 3: Role = User, Department = HR, Position = Employee
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "hremployee",
                    NormalizedUserName = "HREMPLOYEE",
                    Email = "hremployee@company.com",
                    NormalizedEmail = "HREMPLOYEE@COMPANY.COM",
                    Name = "HR Employee",
                    DepartmentId = 1, // HR
                    PositionId = 3,   // Employee
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "HREmployee123!"),
                    SecurityStamp = Guid.NewGuid().ToString() // Thêm SecurityStamp
                }
            );

            // Gán vai trò cho các tài khoản
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 }, // Admin
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }, // User (HR Manager)
                new IdentityUserRole<int> { UserId = 3, RoleId = 2 }  // User (HR Employee)
            );
        }
    }
}