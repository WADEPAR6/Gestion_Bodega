using System.Collections.Generic;
using Core.Entities;

namespace Infrastructure.Repositories
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetAllCategorias();
        Categoria GetCategoriaById(int id);
        void AddCategoria(Categoria categoria);
        void UpdateCategoria(Categoria categoria);
        void RemoveCategoria(int id);
    }
}
