using Generals.Data;
using Generals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generals.Controllers
{
    [Produces("application/json")]
    [Route("api/ToDoList/{listId}/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private IToDoRepository _repository;

        public ToDoItemController(IToDoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<ToDoItemResponse>>> GetAllForList(int listId)
        {
            var items = await _repository.GetItemsForList(listId);
            if (items == null)
                return NotFound();
            return items.Select(ProjectItem).ToList();
        }

        [HttpGet("{id}", Name = "GetItemById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> GetById(int listId, int id)
        {
            var item = await _repository.GetItemById(listId, id);
            if (item == null)
                return NotFound();
            return ProjectItem(item);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> Create(int listId, [FromBody] ToDoItemRequest request)
        {
            var list = await _repository.GetListById(listId);
            if (list == null)
                return NotFound();
            var item = await _repository.CreateItem(ParseItem(listId, request));
            return CreatedAtRoute("GetItemById", new { listId, id = item.Id }, ProjectItem(item));
        }

        private ToDoItemResponse ProjectItem(ToDoItemRecord item)
        {
            return new ToDoItemResponse
            {
                Id = item.Id,
                Description = item.Description,
                Done = item.Done
            };
        }

        private ToDoItemRecord ParseItem(int listId, ToDoItemRequest request)
        {
            return new ToDoItemRecord
            {
                ListId = listId,
                Description = request.Description,
                Done = request.Done
            };
        }
    }
}