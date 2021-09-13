using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Utils
{
    /// <summary>
    ///  Extension method to map DTO with the model
    /// </summary>
    public static class TodoItemMapper
    {
        public static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
                Description = todoItem.Description
            };
    }
}
