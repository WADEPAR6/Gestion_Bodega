using System.Windows;
using Core.Entities;
using AppServices.Services;

namespace UI
{
    public partial class AddCategoriaWindow : Window
    {
        private readonly InventoryService _inventoryService;
        public Categoria Categoria { get; private set; }

        public AddCategoriaWindow(InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) || string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                ErrorMessage.Text = "Por favor complete todos los campos.";
                return;
            }

            Categoria = new Categoria
            {
                Nombre = NombreTextBox.Text,
                Descripcion = DescripcionTextBox.Text
            };

            try
            {
                _inventoryService.AddCategoria(Categoria);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Error al agregar la categoría: {ex.Message}";
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
