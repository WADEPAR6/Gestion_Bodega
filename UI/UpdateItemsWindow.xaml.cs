using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Core.Entities;
using AppServices.Services;
using System.Windows.Controls;

namespace UI
{
    public partial class UpdateItemsWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private readonly Area _area;
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Area> Areas { get; set; }
        public static int CurrentAreaId { get; private set; }
        public bool ItemsUpdated { get; private set; }

        public event Action ItemsUpdatedEvent;

        public UpdateItemsWindow(ObservableCollection<Item> items, ObservableCollection<Area> areas, InventoryService inventoryService, Area area)
        {
            InitializeComponent();
            DataContext = this;
            Items = items;
            Areas = areas;
            _inventoryService = inventoryService;
            _area = area;
            CurrentAreaId = area.Id;
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
                if (item.AreaID == _area.Id)
                {
                    MessageBox.Show("Debe mover los ítems a otra área antes de eliminar esta área.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
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
