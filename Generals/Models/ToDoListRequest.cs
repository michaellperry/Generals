using System;

namespace Generals.Controllers
{
    public class ToDoListRequest
    {
        public string Name { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
