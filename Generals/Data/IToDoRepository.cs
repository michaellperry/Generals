using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generals.Data
{
    public interface IToDoRepository
    {
        Task<List<ToDoListRecord>> GetAllLists();
        Task<ToDoListRecord> GetListById(int listId);
        Task<ToDoListRecord> CreateList(ToDoListRecord list);
        Task DeleteList(int listId);

        Task<List<ToDoItemRecord>> GetItemsForList(int listId);
        Task<ToDoItemRecord> GetItemById(int listId, int itemId);
        Task<ToDoItemRecord> CreateItem(ToDoItemRecord item);
        Task DeleteItem(int listId, int id);

        Task SaveChanges();
    }
}
