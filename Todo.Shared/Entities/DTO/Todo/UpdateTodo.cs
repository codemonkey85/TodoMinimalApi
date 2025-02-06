namespace Todo.Shared.Entities.DTO.Todo;

public class UpdateTodo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
