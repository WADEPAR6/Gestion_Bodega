using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Core.Entities;
using AppServices.Services;
using System.Windows.Controls;

namespace UI
{
    public partial class UpdateItemsForCategoryWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private readonly Categoria _category;
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Categoria> Categories { get; set; }
        public bool ItemsUpdated { get; private set; }

        public event Action ItemsUpdatedEvent;

        public UpdateItemsForCategoryWindow(ObservableCollection<Item> items, ObservableCollection<Categoria> categories, InventoryService inventoryService, Categoria category)
        {
            InitializeComponent();
            DataContext = this;
            Items = items;
            Categories = categories;
            _inventoryService = inventoryService;
            _category = category;
        }

        private void EliminarItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
            {
                var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar este ítem?");
                if (confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed)
                {
                    _inventoryService.RemoveItem(item.Id);
                    Items.Remove(item);
                }
            }
        }

        private async void ActualizarItems_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Items)
            {
                if (item.CategoriaID == _category.Id)
                {
                    MessageBox.Show("Debe mover los ítems a otra categoría antes de eliminar esta categoría.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _inventoryService.UpdateItem(item);
            }
            ItemsUpdated = true;
            MessageBox.Show("Todos los ítems han sido actualizados.");
            await Task.Delay(500); // Espera de 0.5 segundos
            ItemsUpdatedEvent?.Invoke(); // Notifica que los ítems han sido actualizados
            Close();
        }

        private void EliminarTodosItems_Click(object sender, RoutedEventArgs e)
        {
            var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar todos los ítems?");
            if (confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed)
            {
                foreach (var item in Items)
                {
                    _inventoryService.RemoveItem(item.Id);
                }
                Items.Clear();
                MessageBox.Show("Todos los ítems han sido eliminados.");
                ItemsUpdated = true;
                ItemsUpdatedEvent?.Invoke(); // Notifica que los ítems han sido eliminados
                Close();
            }
        }
    }
}
