using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Transactions;

namespace ImageResizer.Infrastructure.Transactions
{
    public class ResizerTransactionExecutor(IResizerDbContext resizerDbContext) : IResizerTransactionExecutor
    {
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var transaction = await resizerDbContext.Database.BeginTransactionAsync();

            try
            {
                await action();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
