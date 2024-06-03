namespace AddressBook.Application.Interfaces
{
    public interface ICommonRepository<T> where T : class
    {
        Task<int> CountAsync();
    }
}
