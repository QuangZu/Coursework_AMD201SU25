using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Npgsql;
using Dapper;

[ApiController]
[Route("{shortCode}")]
public class RedirectController : ControllerBase
{
    private readonly IDatabase _redis;
    private readonly IConfiguration _config;

    public RedirectController(IConfiguration config)
    {
        _config = config;
        var redis = ConnectionMultiplexer.Connect(_config["Redis:Connection"]);
        _redis = redis.GetDatabase();
    }

    [HttpGet]
    public async Task<IActionResult> RedirectToLongUrl(string shortCode)
    {
        // Try Redis cache first
        var longUrl = await _redis.StringGetAsync(shortCode);

        if (longUrl.IsNullOrEmpty)
        {
            // If not found in Redis, query Postgres
            using var conn = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            var sql = "SELECT long_url FROM \"URLs\" WHERE short_url = @Short";
            longUrl = await conn.QueryFirstOrDefaultAsync<string>(sql, new { Short = shortCode });

            if (!string.IsNullOrEmpty(longUrl))
            {
                await _redis.StringSetAsync(shortCode, longUrl);
            }
        }

        return Redirect(longUrl);
    }
}
