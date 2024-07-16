using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Core.Entities;
using AppServices.Services;
using System.Windows.Controls;

namespace UI
{
    public partial class AreaWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private ObservableCollection<Area> _areas;

        public AreaWindow(InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
            LoadAreas();
        }

        private void LoadAreas()
        {
            var areas = _inventoryService.GetAllAreas();
            _areas = new ObservableCollection<Area>(areas);
            AreasListView.ItemsSource = _areas;
        }

        private void EditarArea_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Area area)
            {
                var editAreaWindow = new AddEditEntityWindow("Area", _inventoryService, area);
                if (editAreaWindow.ShowDialog() == true)
                {
                    LoadAreas();
                }
            }
        }

        private async void EliminarArea_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Area area)
            {
                var items = _inventoryService.GetAllItems().Where(i => i.AreaID == area.Id).ToList();

                if (items.Any())
                {
                    var itemsObservable = new ObservableCollection<Item>(items);
                    var areasObservable = new ObservableCollection<Area>(_inventoryService.GetAllAreas());

                    var updateItemsWindow = new UpdateItemsWindow(itemsObservable, areasObservable, _inventoryService, area);
                    updateItemsWindow.ItemsUpdatedEvent += async () =>
                    {
                        await Task.Delay(500); // Espera de 0.5 segundos antes de eliminar el área
                        try
                        {
                            _inventoryService.RemoveArea(area.Id);
                            LoadAreas();
                            MessageBox.Show("Área eliminada con éxito.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al eliminar el área: {ex.Message}");
                        }
                    };

                    updateItemsWindow.ShowDialog();
                }
                else
                {
                    try
                    {
                        var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar esta área?");
                        if (confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed)
                        {
                            _inventoryService.RemoveArea(area.Id);
                            LoadAreas();
                            MessageBox.Show("Área eliminada con éxito.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar el área: {ex.Message}");
                    }
                }
            }
        }

        private void AgregarArea_Click(object sender, RoutedEventArgs e)
        {
            var addAreaWindow = new AddEditEntityWindow("Area", _inventoryService);
            if (addAreaWindow.ShowDialog() == true)
            {
                LoadAreas();
            }
        }
    }
}
