using Generals.Data;
using Generals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generals.Controllers
{
    [Produces("application/json")]
    [Route("api/ToDoList/{listIdentity}/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private IToDoRepository _repository;

        public ToDoItemController(IToDoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet(Name = "GetItemsByListIdentity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<ToDoItemResponse>>> GetAllForList(string listIdentity)
        {
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var items = await _repository.GetItemsForList(list.Id);
            return items.Select(i => ProjectItem(listIdentity, i)).ToList();
        }

        [HttpGet("{id}", Name = "GetItemById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> GetById(string listIdentity, int id)
        {
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var item = await _repository.GetItemById(list.Id, id);
            if (item == null)
                return NotFound();
            return ProjectItem(listIdentity, item);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> Create(string listIdentity, [FromBody] ToDoItemRequest request)
        {
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var item = await _repository.CreateItem(ParseItem(list.Id, request));
            return CreatedAtRoute("GetItemById", new { listIdentity, id = item.Id }, ProjectItem(listIdentity, item));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> Update(string listIdentity, int id, [FromBody] ToDoItemRequest request)
        {
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var item = await _repository.GetItemById(list.Id, id);
            if (item == null)
                return NotFound();
            ParseOntoItem(request, item);
            await _repository.SaveChanges();
            return ProjectItem(listIdentity, item);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int listId, int id)
        {
            var item = await _repository.GetItemById(listId, id);
            if (item == null)
                return NotFound();
            await _repository.DeleteItem(listId, id);
            return NoContent();
        }

        private ToDoItemResponse ProjectItem(string listIdentity, ToDoItemRecord item)
        {
            return new ToDoItemResponse
            {
                Id = item.Id,
                Description = item.Description,
                Done = item.Done,
                _links = new Dictionary<string, Link>
                {
                    { "self", new Link(Url.RouteUrl("GetItemById", new { listIdentity, id = item.Id })) },
                    { "collection", new Link(Url.RouteUrl("GetItemsByListIdentity", new { listIdentity })) },
                    { "list", new Link(Url.RouteUrl("GetListByIdentity", new { identity = listIdentity })) },
                }
            };
        }

        private ToDoItemRecord ParseItem(int listId, ToDoItemRequest request)
        {
            var item = new ToDoItemRecord()
            {
                ListId = listId
            };
            ParseOntoItem(request, item);
            return item;
        }

        private void ParseOntoItem(ToDoItemRequest request, ToDoItemRecord item)
        {
            item.Description = request.Description;
            item.Done = request.Done;
        }
    }
}