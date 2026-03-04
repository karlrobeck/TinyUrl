using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.Data;

namespace TinyUrl.Controllers;

[Route("[controller]")]
[ApiController]
public class UrlController(AppDbContext context) : ControllerBase
{
  private readonly AppDbContext _context = context;

  [HttpGet("{id}")]
  public async Task<IActionResult> RedirectToOriginalUrl(string id)
  {
    Console.WriteLine(id);
    var shortUrl = await _context.ShortUrls.FindAsync(id);
    if (shortUrl == null)
    {
      return NotFound();
    }
    return Redirect(shortUrl.OriginalUrl);
  }
}
