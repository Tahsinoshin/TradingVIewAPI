
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    public DbSet<ApiItem> ApiItems { get; set; } = null!;
}
