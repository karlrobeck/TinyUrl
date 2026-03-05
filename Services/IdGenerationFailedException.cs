namespace TinyUrl.Services;

/// <summary>
/// Exception thrown when URL short ID generation fails after all retry attempts are exhausted.
/// This typically indicates either a GUID collision detection occurred too many times
/// or the underlying database has a constraint violation.
/// </summary>
public class IdGenerationFailedException : Exception
{
  public IdGenerationFailedException(string message) : base(message)
  {
  }

  public IdGenerationFailedException(string message, Exception innerException) : base(message, innerException)
  {
  }
}
