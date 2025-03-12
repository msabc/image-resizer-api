using ImageResizer.Domain.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ImageResizer.Domain.Interfaces.DatabaseContext
{
    public interface IResizerDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<FileUpload> FileUploads { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
