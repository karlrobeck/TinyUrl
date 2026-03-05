namespace TinyUrl.Services;

/// <summary>
/// Service for generating unique short URL IDs with collision detection and retry logic.
/// </summary>
public interface IUrlGenerationService
{
  /// <summary>
  /// Generates a new short ID candidate (8-character hexadecimal substring of a GUID).
  /// Note: This does NOT check for collisions; use GenerateWithRetryAsync for that.
  /// </summary>
  string GenerateShortId();

  /// <summary>
  /// Generates a unique short ID with collision detection and automatic retry.
  /// Attempts up to maxRetries times to generate an ID that doesn't already exist in the database.
  /// </summary>
  /// <param name="maxRetries">Maximum number of retry attempts (default 5)</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>A unique 8-character short ID</returns>
  /// <exception cref="IdGenerationFailedException">Thrown when max retries exceeded</exception>
  Task<string> GenerateWithRetryAsync(int maxRetries = 5, CancellationToken cancellationToken = default);
}
