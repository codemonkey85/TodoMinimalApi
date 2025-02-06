using FluentValidation;
using Todo.Shared.Entities.DTO.Todo;

namespace Todo.Shared.Entities.Validators;

public class AddTodoValidator : AbstractValidator<AddTodo>
{
    public AddTodoValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(100);
    }
}
