using System.Collections.Generic;
using Core.Entities;
using Core.Interfaces;
using Dapper;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly string _connectionString;

        public AreaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Area> GetAllAreas()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return connection.Query<Area>("SELECT * FROM area");
        }

        public Area GetAreaById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "SELECT * FROM area WHERE Id = @Id";
            return connection.QuerySingleOrDefault<Area>(query, new { Id = id });
        }

        public void AddArea(Area area)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "INSERT INTO area (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
            connection.Execute(query, area);
        }

        public void UpdateArea(Area area)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "UPDATE area SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
            connection.Execute(query, area);
        }

        public void RemoveArea(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = "DELETE FROM area WHERE id = @Id";
            connection.Execute(query, new { Id = id });
        }

    }
}
