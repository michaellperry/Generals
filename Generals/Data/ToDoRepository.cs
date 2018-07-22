using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generals.Models
{
    public class ToDoRepository : IToDoRepository
    {
        private List<ToDoListRecord> _lists = new List<ToDoListRecord>();
        private List<ToDoItemRecord> _items = new List<ToDoItemRecord>();

        public ToDoRepository()
        {
            InitializeTables();
        }

        public Task<List<ToDoListRecord>> GetAllLists()
        {
            return Task.FromResult(_lists);
        }

        public Task<ToDoListRecord> GetListById(int listId)
        {
            return Task.FromResult(_lists.Where(i => i.Id == listId).SingleOrDefault());
        }

        public Task<List<ToDoItemRecord>> GetItemsForList(int listId)
        {
            return Task.FromResult(_items.Where(i => i.ListId == listId).ToList());
        }

        public Task<ToDoItemRecord> GetItemById(int itemId)
        {
            return Task.FromResult(_items.Where(i => i.Id == itemId).SingleOrDefault());
        }

        private void InitializeTables()
        {
            _lists.Add(new ToDoListRecord
            {
                Id = 32,
                Name = "Household"
            });
            _lists.Add(new ToDoListRecord
            {
                Id = 36,
                Name = "Shopping"
            });
            _lists.Add(new ToDoListRecord
            {
                Id = 47,
                Name = "Workshop"
            });

            _items.Add(new ToDoItemRecord
            {
                Id = 12,
                ListId = 32,
                Description = "Hang the towel rack",
                Done = false
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 16,
                ListId = 32,
                Description = "Paint the scrap room",
                Done = true
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 14,
                ListId = 36,
                Description = "Bread",
                Done = false
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 15,
                ListId = 36,
                Description = "Cheese",
                Done = true
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 17,
                ListId = 36,
                Description = "Eggs",
                Done = false
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 22,
                ListId = 47,
                Description = "Hang the pegboard",
                Done = true
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 24,
                ListId = 47,
                Description = "Sort the tools",
                Done = false
            });
        }
    }
}
