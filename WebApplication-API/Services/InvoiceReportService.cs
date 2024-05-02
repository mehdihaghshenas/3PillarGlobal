using WebApplication_API.DapperRepository;
using WebApplication_API.Domains;

namespace WebApplication_API.Services
{
    public class InvoiceReportService: IInvoiceReportService
    {
        private readonly IInvoiceRepository _factorRepository;
        public InvoiceReportService(IInvoiceRepository factorRepository)
        {
            _factorRepository = factorRepository;
        }
        public async Task<List<Invoice>> GetAllFactorsAsync()
        {
            return (await _factorRepository.GetAllInvoiceWithDetails()).ToList();
        }
    }
}
