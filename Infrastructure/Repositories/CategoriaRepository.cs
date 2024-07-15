using System.Collections.Generic;
using Core.Entities;
using Core.Interfaces;
using Dapper;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Categoria> GetAllCategorias()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return connection.Query<Categoria>("SELECT * FROM categoria");
        }

        public Categoria GetCategoriaById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return connection.QuerySingleOrDefault<Categoria>("SELECT * FROM categoria WHERE id = @Id", new { Id = id });
        }

        public void AddCategoria(Categoria categoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Execute("INSERT INTO categoria (nombre, descripcion) VALUES (@Nombre, @Descripcion)", categoria);
        }

        public void UpdateCategoria(Categoria categoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Execute("UPDATE categoria SET nombre = @Nombre, descripcion = @Descripcion WHERE id = @Id", categoria);
        }

        public void RemoveCategoria(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Execute("DELETE FROM categoria WHERE id = @Id", new { Id = id });
        }
    }
}
