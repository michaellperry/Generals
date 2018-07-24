using Generals.Data;
using Generals.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Generals.Controllers
{
    [Produces("application/json")]
    [Route("api/ToDoList/{listIdentity}/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private const string DateTimeCodeFormat = "yyyyMMddHHmmssFFF";

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

        [HttpGet("{creationDateTimeCode}", Name = "GetItemByCreationDateTime")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> GetByCreationDateTime(string listIdentity, string creationDateTimeCode)
        {
            var creationDateTime = ParseDateTimeCode(creationDateTimeCode);
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var item = await _repository.GetItemByCreationDateTime(list.Id, creationDateTime);
            if (item == null)
                return NotFound();
            return ProjectItem(listIdentity, item);
        }

        [HttpPut("{creationDateTimeCode}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ToDoItemResponse>> Update(string listIdentity, string creationDateTimeCode, [FromBody] ToDoItemRequest request)
        {
            var creationDateTime = ParseDateTimeCode(creationDateTimeCode);
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var item = await _repository.GetItemByCreationDateTime(list.Id, creationDateTime);
            if (item == null)
            {
                item = await _repository.CreateItem(ParseItem(list.Id, creationDateTime, request));
                return CreatedAtRoute("GetItemByCreationDateTime", new { listIdentity, creationDateTimeCode = FormatDateTimeCode(item.CreationDateTime) }, ProjectItem(listIdentity, item));
            }
            else if (item.LastUpdateDateTime < request.UpdateDateTime)
            {
                ParseOntoItem(request, item);
                await _repository.SaveChanges();
            }
            return ProjectItem(listIdentity, item);
        }

        [HttpDelete("{creationDateTimeCode}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(string listIdentity, string creationDateTimeCode)
        {
            var creationDateTime = ParseDateTimeCode(creationDateTimeCode);
            var list = await _repository.GetListByIdentity(listIdentity);
            if (list == null)
                return NotFound();
            var item = await _repository.GetItemByCreationDateTime(list.Id, creationDateTime);
            if (item == null)
                return NotFound();
            await _repository.DeleteItem(list.Id, item.Id);
            return NoContent();
        }

        private ToDoItemResponse ProjectItem(string listIdentity, ToDoItemRecord item)
        {
            return new ToDoItemResponse
            {
                Description = item.Description,
                Done = item.Done,
                _links = new Dictionary<string, Link>
                {
                    { "self", new Link(Url.RouteUrl("GetItemByCreationDateTime", new { listIdentity, creationDateTimeCode = FormatDateTimeCode(item.CreationDateTime) })) },
                    { "collection", new Link(Url.RouteUrl("GetItemsByListIdentity", new { listIdentity })) },
                    { "list", new Link(Url.RouteUrl("GetListByIdentity", new { identity = listIdentity })) },
                }
            };
        }

        private DateTime ParseDateTimeCode(string dateTimeCode)
        {
            if (!DateTime.TryParseExact(
                dateTimeCode,
                DateTimeCodeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var result))
                throw new ArgumentException("Invalid date time code");
            return result;
        }

        private string FormatDateTimeCode(DateTime creationDateTime)
        {
            string str = creationDateTime.ToString(DateTimeCodeFormat, CultureInfo.InvariantCulture);
            return str;
        }

        private ToDoItemRecord ParseItem(int listId, DateTime creationDateTime, ToDoItemRequest request)
        {
            var item = new ToDoItemRecord()
            {
                ListId = listId,
                CreationDateTime = creationDateTime
            };
            ParseOntoItem(request, item);
            return item;
        }

        private void ParseOntoItem(ToDoItemRequest request, ToDoItemRecord item)
        {
            item.Description = request.Description;
            item.Done = request.Done;
            item.LastUpdateDateTime = request.UpdateDateTime;
        }
    }
}