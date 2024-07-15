using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public int? CategoriaID { get; set; }
        public Categoria Categoria { get; set; }
        public int? AreaID { get; set; }
        public Area Area { get; set; }
        public int? ParentItemID { get; set; }
        public Item ParentItem { get; set; }
    }
}
