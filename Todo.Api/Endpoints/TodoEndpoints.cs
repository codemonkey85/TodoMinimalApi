using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Contracts;
using Todo.Api.Data;
using Todo.Api.Entities;
using Todo.Api.Entities.DTO.Todo;
using Todo.Api.Entities.Validators;

namespace Todo.Api.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos");

        group.MapGet("/", async Task<Results<Ok<ListTodos>,Ok>> 
            ([FromServices] ITodoService service) =>
        {
            var todos = await service.GetAllAsync();

            if (todos is null || todos.Count == 0)
            {
                return TypedResults.Ok();
            }

            var list = new ListTodos(todos, todos.Count);
            return TypedResults.Ok<ListTodos>(list);
        }).WithName("GetTodosAsync");

        group.MapGet("/{id}", async Task<Results<Ok<TodoItemDTO>, NotFound>> 
            (int id, 
            [FromServices] ITodoService service) =>
        {
            var todo = await service.GetByIdAsync(id);

            return await service.GetByIdAsync(id) is not null
                ? TypedResults.Ok<TodoItemDTO>(todo)
                : TypedResults.NotFound();

        }).WithName("GetByIdAsync");

        group.MapPost("/", async Task<Results<Created<TodoItemDTO>, ValidationProblem>> (
            [FromServices]ITodoService service,
            [FromServices]IValidator<AddTodo> validator,
            [FromBody]AddTodo model) =>
        {
            var result = await validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return TypedResults.ValidationProblem(result.ToDictionary());
            }

            var newTodo = await service.CreateAsync(model);
            
            return TypedResults.Created($"{newTodo.Id}", newTodo);
        }).WithName("CreateTodoAsync");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, ValidationProblem, BadRequest<string>>> (
            int id,
            [FromServices]ITodoService service,
            [FromServices]IValidator<UpdateTodo> validator,
            [FromBody]UpdateTodo model) =>
        {
            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                return TypedResults.ValidationProblem(result.ToDictionary());
            }

            await service.UpdateAsync(id, model);

            return TypedResults.Ok();
        }).WithName("UpdateTodoAsync");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>>
            (int id, [FromServices]ITodoService service) =>
        {
            await service.DeleteAsync(id);

            return TypedResults.Ok();
        }).WithName("DeleteTodoAsync");

        return app;
    }
}
