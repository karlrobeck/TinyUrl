using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyUrl.Data;

namespace TinyUrl.Pages;

[BindProperties]
public class IndexModel(AppDbContext context) : PageModel
{
    private readonly AppDbContext _context = context;

    public string? NewUrl { get; set; }
    public string? ShortenedUrl { get; set; }

    public void OnGet()
    {

    }

    public async Task OnPost()
    {
        if (string.IsNullOrEmpty(NewUrl))
        {
            ModelState.AddModelError("NewUrl", "URL is required.");
            return;
        }

        var shortUrl = new Models.ShortUrl
        {
            OriginalUrl = NewUrl,
            CreatedAt = DateTime.UtcNow
        };

        _context.Add(shortUrl);

        await _context.SaveChangesAsync();

        ShortenedUrl = $"{Request.Scheme}://{Request.Host}/url/{shortUrl.Id}";
    }
}
