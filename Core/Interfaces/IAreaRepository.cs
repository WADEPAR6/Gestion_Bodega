using Core.Entities;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IAreaRepository
    {
        IEnumerable<Area> GetAllAreas();
        Area GetAreaById(int id);
        void AddArea(Area area);
        void UpdateArea(Area area);
        void RemoveArea(int id);
    }
}
