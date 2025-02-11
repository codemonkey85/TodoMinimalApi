using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Todo.Shared.Entities.DTO.Todo;
using Todo.Shared.Helpers;

namespace Todo.Client.Pages;

public partial class Home(
    IHttpClientFactory httpClientFactory,
    IDialogService dialogService,
    ILogger<Home> logger,
    ISnackbar snackbarService) : ComponentBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IDialogService _dialogService = dialogService;
    private readonly ILogger<Home> _logger = logger;
    private readonly ISnackbar _snackbarService = snackbarService;

    private ListTodos? todos;
    private ObservableCollection<TodoItemDTO> _items = [];

    protected override async Task OnInitializedAsync()
    {
        using var client = _httpClientFactory.CreateClient("Todo.Api");
        todos = await client.GetFromJsonAsync<ListTodos>("/api/todos");
        _items = new(todos!.Todos);
    }

    async Task DeleteItemAsync(TodoItemDTO item)
    {        
        var parameters = new DialogParameters<_DataGridDeleteDialog> { { x => x.Todo, item } };

        var dialog = await _dialogService.ShowAsync<_DataGridDeleteDialog>("Delete Todo", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            _items.Remove(item);
        }
    }

    async Task EditItemAsync(TodoItemDTO item)
    {
        var parameters = new DialogParameters<_DataGridEditDialog> { { x => x.Todo, item } };

        var dialog = await _dialogService.ShowAsync<_DataGridEditDialog>("Edit Todo", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            _items.ReplaceItem(r => r.Id == item.Id, item);
        }
    }
}
