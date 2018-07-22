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
        public async Task<ActionResult<ToDoListResponse>> Create([FromBody] ToDoListRequest request)
        {
            var record = await _repository.CreateList(ParseList(request));
            return CreatedAtRoute("GetListById", new { id = record.Id }, ProjectList(record));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoListResponse>> Update(int id, [FromBody] ToDoListRequest request)
        {
            var list = await _repository.GetListById(id);
            if (list == null)
                return NotFound();
            ParseOntoList(request, list);
            await _repository.SaveChanges();
            return ProjectList(list);
        }

        private ToDoListResponse ProjectList(ToDoListRecord list)
        {
            return new ToDoListResponse
            {
                Id = list.Id,
                Name = list.Name
            };
        }

        private ToDoListRecord ParseList(ToDoListRequest request)
        {
            var list = new ToDoListRecord();
            ParseOntoList(request, list);
            return list;
        }

        private void ParseOntoList(ToDoListRequest request, ToDoListRecord list)
        {
            list.Name = request.Name;
        }
    }
}