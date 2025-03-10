namespace Solarnelle.Domain.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message, string methodName, string repositoryName, Exception innerException) : base(message, innerException)
        {
            MethodName = methodName;
            RepositoryName = repositoryName;
        }

        public DatabaseException(string message, string methodName, string repositoryName) : base(message)
        {
            MethodName = methodName;
            RepositoryName = repositoryName;
        }

        public string MethodName { get; set; }

        public string RepositoryName { get; set; }
    }
}
