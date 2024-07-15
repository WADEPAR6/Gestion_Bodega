using Core.Entities;

namespace Core.Interfaces
{
    public interface IItemRepository
    {
        void AddItem(Item item);
        void RemoveItem(int id);
        void UpdateItem(Item item);
        Item GetItem(int id);
        IEnumerable<Item> GetAllItems();
    }
}