using System.Windows;
using Core.Entities;
using AppServices.Services;

namespace UI
{
    public partial class EditCategoryWindow : Window
    {
        private readonly InventoryService _inventoryService;
        public Categoria Categoria { get; private set; }

        public EditCategoryWindow(Categoria categoria, InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
            Categoria = categoria;
            NombreTextBox.Text = categoria.Nombre;
            DescripcionTextBox.Text = categoria.Descripcion;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) || string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Categoria.Nombre = NombreTextBox.Text;
            Categoria.Descripcion = DescripcionTextBox.Text;

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
