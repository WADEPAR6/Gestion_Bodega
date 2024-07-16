using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Interfaces;
using Dapper;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Item> GetAllItems()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = @"
                SELECT i.*, c.*, a.*, p.*, pa.*
                FROM items i
                LEFT JOIN categoria c ON i.CategoriaID = c.Id
                LEFT JOIN area a ON i.AreaID = a.Id
                LEFT JOIN items p ON i.ParentID = p.Id
                LEFT JOIN area pa ON p.AreaID = pa.Id";
            var items = connection.Query<Item, Categoria, Area, Item, Area, Item>(
                query,
                (item, categoria, area, parentItem, parentArea) =>
                {
                    item.Categoria = categoria;
                    item.Area = area;
                    item.ParentItem = parentItem;
                    if (item.ParentItem != null)
                    {
                        item.ParentItem.Area = parentArea;
                    }
                    return item;
                });
            return items;
        }

        public Item GetItem(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = @"
                SELECT i.*, c.*, a.*, p.*, pa.*
                FROM items i
                LEFT JOIN categoria c ON i.CategoriaID = c.Id
                LEFT JOIN area a ON i.AreaID = a.Id
                LEFT JOIN items p ON i.ParentID = p.Id
                LEFT JOIN area pa ON p.AreaID = pa.Id
                WHERE i.Id = @Id";
            var item = connection.Query<Item, Categoria, Area, Item, Area, Item>(
                query,
                (item, categoria, area, parentItem, parentArea) =>
                {
                    item.Categoria = categoria;
                    item.Area = area;
                    item.ParentItem = parentItem;
                    if (item.ParentItem != null)
                    {
                        item.ParentItem.Area = parentArea;
                    }
                    return item;
                }, new { Id = id }).FirstOrDefault();
            return item;
        }

        public void AddItem(Item item)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "INSERT INTO items (Nombre, Cantidad, Descripcion, CategoriaID, AreaID, ParentID) " +
                        "VALUES (@Nombre, @Cantidad, @Descripcion, @CategoriaID, @AreaID, @ParentID)";
            connection.Execute(query, item);
        }

        public void UpdateItem(Item item)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "UPDATE items SET Nombre = @Nombre, Cantidad = @Cantidad, Descripcion = @Descripcion, " +
                        "CategoriaID = @CategoriaID, AreaID = @AreaID, ParentID = @ParentID WHERE Id = @Id";
            connection.Execute(query, item);
        }

        public void RemoveItem(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "DELETE FROM items WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }

    }
}
