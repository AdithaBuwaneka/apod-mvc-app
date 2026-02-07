using ApodMvcApp.DTOs;

namespace ApodMvcApp.Services
{
    public interface IApodService
    {
        Task<ApodDto?> GetTodayApodAsync();
        Task<List<ApodDto>> GetApodsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
