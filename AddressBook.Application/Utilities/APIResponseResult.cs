namespace AddressBook.Application.Utilities
{
    public class APIResponseResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int TotalPages { get; set; }

        public APIResponseResult(T data, string message = null, int totalPages = 0)
        {
            Success = true;
            Data = data;
            Message = message;
            TotalPages = totalPages;
        }

        public APIResponseResult(string message)
        {
            Success = false;
            Message = message;
        }
    }
}
