using DevMentorHub.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DevMentorHub.Infrastructure.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        public ChatGptService(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? cfg["OpenAI:ApiKey"] ?? string.Empty;
        }

        public async Task<string> GenerateReviewAsync(string systemPrompt, string userPrompt, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("OPENAI_API_KEY not configured");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                }
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            using var res = await _http.SendAsync(req, ct);
            res.EnsureSuccessStatusCode();
            using var stream = await res.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            var content = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return content ?? string.Empty;
        }
    }
}
