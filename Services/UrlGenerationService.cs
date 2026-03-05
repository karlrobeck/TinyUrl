using TinyUrl.Data;

namespace TinyUrl.Services;

/// <summary>
/// Implements URL short ID generation with collision detection and retry logic.
/// Generates 8-character hexadecimal IDs from GUIDs and retries if a collision is detected.
/// </summary>
public class UrlGenerationService : IUrlGenerationService
{
  private readonly AppDbContext _context;
  private readonly ILogger<UrlGenerationService> _logger;

  public UrlGenerationService(AppDbContext context, ILogger<UrlGenerationService> logger)
  {
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Generates a new short ID candidate (8-character hexadecimal substring of a GUID).
  /// </summary>
  public string GenerateShortId()
  {
    return Guid.NewGuid().ToString("N").Substring(0, 8);
  }

  /// <summary>
  /// Generates a unique short ID with collision detection.
  /// Retries with new GUIDs if a collision is detected (ID already exists in database).
  /// </summary>
  public async Task<string> GenerateWithRetryAsync(int maxRetries = 5, CancellationToken cancellationToken = default)
  {
    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
      string candidateId = GenerateShortId();

      // Check if this ID already exists in the database
      var existingUrl = await _context.ShortUrls.FindAsync(new object[] { candidateId }, cancellationToken: cancellationToken);

      if (existingUrl == null)
      {
        _logger.LogInformation("Generated unique short ID on attempt {Attempt}: {ShortId}", attempt, candidateId);
        return candidateId;
      }

      // Collision detected
      _logger.LogInformation("Collision detected for short ID {ShortId} on attempt {Attempt}/{MaxRetries}", candidateId, attempt, maxRetries);
    }

    // All retries exhausted
    _logger.LogError("Failed to generate unique short ID after {MaxRetries} attempts", maxRetries);
    throw new IdGenerationFailedException(
        $"Unable to generate a unique short URL ID after {maxRetries} attempts. Please try again later."
    );
  }
}
