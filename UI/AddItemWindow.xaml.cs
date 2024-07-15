using System.Collections.Generic;
using System.Windows;
using Core.Entities;
using AppServices.Services;

namespace UI
{
    public partial class AddItemWindow : Window
    {
        private readonly InventoryService _inventoryService;
        public Item Item { get; private set; }

        public AddItemWindow(IEnumerable<Categoria> categorias, IEnumerable<Area> areas, IEnumerable<Item> parentItems, InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;

            CategoriaComboBox.ItemsSource = categorias;
            AreaComboBox.ItemsSource = areas;
            ParentItemComboBox.ItemsSource = parentItems;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) || string.IsNullOrWhiteSpace(CantidadTextBox.Text) || string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Item = new Item
            {
                Nombre = NombreTextBox.Text,
                Cantidad = int.Parse(CantidadTextBox.Text),
                Descripcion = DescripcionTextBox.Text,
                CategoriaID = ((Categoria)CategoriaComboBox.SelectedItem)?.Id,
                AreaID = ((Area)AreaComboBox.SelectedItem)?.Id,
                ParentItemID = ((Item)ParentItemComboBox.SelectedItem)?.Id
            };

            try
            {
                _inventoryService.AddItem(Item);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
