namespace WebApplication_API.Services
{
    public interface IBlobService
    {
        Task UploadAttachment(int invoiceId, IFormFile[] files, bool isSecure);
    }
}