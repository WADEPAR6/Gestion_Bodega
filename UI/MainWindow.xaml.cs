using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core.Entities;
using AppServices.Services;
using Infrastructure.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private ObservableCollection<Item> _items;

        public MainWindow()
        {
            InitializeComponent();
            var connectionString = "Host=localhost;Username=postgres;Password=root;Database=bodega";
            var itemRepository = new ItemRepository(connectionString);
            var categoriaRepository = new CategoriaRepository(connectionString);
            var areaRepository = new AreaRepository(connectionString);
            _inventoryService = new InventoryService(itemRepository, categoriaRepository, areaRepository);
            LoadItems();
            SearchCategoryComboBox.ItemsSource = _inventoryService.GetAllCategorias();
        }

        private void LoadItems()
        {
            var items = _inventoryService.GetAllItems() ?? new List<Item>();
            _items = new ObservableCollection<Item>(items);
            ItemsListView.ItemsSource = _items;
        }

        private void OpenAddItemWindow_Click(object sender, RoutedEventArgs e)
        {
            var categorias = _inventoryService.GetAllCategorias();
            var areas = _inventoryService.GetAllAreas();
            var parentItems = _inventoryService.GetAllItems(); // Obtener todos los items para seleccionar un item padre
            var addItemWindow = new AddItemWindow(categorias, areas, parentItems, _inventoryService);
            addItemWindow.ShowDialog();
            LoadItems(); // Refrescar la lista después de cerrar la ventana
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
            {
                var categorias = _inventoryService.GetAllCategorias();
                var areas = _inventoryService.GetAllAreas();
                var parentItems = _inventoryService.GetAllItems(); // Obtener todos los items para seleccionar un item padre
                var editItemWindow = new EditItemWindow(item, categorias, areas, parentItems, _inventoryService);
                if (editItemWindow.ShowDialog() == true)
                {
                    _inventoryService.UpdateItem(editItemWindow.Item);
                    LoadItems();
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
            {
                _inventoryService.RemoveItem(item.Id);
                LoadItems();
            }
        }

        private void SearchNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_inventoryService == null || string.IsNullOrEmpty(SearchNameTextBox.Text))
            {
                return;
            }

            var searchName = SearchNameTextBox.Text.ToLower();
            var filteredItems = _inventoryService.GetAllItems()
                .Where(i => i.Nombre != null && i.Nombre.ToLower().Contains(searchName));
            _items = new ObservableCollection<Item>(filteredItems);
            ItemsListView.ItemsSource = _items;
        }

        private void SearchCategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SearchCategoryComboBox.SelectedItem is Categoria selectedCategory)
            {
                var filteredItems = _inventoryService.GetAllItems().Where(i => i.CategoriaID == selectedCategory.Id);
                _items = new ObservableCollection<Item>(filteredItems);
                ItemsListView.ItemsSource = _items;
            }
        }

        private void ShowAllItems_Click(object sender, RoutedEventArgs e)
        {
            SearchNameTextBox.Text = string.Empty;
            SearchCategoryComboBox.SelectedIndex = -1;
            LoadItems();
        }

        private void SearchParentTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_inventoryService == null || string.IsNullOrEmpty(SearchParentTextBox.Text))
            {
                return;
            }

            var searchParentName = SearchParentTextBox.Text.ToLower();
            var filteredItems = _inventoryService.GetAllItems()
                .Where(i => i.ParentItem != null && i.ParentItem.Nombre != null && i.ParentItem.Nombre.ToLower().Contains(searchParentName));
            _items = new ObservableCollection<Item>(filteredItems);
            ItemsListView.ItemsSource = _items;
        }

        private void OpenCategoryWindow_Click(object sender, RoutedEventArgs e)
        {
            var categoryWindow = new CategoryWindow(_inventoryService);
            categoryWindow.ShowDialog();
        }

        private void OpenAreaWindow_Click(object sender, RoutedEventArgs e)
        {
            var areaWindow = new AreaWindow(_inventoryService);
            areaWindow.ShowDialog();
        }
    }
}
