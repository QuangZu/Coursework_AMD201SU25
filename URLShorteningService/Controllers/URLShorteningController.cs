using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
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

        public URLShorteningController(
            URLShorteningContext context,
            RedisCacheService cache,
            RabbitMQPublisher publisher)
        {
            _context = context;
            _cache = cache;
            _publisher = publisher;
        }

        [EnableRateLimiting("Fixed")]
        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.LongUrl))
                return BadRequest("URL is required.");

            var shortCode = GenerateShortCode(request.LongUrl);

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
    }

    public class UrlRequest
    {
        public string LongUrl { get; set; } = string.Empty;
    }
}
