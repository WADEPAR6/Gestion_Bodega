using System.Collections.Generic;
using Core.Entities;
using Core.Interfaces;

namespace AppServices.Services
{
    public class InventoryService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IAreaRepository _areaRepository;

        public InventoryService(IItemRepository itemRepository, ICategoriaRepository categoriaRepository, IAreaRepository areaRepository)
        {
            _itemRepository = itemRepository;
            _categoriaRepository = categoriaRepository;
            _areaRepository = areaRepository;
        }

        // Métodos para Item
        public IEnumerable<Item> GetAllItems()
        {
            return _itemRepository.GetAllItems();
        }

        public void AddItem(Item item)
        {
            _itemRepository.AddItem(item);
        }

        public void UpdateItem(Item item)
        {
            _itemRepository.UpdateItem(item);
        }

        public void RemoveItem(int id)
        {
            _itemRepository.RemoveItem(id);
        }

        // Métodos para Categoria
        public IEnumerable<Categoria> GetAllCategorias()
        {
            return _categoriaRepository.GetAllCategorias();
        }

        public void AddCategoria(Categoria categoria)
        {
            _categoriaRepository.AddCategoria(categoria);
        }

        public void UpdateCategoria(Categoria categoria)
        {
            _categoriaRepository.UpdateCategoria(categoria);
        }

        public void RemoveCategoria(int id)
        {
            _categoriaRepository.RemoveCategoria(id);
        }

        // Métodos para Area
        public IEnumerable<Area> GetAllAreas()
        {
            return _areaRepository.GetAllAreas();
        }

        public void AddArea(Area area)
        {
            _areaRepository.AddArea(area);
        }

        public void UpdateArea(Area area)
        {
            _areaRepository.UpdateArea(area);
        }

        public void RemoveArea(int id)
        {
            _areaRepository.RemoveArea(id);
        }
    }
}
