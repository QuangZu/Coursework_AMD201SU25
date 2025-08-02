using Microsoft.EntityFrameworkCore;
using URLShorteningService.Models;

namespace URLShorteningService.Data
{
    public class URLShorteningContext : DbContext
    {
        public URLShorteningContext(DbContextOptions<URLShorteningContext> options)
            : base(options)
        {
        }

        public DbSet<URLShortening> URLs { get; set; }
    }
}
