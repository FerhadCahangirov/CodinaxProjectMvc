namespace CodinaxProjectMvc.Exceptions
{
    public class PaginationException : Exception
    {
        public PaginationException() : base("Pagination Problem")
        {
        }

        public PaginationException(string? message) : base(message)
        {
        }
    }
}
