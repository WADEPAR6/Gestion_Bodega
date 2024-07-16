using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Core.Entities;
using AppServices.Services;
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

        private async void EliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Categoria category)
            {
                var items = _inventoryService.GetAllItems().Where(i => i.CategoriaID == category.Id).ToList();

                if (items.Any())
                {
                    var itemsObservable = new ObservableCollection<Item>(items);
                    var categoriesObservable = new ObservableCollection<Categoria>(_inventoryService.GetAllCategorias());

                    var updateItemsWindow = new UpdateItemsForCategoryWindow(itemsObservable, categoriesObservable, _inventoryService, category);
                    updateItemsWindow.ItemsUpdatedEvent += async () =>
                    {
                        await Task.Delay(500); // Espera de 0.5 segundos antes de eliminar la categoría
                        try
                        {
                            _inventoryService.RemoveCategoria(category.Id);
                            LoadCategories();
                            MessageBox.Show("Categoría eliminada con éxito.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al eliminar la categoría: {ex.Message}");
                        }
                    };

                    updateItemsWindow.ShowDialog();
                }
                else
                {
                    try
                    {
                        var confirmationWindow = new ConfirmationWindow("¿Estás seguro de que deseas eliminar esta categoría?");
                        if (confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed)
                        {
                            _inventoryService.RemoveCategoria(category.Id);
                            LoadCategories();
                            MessageBox.Show("Categoría eliminada con éxito.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar la categoría: {ex.Message}");
                    }
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
