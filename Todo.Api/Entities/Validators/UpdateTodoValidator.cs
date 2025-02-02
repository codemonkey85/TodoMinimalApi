﻿using FluentValidation;
using Todo.Api.Entities.DTO.Todo;

namespace Todo.Api.Entities.Validators;

public class UpdateTodoValidator : AbstractValidator<UpdateTodo>
{
    public UpdateTodoValidator()
    {
        RuleFor(r => r.Id).NotNull().GreaterThan(0);
        RuleFor(r => r.Name).NotEmpty().MaximumLength(100);
        RuleFor(r => r.IsComplete).NotNull();
    }
}
