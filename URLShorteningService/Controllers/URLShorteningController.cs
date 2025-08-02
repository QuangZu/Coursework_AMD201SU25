using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using URLShorteningService.Data;
using URLShorteningService.Models;

namespace URLShorteningService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class URLShorteningController : ControllerBase
    {
        private readonly URLShorteningContext _context;
        private readonly RedisCacheService _cache;
        private readonly RabbitMQPublisher _publisher;
        private readonly HttpClient _httpClient;

        public URLShorteningController(
            URLShorteningContext context,
            RedisCacheService cache,
            RabbitMQPublisher publisher,
            HttpClient httpClient)
        {
            _context = context;
            _cache = cache;
            _publisher = publisher;
            _httpClient = httpClient;
        }

        [HttpOptions("shorten")]
        public IActionResult ShortenUrlOptions()
        {
            // Handle CORS preflight request
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return Ok();
        }

        [EnableRateLimiting("Fixed")]
        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.LongUrl))
                return BadRequest("URL is required.");

            string shortCode;

            // Check if custom alias is provided
            if (!string.IsNullOrWhiteSpace(request.CustomAlias))
            {
                shortCode = request.CustomAlias;
                
                // Check if custom alias already exists
                var existingUrl = await _context.URLs.FirstOrDefaultAsync(u => u.short_url == shortCode);
                if (existingUrl != null)
                {
                    return BadRequest("Custom alias already exists. Please choose a different one.");
                }
            }
            else
            {
                // Generate random short code
                shortCode = GenerateShortCode(request.LongUrl);
            }

            var entity = new URLShortening
            {
                short_url = shortCode,
                long_url = request.LongUrl
            };

            _context.URLs.Add(entity);
            await _context.SaveChangesAsync();

            // Cache and Publish
            await _cache.SetCacheAsync(shortCode, request.LongUrl);
            var message = System.Text.Json.JsonSerializer.Serialize(new { short_url = shortCode, long_url = request.LongUrl });
            _publisher.Publish(message);

            // Add to user history if userId is provided
            if (request.UserId.HasValue)
            {
                await AddToHistory(request.UserId.Value, request.LongUrl, shortCode);
            }

            return Ok(new { short_url = shortCode });
        }

        private string GenerateShortCode(string input)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input + DateTime.UtcNow));
            return Convert.ToBase64String(hash)[..6] // 6-char code
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "");
        }

        private async Task AddToHistory(int userId, string longUrl, string shortCode)
        {
            try
            {
                var historyData = new
                {
                    userId = userId,
                    longUrl = longUrl,
                    shortUrl = shortCode,
                    createdAt = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(historyData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send request to UserService through OcelotGateway
                var response = await _httpClient.PostAsync("http://localhost:8000/user/history", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    // Log error but don't fail the main operation
                    Console.WriteLine($"Failed to add URL to history: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the main operation
                Console.WriteLine($"Error adding URL to history: {ex.Message}");
            }
        }
    }

    public class UrlRequest
    {
        public string LongUrl { get; set; } = string.Empty;
        public string? CustomAlias { get; set; }
        public int? UserId { get; set; }
    }
}
