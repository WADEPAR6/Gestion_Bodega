using System.Collections.Generic;
using Core.Entities;
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
            var query = "SELECT * FROM categoria WHERE Id = @Id";
            return connection.QuerySingleOrDefault<Categoria>(query, new { Id = id });
        }

        public void AddCategoria(Categoria categoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "INSERT INTO categoria (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
            connection.Execute(query, categoria);
        }

        public void UpdateCategoria(Categoria categoria)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "UPDATE categoria SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
            connection.Execute(query, categoria);
        }

        public void RemoveCategoria(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "DELETE FROM categoria WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }

    }
}
