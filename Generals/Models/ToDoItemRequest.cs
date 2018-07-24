using System;

namespace Generals.Models
{
    public class ToDoItemRequest
    {
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
