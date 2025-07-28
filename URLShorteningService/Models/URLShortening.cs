namespace URLShorteningService.Models
{
    public class URLShortening
    {
        public int Id { get; set; }
        public string? short_url { get; set; }
        public string? long_url { get; set; }
        public DateOnly created_date { get; set; }
    }
}
