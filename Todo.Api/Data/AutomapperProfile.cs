using AutoMapper;
using Todo.Shared.Entities;
using Todo.Shared.Entities.DTO.Todo;

namespace Todo.Api.Data;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<TodoItem, TodoItemDTO>();
        CreateMap<AddTodo, TodoItem>();
        CreateMap<UpdateTodo, TodoItem>()
            .ForMember(f => f.Id, opt => opt.Ignore());
    }
}
