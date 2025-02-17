using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Contracts;
using Todo.Api.Data;
using Todo.Shared.Entities;
using Todo.Shared.Entities.DTO.Todo;
using Todo.Shared.Entities.Validators;

namespace Todo.Api.Endpoints;

public static class TodoEndpoints
{
    public static object MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todos");

        group.MapGet("/", async Task<Results<Ok<ListTodos>,Ok>> 
            ([FromServices] ITodoService service) =>
        {
            return await service.GetAllAsync()
                 is List<TodoItemDTO> todoItems && todoItems.Count > 0
                    ? TypedResults.Ok(new ListTodos(todoItems, todoItems.Count))
                    : TypedResults.Ok();

        }).WithName("GetTodosAsync");

        group.MapGet("/{id}", async Task<Results<Ok<TodoItemDTO>, NotFound>> 
            (int id, 
            [FromServices] ITodoService service) =>
        {
            return await service.GetByIdAsync(id) 
                is TodoItemDTO todo
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
