namespace ImageResizer.Domain.Interfaces.Transactions
{
    public interface IResizerTransactionExecutor
    {
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
