using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageResizer.Infrastructure.DatabaseContext
{
    public class ResizerDbContext(DbContextOptions<ResizerDbContext> options) :
        IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IResizerDbContext
    {
        public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<FileUpload> FileUploads { get; set; }

        public DbSet<Thumbnail> Thumbnails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
