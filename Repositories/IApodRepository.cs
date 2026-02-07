using ApodMvcApp.DTOs;

namespace ApodMvcApp.Repositories
{
    public interface IApodRepository
    {
        Task<int> InsertApodAsync(ApodDto apod);
        Task<List<ApodDto>> GetAllApodsAsync();
        Task<bool> ExistsAsync(string date);
    }
}
