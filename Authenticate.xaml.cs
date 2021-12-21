using System.Windows;

namespace ADMUtil
{
    /// <summary>
    /// Get the ADM host, username, and password to login, with an option to use http if ADM server
    /// certificates aren't trusted.
    /// </summary>
    public partial class Authenticate : Window
    {
        public Authenticate()
        {
            Globals.Continue = false;
            InitializeComponent();
            this.Title = "ADMUtil v" + Globals.version + ": Login";
            if (Globals.ADMHost != null)
            {
                ADMHost.Text = Globals.ADMHost;
            }
            else
            {
                ADMHost.Text = "adm";
            }
            if (Globals.ADMUser != null)
            {
                ADMUser.Text = Globals.ADMUser;
            }
            else
            {
                ADMUser.Text = "username";
            }
            ADMPass.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.ADMHost = ADMHost.Text;
            Globals.ADMUser = ADMUser.Text;
            Globals.ADMPass = ADMPass.Password;
            if (Globals.ADMPass.Length < 1)
            {
                MessageBox.Show("Please enter a password.");
            }
            else
            {
                if (CheckBox1.IsChecked == false)
                {
                    Globals.HttpType = "http://";
                }
                else
                {
                    Globals.HttpType = "https://";
                }
                Globals.Continue = true;
                this.Close();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Continue = false;
            this.Close();
        }
    }
}