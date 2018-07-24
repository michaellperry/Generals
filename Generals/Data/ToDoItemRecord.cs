using System;

namespace Generals.Data
{
    public class ToDoItemRecord
    {
        public DateTime CreationDateTime { get; set; }
        public int Id { get; set; }
        public int ListId { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
    }
}