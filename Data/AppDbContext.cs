using System;
using Microsoft.EntityFrameworkCore;
using TinyUrl.Models;

namespace TinyUrl.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<ShortUrl> ShortUrls { get; set; }
}
