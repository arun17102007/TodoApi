using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.DAL
{
    public interface ITodoItemsRepository
    {
        Task<IEnumerable<TodoItemDTO>> GetTodoItems();

        Task<TodoItemDTO> GetTodoItem(long id);

        Task<TodoItemDTO> UpdateTodoItem(long id, TodoItemDTO todoItemDTO);

        Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO);

        Task<bool?> DeleteTodoItem(long id);
    }
}
