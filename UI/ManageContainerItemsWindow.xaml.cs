using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Core.Entities;
using AppServices.Services;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;

namespace UI
{
    public partial class ManageContainerItemsWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private readonly Item _container;
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> Containers { get; set; }
        public static int CurrentContainerId { get; private set; }
        public bool ItemsUpdated { get; private set; }

        public event Action ItemsUpdatedEvent;

        public ManageContainerItemsWindow(ObservableCollection<Item> items, ObservableCollection<Item> containers, InventoryService inventoryService, Item container)
        {
            InitializeComponent();
            DataContext = this;
            Items = items;
            Containers = containers;
            _inventoryService = inventoryService;
            _container = container;
            CurrentContainerId = container.Id;
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
                if (item.ParentID == _container.Id)
                {
                    MessageBox.Show("Debe mover los ítems a otro contenedor antes de eliminar este contenedor.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void EliminarContenedor_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Items)
            {
                if (item.ParentID == _container.Id)
                {
                    MessageBox.Show("Debe mover todos los ítems a otro contenedor o seleccionar 'Sin contenedor' antes de eliminar este contenedor.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar este contenedor?");
            if (confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed)
            {
                foreach (var item in Items)
                {
                    item.ParentID = null; // Remueve la asociación con el contenedor
                    _inventoryService.UpdateItem(item);
                }
                _inventoryService.RemoveItem(_container.Id);
                ItemsUpdated = true;
                ItemsUpdatedEvent?.Invoke(); // Notifica que el contenedor ha sido eliminado
                Close();
            }
        }
    }
}
