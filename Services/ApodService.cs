using System.Text.Json;
using ApodMvcApp.DTOs;

namespace ApodMvcApp.Services
{
    public class ApodService : IApodService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.nasa.gov/planetary/apod";

        public ApodService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["NasaApi:ApiKey"] ?? "DEMO_KEY";
        }

        public async Task<ApodDto?> GetTodayApodAsync()
        {
            try
            {
                var url = $"{BaseUrl}?api_key={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApodDto>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ApodDto>> GetApodsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var start = startDate.ToString("yyyy-MM-dd");
                var end = endDate.ToString("yyyy-MM-dd");
                
                var url = $"{BaseUrl}?api_key={_apiKey}&start_date={start}&end_date={end}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                    return new List<ApodDto>();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ApodDto>>(json) ?? new List<ApodDto>();
            }
            catch (Exception)
            {
                return new List<ApodDto>();
            }
        }
    }
}
