using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generals.Models
{
    public interface IToDoRepository
    {
        Task<List<ToDoListRecord>> GetAllLists();
        Task<ToDoItemRecord> GetItemById(int itemId);
        Task<List<ToDoItemRecord>> GetItemsForList(int listId);
        Task<ToDoListRecord> GetListById(int listId);
    }
}
