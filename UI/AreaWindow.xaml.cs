﻿using System.Windows;
using Core.Entities;
using AppServices.Services;
using System.Collections.ObjectModel;
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

        private void EliminarArea_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Area area)
            {
                _inventoryService.RemoveArea(area.Id);
                LoadAreas();
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
