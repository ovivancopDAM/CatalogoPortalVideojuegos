using System.Windows;

namespace CatalogoPortalVideojuegos
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        // TC06: cerrar sesión y volver al login
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();   // vuelve al login
            this.Close();               // cierra el catálogo
        }
    }
}