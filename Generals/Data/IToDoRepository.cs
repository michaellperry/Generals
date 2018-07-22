using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generals.Models
{
    public interface IToDoRepository
    {
        Task<List<ToDoListRecord>> GetAllLists();
        Task<ToDoListRecord> GetListById(int listId);
        Task<ToDoListRecord> CreateList(ToDoListRecord record);

        Task<ToDoItemRecord> GetItemById(int itemId);
        Task<List<ToDoItemRecord>> GetItemsForList(int listId);
    }
}
