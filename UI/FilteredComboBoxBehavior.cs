using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    public static class FilteredComboBoxBehavior
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.RegisterAttached(
                "Items",
                typeof(IEnumerable<object>),
                typeof(FilteredComboBoxBehavior),
                new PropertyMetadata(null, OnItemsChanged));

        public static IEnumerable<object> GetItems(DependencyObject obj)
        {
            return (IEnumerable<object>)obj.GetValue(ItemsProperty);
        }

        public static void SetItems(DependencyObject obj, IEnumerable<object> value)
        {
            obj.SetValue(ItemsProperty, value);
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as ComboBox;
            if (comboBox != null)
            {
                comboBox.Loaded += (s, args) =>
                {
                    var allItems = GetItems(comboBox);
                    comboBox.ItemsSource = allItems;

                    var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
                    if (textBox != null)
                    {
                        textBox.TextChanged += (s1, e1) =>
                        {
                            var filteredItems = allItems
                                .OfType<object>()
                                .Where(item => item.ToString().ToLower().Contains(textBox.Text.ToLower()))
                                .ToList();

                            comboBox.ItemsSource = filteredItems;
                            comboBox.IsDropDownOpen = true;
                            textBox.Focus();
                            textBox.SelectionStart = textBox.Text.Length;
                        };
                    }
                };
            }
        }
    }
}
