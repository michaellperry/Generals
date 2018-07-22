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
        private IToDoRepository _repository = ToDoRepository.Instance;

        [HttpGet]
        public async Task<ActionResult<List<ToDoListResponse>>> GetAll()
        {
            var lists = await _repository.GetAllLists();
            return lists.Select(ProjectList).ToList();
        }

        [HttpGet("{id}", Name = "GetListById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoListResponse>> GetById(int id)
        {
            var list = await _repository.GetListById(id);
            if (list == null)
                return NotFound();
            return ProjectList(list);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<ToDoListResponse>> Create([FromBody] ToDoListRequest list)
        {
            var record = await _repository.CreateList(ParseList(list));
            return CreatedAtRoute("GetListById", new { id = record.Id }, ProjectList(record));
        }

        private ToDoListResponse ProjectList(ToDoListRecord record)
        {
            return new ToDoListResponse
            {
                Id = record.Id,
                Name = record.Name
            };
        }

        private static ToDoListRecord ParseList(ToDoListRequest list)
        {
            return new ToDoListRecord
            {
                Name = list.Name
            };
        }
    }
}