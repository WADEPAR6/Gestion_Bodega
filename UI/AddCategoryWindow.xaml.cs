using System.Windows;
using Core.Entities;
using AppServices.Services;

namespace UI
{
    public partial class AddCategoryWindow : Window
    {
        private readonly InventoryService _inventoryService;
        public Categoria Categoria { get; private set; }

        public AddCategoryWindow(InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) || string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Categoria = new Categoria
            {
                Nombre = NombreTextBox.Text,
                Descripcion = DescripcionTextBox.Text
            };

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
