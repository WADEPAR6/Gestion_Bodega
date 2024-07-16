using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace UI
{
    public static class PlaceholderTextExtension
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(PlaceholderTextExtension), new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public static string GetPlaceholder(UIElement element)
        {
            return (string)element.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(UIElement element, string value)
        {
            element.SetValue(PlaceholderProperty, value);
        }

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus += RemovePlaceholder;
                textBox.LostFocus += ShowPlaceholder;
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    RemovePlaceholder(textBox, null);
                }
                else
                {
                    ShowPlaceholder(textBox, null);
                }
            }
        }

        private static void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == GetPlaceholder(textBox))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private static void ShowPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = GetPlaceholder(textBox);
                textBox.Foreground = SystemColors.GrayTextBrush;
            }
        }
    }
}
