using Generals.Data;
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
        private IToDoRepository _repository;

        public ToDoListController(IToDoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoListResponse>>> GetAll()
        {
            var lists = await _repository.GetAllLists();
            return lists.Select(ProjectList).ToList();
        }

        [HttpGet("{identity}", Name = "GetListByIdentity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoListResponse>> GetByIdentity(string identity)
        {
            var list = await _repository.GetListByIdentity(identity);
            if (list == null)
                return NotFound();
            return ProjectList(list);
        }

        [HttpPut("{identity}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<ToDoListResponse>> CreateOrUpdate(string identity, [FromBody] ToDoListRequest request)
        {
            var list = await _repository.GetListByIdentity(identity);
            if (list == null)
            {
                var record = await _repository.CreateList(ParseList(identity, request));
                return CreatedAtRoute("GetListByIdentity", new { identity }, ProjectList(record));
            }
            else if (list.LastUpdateDateTime < request.UpdateDateTime)
            {
                ParseOntoList(request, list);
                await _repository.SaveChanges();
            }

            return ProjectList(list);
        }

        [HttpDelete("{identity}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(string identity)
        {
            var list = await _repository.GetListByIdentity(identity);
            if (list == null)
                return NotFound();
            await _repository.DeleteList(list.Id);
            return NoContent();
        }

        private ToDoListResponse ProjectList(ToDoListRecord list)
        {
            return new ToDoListResponse
            {
                Name = list.Name,
                _links = new Dictionary<string, Link>
                {
                    { "self", new Link(Url.RouteUrl("GetListByIdentity", new { identity = list.Identity })) },
                    { "items", new Link(Url.RouteUrl("GetItemsByListIdentity", new { listIdentity = list.Identity })) }
                }
            };
        }

        private ToDoListRecord ParseList(string identity, ToDoListRequest request)
        {
            var list = new ToDoListRecord
            {
                Identity = identity
            };
            ParseOntoList(request, list);
            return list;
        }

        private void ParseOntoList(ToDoListRequest request, ToDoListRecord list)
        {
            list.Name = request.Name;
            list.LastUpdateDateTime = request.UpdateDateTime;
        }
    }
}