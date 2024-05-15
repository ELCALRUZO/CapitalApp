using CapitalApp.Models;

namespace CapitalApp.Data
{
    public interface IApplicationRepository
    {
        Task<ApplicationDto> CreateApplicationAsync(ApplicationDto applicationDto);

        Task<ApplicationDto> GetApplicationByIdAsync(string applicationId);
    }
}
