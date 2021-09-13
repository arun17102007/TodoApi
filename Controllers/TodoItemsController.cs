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
using TodoApi.DAL;
using Microsoft.AspNetCore.Cors;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsRepository _todoItemsRepository;
        private readonly ILogger _logger;
        public TodoItemsController(ITodoItemsRepository todoItemsRepository, ILogger logger)
        {
            _todoItemsRepository = todoItemsRepository;
            _logger = logger;
        }

        #region CRUD methods
        // GET: api/TodoItems
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var result = await _todoItemsRepository.GetTodoItems();
            if (result != null && result.Any())
                return  Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _todoItemsRepository.GetTodoItem(id);
            _logger.Information($"Query string {id}");
            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItemDTO>> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            _logger.Information($"Id {id}, TodoItemDTO {todoItemDTO.Id + todoItemDTO.Name + todoItemDTO.IsComplete }");
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _todoItemsRepository.UpdateTodoItem(id, todoItemDTO);
            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem; 
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var todoItemDTOResult = await _todoItemsRepository.CreateTodoItem(todoItemDTO);

            var result = CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItemDTOResult.Id },
                todoItemDTOResult).Value;
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _todoItemsRepository.DeleteTodoItem(id);

            if (todoItem == null)
            {
                return NotFound();
            }

           return NoContent();
        }
        #endregion

        /*
        // This method is just for testing populating the secret field
        // POST: api/TodoItems/test
        [HttpPost("test")]
        public async Task<ActionResult<TodoItem>> PostTestTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // This method is just for testing
        // GET: api/TodoItems/test
        [HttpGet("test")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTestTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }
        */
    }
}
