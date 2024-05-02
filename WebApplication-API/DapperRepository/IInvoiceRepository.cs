using WebApplication_API.Domains;

namespace WebApplication_API.DapperRepository
{
    public interface IInvoiceRepository: IBaseRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllInvoiceWithDetails();
    }
}