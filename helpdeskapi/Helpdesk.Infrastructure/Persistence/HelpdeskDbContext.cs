using Helpdesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Infrastructure.Persistence;

public class HelpdeskDbContext : DbContext
{
    public HelpdeskDbContext(DbContextOptions<HelpdeskDbContext> options) : base(options)
    {
    }

    public DbSet<Token> Tokens => Set<Token>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Token>(entity =>
        {
            entity.ToTable("Tokens");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).HasMaxLength(200);
            entity.Property(t => t.Status).HasMaxLength(50);
        });
    }
}
