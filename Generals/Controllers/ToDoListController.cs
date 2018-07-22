using Generals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generals.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private IToDoRepository _repository = new ToDoRepository();

        [HttpGet]
        public async Task<ActionResult<List<ToDoList>>> GetAllAsync()
        {
            var lists = await _repository.GetAllLists();
            return lists.Select(ProjectList).ToList();
        }

        private ToDoList ProjectList(ToDoListRecord record)
        {
            return new ToDoList
            {
                Id = record.Id,
                Name = record.Name
            };
        }
    }
}