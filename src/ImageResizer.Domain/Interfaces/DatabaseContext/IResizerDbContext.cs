using ImageResizer.Domain.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace ImageResizer.Domain.Interfaces.DatabaseContext
{
    public interface IResizerDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<Image> Images { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
