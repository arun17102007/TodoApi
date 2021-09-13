using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Utils;
using Serilog;

namespace TodoApi.DAL
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly TodoContext _context;
        private readonly ILogger _logger;
        public TodoItemsRepository(TodoContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #region CRUD methods
        public async Task<IEnumerable<TodoItemDTO>> GetTodoItems()
        {
            return await _context.TodoItems
                .Select(x => TodoItemMapper.ItemToDTO(x))
                .ToListAsync();
        }

        public async Task<TodoItemDTO> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            return TodoItemMapper.ItemToDTO(todoItem);
        }

        public async Task<TodoItemDTO> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;
            todoItem.Id = todoItemDTO.Id;
            todoItem.Description = todoItemDTO.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return null;
            }
            return TodoItemMapper.ItemToDTO(todoItem);
        }

        public async Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name,
                Id = todoItemDTO.Id,
                Description = todoItemDTO.Description
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return TodoItemMapper.ItemToDTO(todoItem);
        }

        public async Task<bool?> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
        private bool TodoItemExists(long id) => _context.TodoItems.Any(e => e.Id == id);
    }
}


