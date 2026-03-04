using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyUrl.Models;

[Table("ShortUrls", Schema = "dbo")]
public class ShortUrl
{
  [Key]
  public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8);
  public string OriginalUrl { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}
