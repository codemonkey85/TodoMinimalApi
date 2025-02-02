namespace Todo.Api.Entities.DTO.Todo;

public class TodoItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public bool IsComplete { get; set; }
}
