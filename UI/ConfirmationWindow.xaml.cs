using System.Windows;

namespace UI
{
    public partial class ConfirmationWindow : Window
    {
        public bool IsConfirmed { get; private set; }

        public ConfirmationWindow(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            DialogResult = true; // Asegúrate de establecer DialogResult para cerrar la ventana y devolver el resultado
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            DialogResult = false; // Asegúrate de establecer DialogResult para cerrar la ventana y devolver el resultado
            Close();
        }
    }
}
