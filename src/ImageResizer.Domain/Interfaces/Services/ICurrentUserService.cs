namespace ImageResizer.Domain.Interfaces.Services
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }

        Guid UserId { get; }
        
        string EmailAddress { get; }
    }
}
