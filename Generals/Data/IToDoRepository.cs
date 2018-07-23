using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generals.Data
{
    public interface IToDoRepository
    {
        Task<List<ToDoListRecord>> GetAllLists();
        Task<ToDoListRecord> GetListByIdentity(string listIdentity);
        Task<ToDoListRecord> CreateList(ToDoListRecord list);
        Task DeleteList(int listId);

        Task<List<ToDoItemRecord>> GetItemsForList(int listId);
        Task<ToDoItemRecord> GetItemByCreationDateTime(int listId, DateTime creationDateTime);
        Task<ToDoItemRecord> CreateItem(ToDoItemRecord item);
        Task DeleteItem(int listId, int id);

        Task SaveChanges();
    }
}
