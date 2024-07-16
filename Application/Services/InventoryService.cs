using System.Collections.Generic;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;

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

        public Item GetItem(int id)
        {
            return _itemRepository.GetItem(id);
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

        public Categoria GetCategoriaById(int id)
        {
            return _categoriaRepository.GetCategoriaById(id);
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

        public Area GetAreaById(int id)
        {
            return _areaRepository.GetAreaById(id);
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
