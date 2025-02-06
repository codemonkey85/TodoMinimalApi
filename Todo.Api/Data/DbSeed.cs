using Bogus;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Todo.Shared.Entities;

namespace Todo.Api.Data;

public static class DbInit
{
    public static void Seed(DataContext context)
    {
        try
        {
            Log.Information("--- Dropping and recreating Database...---");
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            Log.Information("--- Creating Todos:");
            SeedTodos(context);

            Log.Information("--- Db Seed Routine done! ---");
            Environment.Exit(0);
        }
        catch(Exception ex)
        {
            Log.Fatal(ex.Message);
            Environment.Exit(1);
        }
    }

    private static void SeedTodos(DataContext context)
    {
        var fakeTodos = new Faker<TodoItem>();

        fakeTodos.RuleFor(u => u.Name, f => f.Lorem.Sentence(5))
                 .RuleFor(u => u.IsComplete, f => f.Random.Bool(.25f));

        var todos = fakeTodos.Generate(100);

        context.Todos.AddRange(todos);
        context.SaveChanges();
    }
}
