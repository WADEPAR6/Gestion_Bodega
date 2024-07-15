using Core.Entities;
using Core.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddItem(Item item)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO items (nombre, cantidad, descripcion, categoriaid, areaid, parentid) VALUES (@Nombre, @Cantidad, @Descripcion, @CategoriaID, @AreaID, @ParentID)";
                db.Execute(sql, item);
            }
        }

        public void UpdateItem(Item item)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = "UPDATE items SET nombre = @Nombre, cantidad = @Cantidad, descripcion = @Descripcion, categoriaid = @CategoriaID, areaid = @AreaID, parentid = @ParentID WHERE id = @Id";
                db.Execute(sql, item);
            }
        }

        public void RemoveItem(int id)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = "DELETE FROM items WHERE id = @Id";
                db.Execute(sql, new { Id = id });
            }
        }

        public Item GetItem(int id)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = @"SELECT i.*, c.*, a.*, p.*
                    FROM items i
                    JOIN categoria c ON i.categoriaid = c.id
                    JOIN area a ON i.areaid = a.id
                    LEFT JOIN items p ON i.parentid = p.id
                    WHERE i.id = @Id";
                var result = db.Query<Item, Categoria, Area, Item, Item>(sql,
                    (item, categoria, area, parentItem) =>
                    {
                        item.Categoria = categoria;
                        item.Area = area;
                        item.ParentItem = parentItem;
                        return item;
                    },
                    new { Id = id }, splitOn: "id").FirstOrDefault();
                return result;
            }
        }


        public IEnumerable<Item> GetAllItems()
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = @"SELECT i.*, c.*, a.*, p.*
                            FROM items i
                            JOIN categoria c ON i.categoriaid = c.id
                            JOIN area a ON i.areaid = a.id
                            LEFT JOIN items p ON i.parentid = p.id";
                var result = db.Query<Item, Categoria, Area, Item, Item>(sql,
                    (item, categoria, area, parentItem) =>
                    {
                        item.Categoria = categoria;
                        item.Area = area;
                        item.ParentItem = parentItem;
                        return item;
                    },
                    splitOn: "id");
                return result;
            }
        }
    }
}
