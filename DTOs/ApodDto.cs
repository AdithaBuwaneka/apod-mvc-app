using System.Text.Json.Serialization;

namespace ApodMvcApp.DTOs
{
    public class ApodDto
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("hdurl")]
        public string? HdUrl { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; } = string.Empty;

        [JsonPropertyName("service_version")]
        public string ServiceVersion { get; set; } = string.Empty;

        [JsonPropertyName("copyright")]
        public string? Copyright { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }
    }
}
