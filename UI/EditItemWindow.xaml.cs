using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core.Entities;
using AppServices.Services;

namespace UI
{
    public partial class EditItemWindow : Window
    {
        private readonly InventoryService _inventoryService;
        public Item Item { get; private set; }

        public EditItemWindow(Item item, IEnumerable<Categoria> categorias, IEnumerable<Area> areas, IEnumerable<Item> parentItems, InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
            Item = item;

            CategoriaComboBox.ItemsSource = categorias;
            AreaComboBox.ItemsSource = areas;
            ParentItemComboBox.ItemsSource = parentItems;

            NombreTextBox.Text = item.Nombre;
            CantidadTextBox.Text = item.Cantidad.ToString();
            DescripcionTextBox.Text = item.Descripcion;

            if (item.Categoria != null)
            {
                CategoriaComboBox.SelectedItem = categorias.FirstOrDefault(c => c.Id == item.CategoriaID);
            }

            if (item.Area != null)
            {
                AreaComboBox.SelectedItem = areas.FirstOrDefault(a => a.Id == item.AreaID);
            }

            if (item.ParentItem != null)
            {
                ParentItemComboBox.SelectedItem = parentItems.FirstOrDefault(p => p.Id == item.ParentID);
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) || string.IsNullOrWhiteSpace(CantidadTextBox.Text) || string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Item.Nombre = NombreTextBox.Text;
            Item.Cantidad = int.Parse(CantidadTextBox.Text);
            Item.Descripcion = DescripcionTextBox.Text;

            if (CategoriaComboBox.SelectedItem is Categoria selectedCategoria)
            {
                Item.CategoriaID = selectedCategoria.Id;
            }

            if (AreaComboBox.SelectedItem is Area selectedArea)
            {
                Item.AreaID = selectedArea.Id;
            }

            if (ParentItemComboBox.SelectedItem is Item selectedParentItem)
            {
                Item.ParentID = selectedParentItem.Id;
            }
            else
            {
                Item.ParentID = null;
            }

            _inventoryService.UpdateItem(Item);
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
