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

        public Task<ToDoListRecord> GetListByIdentity(string listIdentity)
        {
            return Task.FromResult(_lists.Where(i => i.Identity == listIdentity).SingleOrDefault());
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
            return Task.FromResult(_items.Where(i => i.ListId == listId).ToList());
        }

        public Task<ToDoItemRecord> GetItemByCreationDateTime(int listId, DateTime creationDateTime)
        {
            return Task.FromResult(_items
                .Where(i => i.ListId == listId && i.CreationDateTime == creationDateTime)
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
                Identity = "df076a6",
                Name = "Household"
            });
            _lists.Add(new ToDoListRecord
            {
                Id = 36,
                Identity = "fa1b053",
                Name = "Shopping"
            });
            _lists.Add(new ToDoListRecord
            {
                Id = 47,
                Identity = "1f7b6cc",
                Name = "Workshop"
            });

            _items.Add(new ToDoItemRecord
            {
                Id = 12,
                CreationDateTime = new DateTime(2018, 7, 1, 13, 41, 23, 312, DateTimeKind.Utc),
                ListId = 32,
                Description = "Hang the towel rack",
                Done = false
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 16,
                CreationDateTime = new DateTime(2018, 7, 1, 13, 52, 13, 34, DateTimeKind.Utc),
                ListId = 32,
                Description = "Paint the scrap room",
                Done = true
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 14,
                CreationDateTime = new DateTime(2018, 7, 1, 13, 46, 41, 923, DateTimeKind.Utc),
                ListId = 36,
                Description = "Bread",
                Done = false
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 15,
                CreationDateTime = new DateTime(2018, 7, 1, 13, 46, 47, 314, DateTimeKind.Utc),
                ListId = 36,
                Description = "Cheese",
                Done = true
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 17,
                CreationDateTime = new DateTime(2018, 7, 1, 14, 32, 1, 412, DateTimeKind.Utc),
                ListId = 36,
                Description = "Eggs",
                Done = false
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 22,
                CreationDateTime = new DateTime(2018, 7, 5, 4, 31, 17, 312, DateTimeKind.Utc),
                ListId = 47,
                Description = "Hang the pegboard",
                Done = true
            });
            _items.Add(new ToDoItemRecord
            {
                Id = 24,
                CreationDateTime = new DateTime(2018, 7, 5, 6, 32, 14, 145, DateTimeKind.Utc),
                ListId = 47,
                Description = "Sort the tools",
                Done = false
            });
        }
    }
}
