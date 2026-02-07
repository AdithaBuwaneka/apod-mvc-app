using System.Data;
using ApodMvcApp.DTOs;
using Microsoft.Data.SqlClient;

namespace ApodMvcApp.Repositories
{
    public class ApodRepository : IApodRepository
    {
        private readonly string _connectionString;

        public ApodRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> InsertApodAsync(ApodDto apod)
        {
            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM Apod WHERE Date = @Date)
                BEGIN
                    INSERT INTO Apod (Date, Title, Explanation, Url, HdUrl, MediaType, ServiceVersion, Copyright, ThumbnailUrl)
                    VALUES (@Date, @Title, @Explanation, @Url, @HdUrl, @MediaType, @ServiceVersion, @Copyright, @ThumbnailUrl);
                    SELECT SCOPE_IDENTITY();
                END
                ELSE
                BEGIN
                    SELECT -1;
                END";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = DateTime.Parse(apod.Date) });
            command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 500) { Value = apod.Title });
            command.Parameters.Add(new SqlParameter("@Explanation", SqlDbType.NVarChar, -1) { Value = apod.Explanation });
            command.Parameters.Add(new SqlParameter("@Url", SqlDbType.NVarChar, 1000) { Value = apod.Url });
            command.Parameters.Add(new SqlParameter("@HdUrl", SqlDbType.NVarChar, 1000) { Value = (object?)apod.HdUrl ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@MediaType", SqlDbType.NVarChar, 50) { Value = apod.MediaType });
            command.Parameters.Add(new SqlParameter("@ServiceVersion", SqlDbType.NVarChar, 20) { Value = apod.ServiceVersion });
            command.Parameters.Add(new SqlParameter("@Copyright", SqlDbType.NVarChar, 200) { Value = (object?)apod.Copyright ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@ThumbnailUrl", SqlDbType.NVarChar, 1000) { Value = (object?)apod.ThumbnailUrl ?? DBNull.Value });

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<ApodDto>> GetAllApodsAsync()
        {
            const string sql = @"
                SELECT Date, Title, Explanation, Url, HdUrl, MediaType, ServiceVersion, Copyright, ThumbnailUrl
                FROM Apod
                ORDER BY Date DESC";

            var apods = new List<ApodDto>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                apods.Add(new ApodDto
                {
                    Date = reader.GetDateTime(0).ToString("yyyy-MM-dd"),
                    Title = reader.GetString(1),
                    Explanation = reader.GetString(2),
                    Url = reader.GetString(3),
                    HdUrl = reader.IsDBNull(4) ? null : reader.GetString(4),
                    MediaType = reader.GetString(5),
                    ServiceVersion = reader.GetString(6),
                    Copyright = reader.IsDBNull(7) ? null : reader.GetString(7),
                    ThumbnailUrl = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return apods;
        }

        public async Task<bool> ExistsAsync(string date)
        {
            const string sql = "SELECT COUNT(1) FROM Apod WHERE Date = @Date";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = DateTime.Parse(date) });

            await connection.OpenAsync();
            var count = (int)(await command.ExecuteScalarAsync() ?? 0);
            return count > 0;
        }
    }
}
