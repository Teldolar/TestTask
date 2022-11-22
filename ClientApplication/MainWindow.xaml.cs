using Autofac.Core;
using ClientApplication.Managers;
using ClientApplication.Models;
using ClientApplication.UI;
using ServerApplication.DB.Models;
using ServerApplication.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Managers.ConnectionManager _connectionManager;
        private readonly MouseManager _mouseManager;
        private bool _isLoggingActive;
        private bool _sendMail;

        public MainWindow()
        {

            InitializeComponent();

            var login = new AuthentificationWindow();
            login.ShowDialog();
            if (login.DialogResult==false)
            {
                Close();
            }

            _connectionManager = new Managers.ConnectionManager();
            _mouseManager = new MouseManager(Mouse.GetPosition(this), _connectionManager);
            _isLoggingActive = false;

            startDatePicker.SelectedDate = DateTime.Now.Date;
            endDatePicker.SelectedDate = DateTime.Now.Date;

            noteCountLabel.Content = _connectionManager.GetMouseLogCount(App.UserId);

            _sendMail = (bool)sendMailCheckbox.IsChecked;

            CreateTable();
        }

        private void CreateTable()
        {
            RewriteTable(_connectionManager.GetMouseLogs(App.UserId));
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var windowPosition = Mouse.GetPosition(this);
            if (_isLoggingActive)
            {
                MouseLogs mouseLog = _mouseManager.MouseMovementProcessing(windowPosition, _sendMail);
                if (mouseLog != null)
                {
                    AddRowToDataGrid(mouseLog);
                    noteCountLabel.Content = dataGrid.Items.Count;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var windowPosition = Mouse.GetPosition(this);
            if (_isLoggingActive)
            {
                MouseLogs mouseLog = _mouseManager.MouseDownProcessng(e.ChangedButton, windowPosition, _sendMail);
                if (mouseLog != null)
                {
                    AddRowToDataGrid(mouseLog);
                    noteCountLabel.Content = (int)noteCountLabel.Content + 1;
                }
            }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoggingActive)
            {
                _isLoggingActive = false;
                button.Content = "Start";
                await _connectionManager.LogUserEvent(App.UserId, "Stop button was clicked", DateTime.Now);
            }
            else
            {
                _isLoggingActive = true;
                button.Content = "Stop";
                await _connectionManager.LogUserEvent(App.UserId, "Start button was clicked", DateTime.Now);
            }
        }

        private void AddRowToDataGrid(MouseLogs mouseLog)
        {
            if (mouseLog != null)
            {
                var row = new DataItem()
                {
                    DateTime = mouseLog.DateTime,
                    Event = mouseLog.Message,
                    PositionX = mouseLog.PositionX,
                    PositionY = mouseLog.PositionY
                };
                dataGrid.Items.Add(row);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventFilter.SelectedItem != null)
            {
                switch ((EventFilter.SelectedItem as ComboBoxItem).Content)
                {
                    case "Horizontal movement":
                        RewriteTable(_connectionManager.GetMouseLogsByFilter("horizontal"));
                        break;
                    case "Vertical movement":
                        RewriteTable(_connectionManager.GetMouseLogsByFilter("vertical"));
                        break;
                    case "Left mouse click":
                        RewriteTable(_connectionManager.GetMouseLogsByFilter("Left"));
                        break;
                    case "Right mouse click":
                        RewriteTable(_connectionManager.GetMouseLogsByFilter("Right"));
                        break;
                    case "Middle mouse click":
                        RewriteTable(_connectionManager.GetMouseLogsByFilter("Middle"));
                        break;
                    case "All":
                        RewriteTable(_connectionManager.GetMouseLogs(App.UserId));
                        break;
                }
            }
        }

        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startDatePicker.SelectedDate != null && endDatePicker.SelectedDate != null)
                RewriteTable(_connectionManager.GetMouseLogsByTime(startDatePicker.SelectedDate.Value.Date, endDatePicker.SelectedDate.Value.Date));
        }

        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startDatePicker.SelectedDate != null && endDatePicker.SelectedDate != null)
                RewriteTable(_connectionManager.GetMouseLogsByTime(startDatePicker.SelectedDate.Value.Date, endDatePicker.SelectedDate.Value.Date));
        }

        private void RewriteTable(List<MouseLogs> mouseLogs)
        {
            dataGrid.Items.Clear();
            foreach (MouseLogs mouseLog in mouseLogs)
            {
                AddRowToDataGrid(mouseLog);
            }
        }
    }
}
