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
        public string? CustomAlias { get; set; }
    }
}
