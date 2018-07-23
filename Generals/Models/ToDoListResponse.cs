using Generals.Controllers;
using System.Collections.Generic;

namespace Generals.Models
{
    public class ToDoListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Dictionary<string, Link> _links { get; set; }
    }
}
