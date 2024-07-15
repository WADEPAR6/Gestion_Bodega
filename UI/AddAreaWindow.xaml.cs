using System.Windows;
using Core.Entities;

namespace UI
{
    public partial class AddAreaWindow : Window
    {
        public Area Area { get; private set; }

        public AddAreaWindow()
        {
            InitializeComponent();
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                ErrorMessage.Text = "No se admiten parámetros vacíos";
                return;
            }

            Area = new Area
            {
                Nombre = NombreTextBox.Text,
                Descripcion = DescripcionTextBox.Text
            };
            DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
