using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRM.Identity.Models;
using Microsoft.AspNetCore.Identity;
using IdentityModel;
using IdentityServer4.Models;

namespace CRM.Identity.Data
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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            SeedData(builder);
        }

        private static void SeedData(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "admin", NormalizedName = "admin" });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "guest", NormalizedName = "guest" });

            var adminUser = new ApplicationUser()
            {
                UserName = "admin@nomail.com",
                Email = "admin@nomail.com",
                EmailConfirmed = true,
                NormalizedEmail = "admin@nomail.com",
                NormalizedUserName = "admin@nomail.com",
            };
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "P@ssw0rd");
            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>()
            {
                Id = 1,
                UserId = adminUser.Id,
                ClaimType = JwtClaimTypes.Email,
                ClaimValue = "admin@nomail.com"
            });
        }
    }
}
