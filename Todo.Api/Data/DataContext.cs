using Microsoft.EntityFrameworkCore;
using Todo.Shared.Entities;

namespace Todo.Api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().Property("IsComplete")
            .HasDefaultValue(false);
        base.OnModelCreating(modelBuilder);
    }
}
