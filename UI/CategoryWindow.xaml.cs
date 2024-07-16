using System.Windows;
using Core.Entities;
using AppServices.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace UI
{
    public partial class CategoryWindow : Window
    {
        private readonly InventoryService _inventoryService;
        private ObservableCollection<Categoria> _categories;

        public CategoryWindow(InventoryService inventoryService)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _inventoryService.GetAllCategorias();
            _categories = new ObservableCollection<Categoria>(categories);
            CategoriesListView.ItemsSource = _categories;
        }

        private void EditarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Categoria category)
            {
                var editCategoryWindow = new AddEditEntityWindow("Categoria", _inventoryService, category);
                if (editCategoryWindow.ShowDialog() == true)
                {
                    LoadCategories();
                }
            }
        }

        private void EliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Categoria category)
            {
                var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar esta categoría?");
                var result = confirmationWindow.ShowDialog();

                if (result == true && confirmationWindow.IsConfirmed)
                {
                    _inventoryService.RemoveCategoria(category.Id);
                    LoadCategories();
                }
            }
        }

        private void AgregarCategoria_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddEditEntityWindow("Categoria", _inventoryService);
            if (addCategoryWindow.ShowDialog() == true)
            {
                LoadCategories();
            }
        }
    }
}
