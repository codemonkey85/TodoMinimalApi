﻿using FluentValidation;
using Todo.Api.Contracts;
using Todo.Api.Entities.DTO.Todo;
using Todo.Api.Entities.Validators;
using Todo.Api.Services;

namespace Todo.Api.Helpers;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddTodo>, AddTodoValidator>();
        services.AddScoped<IValidator<UpdateTodo>, UpdateTodoValidator>();        
        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        return services;
    }
}
