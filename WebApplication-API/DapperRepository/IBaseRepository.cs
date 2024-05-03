namespace WebApplication_API.DapperRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsyncById(int id);
        Task<int> Insert(T item);
    }
}
