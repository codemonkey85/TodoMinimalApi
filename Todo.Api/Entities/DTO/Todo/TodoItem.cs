namespace Todo.Api.Entities.DTO.Todo;

public class TodoItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsComplete { get; set; }
}
