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
            LoadCategorias(); // Inicializa las categorías
            LoadAreas(); // Inicializa las áreas
        }

        private void LoadItems()
        {
            var items = _inventoryService.GetAllItems() ?? new List<Item>();
            foreach (var item in items)
            {
                if (item.Area == null && item.ParentItem?.Area != null)
                {
                    item.Area = item.ParentItem.Area;
                }
            }
            _items = new ObservableCollection<Item>(items);
            ItemsListView.ItemsSource = _items;
        }

        private void LoadCategorias()
        {
            var categorias = _inventoryService.GetAllCategorias();
            SearchCategoryComboBox.ItemsSource = categorias;
        }

        private void LoadAreas()
        {
            // Si tienes un ComboBox u otro control para las áreas, actualízalo aquí
        }

        private void OpenAddItemWindow_Click(object sender, RoutedEventArgs e)
        {
            var categorias = _inventoryService.GetAllCategorias();
            var areas = _inventoryService.GetAllAreas();
            var parentItems = _inventoryService.GetAllItems();
            var addItemWindow = new AddItemWindow(categorias, areas, parentItems, _inventoryService);
            addItemWindow.ShowDialog();
            LoadItems();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
            {
                var categorias = _inventoryService.GetAllCategorias();
                var areas = _inventoryService.GetAllAreas();
                var parentItems = _inventoryService.GetAllItems();
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
                var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar este item?");
                var result = confirmationWindow.ShowDialog();

                if (result == true && confirmationWindow.IsConfirmed)
                {
                    _inventoryService.RemoveItem(item.Id);
                    LoadItems();
                }
            }
        }

        private void SearchNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_inventoryService == null || string.IsNullOrEmpty(SearchNameTextBox.Text))
            {
                return;
            }

            var searchName = SearchNameTextBox.Text.ToLower();
            var filteredItems = _items.Where(i => i.Nombre != null && i.Nombre.ToLower().Contains(searchName));
            ItemsListView.ItemsSource = new ObservableCollection<Item>(filteredItems);
        }

        private void SearchCategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SearchCategoryComboBox.SelectedItem is Categoria selectedCategory)
            {
                var filteredItems = _items.Where(i => i.CategoriaID == selectedCategory.Id);
                ItemsListView.ItemsSource = new ObservableCollection<Item>(filteredItems);
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
            var filteredItems = _items.Where(i => i.ParentItem != null && i.ParentItem.Nombre.ToLower().Contains(searchParentName));
            ItemsListView.ItemsSource = new ObservableCollection<Item>(filteredItems);
        }

        private void OpenCategoryWindow_Click(object sender, RoutedEventArgs e)
        {
            var categoryWindow = new CategoryWindow(_inventoryService);
            categoryWindow.ShowDialog();
            LoadCategorias(); // Recarga las categorías después de cerrar la ventana
        }

        private void OpenAreaWindow_Click(object sender, RoutedEventArgs e)
        {
            var areaWindow = new AreaWindow(_inventoryService);
            areaWindow.ShowDialog();
            LoadAreas(); // Recarga las áreas después de cerrar la ventana
        }

        private void EliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Categoria categoria)
            {
                var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar esta categoría?");
                var result = confirmationWindow.ShowDialog();

                if (result == true && confirmationWindow.IsConfirmed)
                {
                    _inventoryService.RemoveCategoria(categoria.Id);
                    LoadCategorias(); // Recargar las categorías después de eliminar
                }
            }
        }


    }
}
