using Generals.Controllers;
using System.Collections.Generic;

namespace Generals.Models
{
    public class ToDoListResponse
    {
        public string Name { get; set; }

        public Dictionary<string, Link> _links { get; set; }
    }
}
