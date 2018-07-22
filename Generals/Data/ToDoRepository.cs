using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generals.Data
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

        public Task<ToDoListRecord> CreateList(ToDoListRecord list)
        {
            list.Id = _lists.Max(l => l.Id) + 1;
            _lists.Add(list);
            return Task.FromResult(list);
        }

        public Task DeleteList(int listId)
        {
            _lists.RemoveAll(l => l.Id == listId);
            // Cascade delete.
            _items.RemoveAll(i => i.ListId == listId);
            return Task.CompletedTask;
        }

        public Task<List<ToDoItemRecord>> GetItemsForList(int listId)
        {
            if (!_lists.Any(l => l.Id == listId))
                return Task.FromResult((List<ToDoItemRecord>)null);
            return Task.FromResult(_items.Where(i => i.ListId == listId).ToList());
        }

        public Task<ToDoItemRecord> GetItemById(int listId, int itemId)
        {
            return Task.FromResult(_items
                .Where(i => i.ListId == listId && i.Id == itemId)
                .SingleOrDefault());
        }

        public Task<ToDoItemRecord> CreateItem(ToDoItemRecord item)
        {
            item.Id = _items.Max(i => i.Id) + 1;
            _items.Add(item);
            return Task.FromResult(item);
        }

        public Task DeleteItem(int listId, int id)
        {
            _items.RemoveAll(i => i.Id == id);
            return Task.CompletedTask;
        }

        public Task SaveChanges()
        {
            // Not necessary.
            return Task.CompletedTask;
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
