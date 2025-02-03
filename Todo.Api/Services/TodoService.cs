using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Contracts;
using Todo.Api.Data;
using Todo.Api.Entities;
using Todo.Api.Entities.DTO.Todo;

namespace Todo.Api.Services;

public class TodoService(DataContext context, 
    ILogger<TodoService> logger,
    IMapper mapper)
    : ITodoService
{
    private const string FILENAME = "~/Services/TodoService.cs";

    private readonly DataContext _context = context;
    private readonly ILogger<TodoService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<List<TodoItemDTO>> GetAllAsync()
    {
        try
        {
            var todos = await _context.Todos.ToListAsync();
            if (todos is null || todos.Count == 0)
            {
                return [];
            }
            return _mapper.Map<List<TodoItemDTO>>(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FILENAME} - [{methodName}]: {exMessage}", FILENAME, nameof(GetAllAsync), ex.Message);
            throw;
        }
    }

    public async Task<TodoItemDTO?> GetByIdAsync(int id)
    {
        try
        {
            TodoItem? todo = await _context.Todos.FindAsync(id)
                ?? throw new KeyNotFoundException($"Unable to find record by ID: `{id}`");
            return _mapper.Map<TodoItemDTO>(todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FILENAME} - [{methodName}]: {exMessage}", FILENAME, nameof(GetByIdAsync), ex.Message);
            throw;
        }
    }

    public async Task<TodoItemDTO> CreateAsync(AddTodo model)
    {
        try
        {
            var newTodo = _mapper.Map<TodoItem>(model);
            _context.Todos.Add(newTodo);
            await _context.SaveChangesAsync();
            return _mapper.Map<TodoItemDTO>(newTodo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FILENAME} - [{methodName}]: {exMessage}", FILENAME, nameof(CreateAsync), ex.Message);
            throw;
        }
    }

    public async Task UpdateAsync(int id, UpdateTodo model)
    {
        try
        {
            if (id != model.Id)
            {
                throw new InvalidOperationException($"Record ID mismatch. Aborting.");
            }

            var dbTodo = await _context.Todos.FindAsync(id) 
                ?? throw new KeyNotFoundException($"Unable to find record by ID: `{id}`");

            _mapper.Map(model, dbTodo);

            _context.Entry(dbTodo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FILENAME} - [{methodName}]: {exMessage}", FILENAME, nameof(UpdateAsync), ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var todo = await _context.Todos.FindAsync(id)
                ?? throw new KeyNotFoundException($"Unable to find record by ID: `{id}`");

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FILENAME} - [{methodName}]: {exMessage}", FILENAME, nameof(DeleteAsync), ex.Message);
            throw;
        }
    }
}
