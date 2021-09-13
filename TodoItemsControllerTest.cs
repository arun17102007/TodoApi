using NUnit.Framework;
using Serilog;
using Moq;
using TodoApi.Models;
using TodoApi.Controllers;
using TodoApi.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace ToDoTest
{
    public class ToItemsControllerTests
    {
        
        private Mock<ILogger> _mockLogger;
        private TodoContext _context;
        private Mock<ITodoItemsRepository> _mockRepository;
        private TodoItemsController todoItemsController;
        private IEnumerable<TodoItemDTO> _todoItemDTOList;
        private TodoItemDTO _todoItemDTO;
        private long id;
        private bool? isDeleted;
        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger>();
            _mockRepository = new Mock<ITodoItemsRepository>();
            todoItemsController = new TodoItemsController(_mockRepository.Object, _mockLogger.Object);
            _todoItemDTOList = new List<TodoItemDTO> { new TodoItemDTO()
                                                       {
                                                            Id = 1,
                                                            Name = "ShoppingItem-1",
                                                            Description = "Gift shopping List",
                                                            IsComplete = true
                                                        },
                                                        new TodoItemDTO()
                                                       {
                                                            Id = 2,
                                                            Name = "ShoppingItem-2",
                                                            Description = "Gift shopping List",
                                                            IsComplete = false
                                                        }
                                                      };
            _todoItemDTO = _todoItemDTOList.FirstOrDefault();
            id = 1;
            isDeleted = true;
        }

        [Test]
        public void GetTodoItems()
        {
            _mockRepository.Setup(p => p.GetTodoItems()).Returns(Task.FromResult<IEnumerable<TodoItemDTO>>(_todoItemDTOList));
            var result = todoItemsController.GetTodoItems().Result;
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTodoItem()
        {
            _mockRepository.Setup(p => p.GetTodoItem(It.IsAny<long>())).Returns(Task.FromResult<TodoItemDTO>(_todoItemDTO));
            var result = todoItemsController.GetTodoItem(id).Result;
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateTodoItem()
        {
            _mockRepository.Setup(p => p.UpdateTodoItem(It.IsAny<long>(), It.IsAny<TodoItemDTO>())).Returns(Task.FromResult<TodoItemDTO>(_todoItemDTO));
            var result = todoItemsController.UpdateTodoItem(id, _todoItemDTO).Result;
            Assert.IsNotNull(result);
        }


        [Test]
        public void CreateTodoItem()
        {
            _mockRepository.Setup(p => p.CreateTodoItem(It.IsAny<TodoItemDTO>())).Returns(Task.FromResult<TodoItemDTO>(_todoItemDTO));
            var result = todoItemsController.CreateTodoItem(_todoItemDTO).Result;
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteTodoItem()
        {
            _mockRepository.Setup(p => p.DeleteTodoItem(It.IsAny<long>())).Returns(Task.FromResult<bool?>(isDeleted));
            var result = todoItemsController.DeleteTodoItem(id).Result;
            Assert.IsNotNull(result);
        }


    }
}