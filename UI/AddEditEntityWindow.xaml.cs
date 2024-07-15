using System.Windows;
using Core.Entities;
using AppServices.Services;

namespace UI
{
    public partial class AddEditEntityWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private readonly string _entityType;
        public object Entity { get; private set; }

        public AddEditEntityWindow(string entityType, InventoryService inventoryService, object entity = null)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
            _entityType = entityType;
            Entity = entity;

            if (entity != null)
            {
                if (entityType == "Categoria")
                {
                    var categoria = entity as Categoria;
                    NombreTextBox.Text = categoria.Nombre;
                    DescripcionTextBox.Text = categoria.Descripcion;
                }
                else if (entityType == "Area")
                {
                    var area = entity as Area;
                    NombreTextBox.Text = area.Nombre;
                    DescripcionTextBox.Text = area.Descripcion;
                }
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) || string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_entityType == "Categoria")
            {
                if (Entity == null)
                {
                    Entity = new Categoria();
                }

                var categoria = Entity as Categoria;
                categoria.Nombre = NombreTextBox.Text;
                categoria.Descripcion = DescripcionTextBox.Text;

                if (categoria.Id == 0)
                {
                    _inventoryService.AddCategoria(categoria);
                }
                else
                {
                    _inventoryService.UpdateCategoria(categoria);
                }
            }
            else if (_entityType == "Area")
            {
                if (Entity == null)
                {
                    Entity = new Area();
                }

                var area = Entity as Area;
                area.Nombre = NombreTextBox.Text;
                area.Descripcion = DescripcionTextBox.Text;

                if (area.Id == 0)
                {
                    _inventoryService.AddArea(area);
                }
                else
                {
                    _inventoryService.UpdateArea(area);
                }
            }

            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
