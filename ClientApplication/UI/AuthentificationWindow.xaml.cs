using ClientApplication.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApplication.UI
{
    /// <summary>
    /// Interaction logic for AuthentificationWindow.xaml
    /// </summary>
    public partial class AuthentificationWindow : Window
    {
        private readonly ConnectionManager _connectionManager;

        public AuthentificationWindow()
        {
            _connectionManager = new ConnectionManager();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int userId = _connectionManager.LogIn(loginTextBox.Text, passwordTextBox.Text);

            if (userId != -1)
            {
                App.UserId = userId;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Login or password is incorrect");
            }
        }
    }
}
