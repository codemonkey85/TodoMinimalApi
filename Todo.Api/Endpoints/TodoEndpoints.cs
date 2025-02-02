using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Entities.DTO.Todo;

namespace Todo.Api.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos");

        group.MapGet("/", async Task<Results<Ok<ListTodos>,Ok>> ([FromServices] DataContext context) =>
        {
            var todos = await context.Todos.Select(s => new TodoItemDTO
            {
                Id = s.Id,
                Name = s.Name,
                IsComplete = s.IsComplete,
            }).AsNoTracking().ToListAsync();

            if (todos is null || todos.Count == 0)
            {
                return TypedResults.Ok();
            }

            var list = new ListTodos(todos, todos.Count);
            return TypedResults.Ok<ListTodos>(list);
        }).WithName("GetTodosAsync")
          .Produces<ListTodos>(200);

        group.MapGet("/{id}", async Task<Results<Ok<TodoItemDTO>, NotFound>> (int id, [FromServices] DataContext context) =>
        {
            var todo = await context.Todos.Select(s => new TodoItemDTO
            {
                Id = s.Id,
                Name = s.Name,
                IsComplete = s.IsComplete,
            }).SingleOrDefaultAsync(s => s.Id == id);

            return todo is not null ? TypedResults.Ok<TodoItemDTO>(todo) : TypedResults.NotFound();
        }).WithName("GetTodoAsync")
        .Produces<TodoItemDTO>(200)
        .Produces(404);

        return app;
    }
}
