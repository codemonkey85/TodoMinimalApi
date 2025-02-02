using FluentValidation;
using Todo.Api.Entities.DTO.Todo;

namespace Todo.Api.Entities.Validators;

public class AddTodoValidator : AbstractValidator<AddTodo>
{
    public AddTodoValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(100);
    }
}
