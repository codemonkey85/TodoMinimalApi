namespace Todo.Api.Entities.DTO.Todo;

public class ListTodos
{
    public List<TodoItemDTO> Todos { get; set; } = [];
    public int TotalCount { get; set; }  
}
