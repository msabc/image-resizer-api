using ImageResizer.Domain.Commands;
using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Models.Tables;

namespace ImageResizer.Infrastructure.Repositories
{
    public class ImageRepository(IResizerDbContext resizerDbContext) : IImageRepository
    {
        public async Task AddAsync(AddImageCommand command)
        {
            Image image = new()
            {
                Url = command.Url
            };

            await resizerDbContext.Images.AddAsync(image);

            await resizerDbContext.SaveChangesAsync();
        }
    }
}
