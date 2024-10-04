using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { 
                    Id = "fdf41afb-4841-43b9-8642-b32179934a49",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole {
                    Id = "0428efdd-bbf9-44fa-bba3-0a1873af0e56",
                    Name = "Librarian",
                    NormalizedName = "LIBRARIAN"
                },
                new IdentityRole {
                    Id = "b117d060-6194-4e16-8c49-f60bbf42ec3e",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                UserName = "admin@admin.com",
                PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                EmailConfirmed = true,
                FirstName = "Admideus",
                LastName = "Admin",
                DateOfBirth = new DateOnly(1991,01,01)
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "b117d060-6194-4e16-8c49-f60bbf42ec3e",
                UserId = "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf"
            });
        }
        public DbSet<LibraryWebApp.Models.Book> Book { get; set; } = default!;
        public DbSet<LendingHistory> LendingHistory { get; set; }
        public DbSet<WaitingList> WaitingList { get; set; }
    }
}
