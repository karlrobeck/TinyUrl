using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyUrl.Data;
using TinyUrl.Services;

namespace TinyUrl.Pages;

[BindProperties]
public class IndexModel(AppDbContext context, IUrlGenerationService urlGenerationService, ILogger<IndexModel> logger) : PageModel
{
    private readonly AppDbContext _context = context;
    private readonly IUrlGenerationService _urlGenerationService = urlGenerationService;
    private readonly ILogger<IndexModel> _logger = logger;

    public string? NewUrl { get; set; }
    public string? ShortenedUrl { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {

    }

    public async Task OnPost()
    {
        ErrorMessage = null;
        ShortenedUrl = null;

        if (string.IsNullOrEmpty(NewUrl))
        {
            ModelState.AddModelError("NewUrl", "URL is required.");
            return;
        }

        try
        {
            // Generate a unique short ID with collision detection and retry
            string shortId = await _urlGenerationService.GenerateWithRetryAsync(maxRetries: 5);

            var shortUrl = new Models.ShortUrl
            {
                Id = shortId,
                OriginalUrl = NewUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Add(shortUrl);
            await _context.SaveChangesAsync();

            ShortenedUrl = $"{Request.Scheme}://{Request.Host}/url/{shortUrl.Id}";
            _logger.LogInformation("Successfully created short URL {ShortId} for {OriginalUrl}", shortId, NewUrl);
        }
        catch (IdGenerationFailedException ex)
        {
            _logger.LogError(ex, "Failed to generate short URL ID");
            ErrorMessage = ex.Message;
            ModelState.AddModelError("", ErrorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating short URL");
            ErrorMessage = "An unexpected error occurred. Please try again later.";
            ModelState.AddModelError("", ErrorMessage);
        }
    }
}
