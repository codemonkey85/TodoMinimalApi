using Todo.Shared.Entities.DTO.Todo;

namespace Todo.Api.Contracts;

public interface ITodoService
{
    Task<List<TodoItemDTO>> GetAllAsync();
    Task<TodoItemDTO?> GetByIdAsync(int id);
    Task<TodoItemDTO> CreateAsync(AddTodo model);
    Task UpdateAsync(int id, UpdateTodo model);
    Task DeleteAsync(int id);
}
