using Generals.Controllers;
using System.Collections.Generic;

namespace Generals.Models
{
    public class ToDoItemResponse
    {
        public string Description { get; set; }
        public bool Done { get; set; }

        public Dictionary<string, Link> _links { get; set; }
    }
}