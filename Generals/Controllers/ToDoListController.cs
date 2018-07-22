using Generals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generals.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        [HttpGet]
        public Task<ActionResult<List<ToDoList>>> GetAllAsync()
        {
            ActionResult<List<ToDoList>> lists = new List<ToDoList>
            {
                new ToDoList
                {
                    Id = 32,
                    Name = "Household",
                    Items = new List<ToDoItem>
                    {
                        new ToDoItem
                        {
                            Id = 23,
                            Description = "Install pegboard",
                            Done = false
                        },
                        new ToDoItem
                        {
                            Id = 57,
                            Description = "Paint craft room",
                            Done = false
                        }
                    }
                }
            };

            return Task.FromResult(lists);
        }
    }
}