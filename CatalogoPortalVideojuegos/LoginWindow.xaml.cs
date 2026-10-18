using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace CatalogoPortalVideojuegos
{
    public partial class LoginWindow : Window
    {
        // Usuarios en memoria: [fila, 0] = usuario, [fila, 1] = contraseña
        private readonly string[,] usuarios = { { "admin", "1234" }, { "dam", "2026" } };

        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;
        private readonly DispatcherTimer timerBloqueo = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };

        public LoginWindow()
        {
            InitializeComponent();
            timerBloqueo.Tick += (s, e) => DesbloquearAcceso();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Password;

            // TC04: campos vacíos
            if (user == "" || pass == "")
            {
                Mostrar("Introduzca los datos requeridos", false);
                return;
            }

            // Buscar el usuario en la matriz
            int fila = -1;
            for (int i = 0; i < usuarios.GetLength(0); i++)
                if (usuarios[i, 0] == user) { fila = i; break; }

            // TC03: usuario inexistente
            if (fila == -1)
            {
                Mostrar("El usuario introducido no existe", false);
                txtUsuario.Clear(); txtPassword.Clear(); txtUsuario.Focus();
                return;
            }

            // TC01: login correcto
            if (usuarios[fila, 1] == pass)
            {
                intentosFallidos = 0;
                new MainWindow().Show();   // abre el catálogo
                this.Close();              // cierra el login
                return;
            }

            // TC02 y TC05: contraseña incorrecta
            intentosFallidos++;
            if (intentosFallidos >= MAX_INTENTOS)
            {
                BloquearAcceso();
            }
            else
            {
                Mostrar($"La contraseña introducida no es correcta (intento {intentosFallidos}/{MAX_INTENTOS})", false);
                txtPassword.Clear(); txtPassword.Focus();
            }
        }

        private void BloquearAcceso()
        {
            txtUsuario.IsEnabled = txtPassword.IsEnabled = btnLogin.IsEnabled = false;
            Mostrar("Demasiados intentos. Acceso bloqueado 30 segundos.", false);
            timerBloqueo.Start();
        }

        private void DesbloquearAcceso()
        {
            timerBloqueo.Stop();
            intentosFallidos = 0;
            txtUsuario.IsEnabled = txtPassword.IsEnabled = btnLogin.IsEnabled = true;
            txtPassword.Clear();
            Mostrar("Acceso desbloqueado. Inténtelo de nuevo.", true);
        }

        private void Mostrar(string texto, bool ok)
        {
            lblMensaje.Text = texto;
            lblMensaje.Foreground = ok ? Brushes.LightGreen : Brushes.IndianRed;
            // El plan de pruebas pide "mensaje por ventana", así que también usamos MessageBox
            if (!ok) MessageBox.Show(texto, "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        // TC07: salir de la aplicación
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}