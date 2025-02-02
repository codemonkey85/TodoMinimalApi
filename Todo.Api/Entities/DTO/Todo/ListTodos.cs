namespace Todo.Api.Entities.DTO.Todo;

public class ListTodos(List<TodoItemDTO> todos, int totalCount)
{
    public ListTodos() : this([], 0)
    { }
    public List<TodoItemDTO> Todos { get; set; } = todos;
    public int TotalCount { get; set; } = totalCount;
}
