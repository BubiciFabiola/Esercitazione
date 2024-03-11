using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace ItaliaTreni.Infrastructure;

public sealed class ItaliaTreniDbContext : DbContext
{
    public DbSet<ItaliaTreni.Domain.Model.File> Files { get; private set; } = null!;

    public ItaliaTreniDbContext(DbContextOptions<ItaliaTreniDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItaliaTreniDbContext).Assembly);
    }
}
