using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Weather_App.Models;

namespace Weather_App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        [ExcludeFromCodeCoverage]
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Favorite> Favorites { get; set; }
    }
}
