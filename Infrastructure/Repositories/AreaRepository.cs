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
            return connection.QuerySingleOrDefault<Area>("SELECT * FROM area WHERE id = @Id", new { Id = id });
        }

        public void AddArea(Area area)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Execute("INSERT INTO area (nombre, descripcion) VALUES (@Nombre, @Descripcion)", area);
        }

        public void UpdateArea(Area area)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Execute("UPDATE area SET nombre = @Nombre, descripcion = @Descripcion WHERE id = @Id", area);
        }

        public void RemoveArea(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Execute("DELETE FROM area WHERE id = @Id", new { Id = id });
        }
    }
}
