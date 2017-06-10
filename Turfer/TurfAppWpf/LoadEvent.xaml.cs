using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace TurfAppWpf
{
    /// <summary>
    /// Interaction logic for LoadEvent.xaml
    /// </summary>
    public partial class LoadEvent : Window
    {
        // PRIVATE FIELDS
        private MainWindow mainWindow;
        private List<Event> events;
        // PUBLIC PROPERTIES
        public Event NewEvent { get; set; }

        //public string EventName { get; set; }
        // PUBLIC METHODES
        public LoadEvent(MainWindow _mainWindow)
        {
            InitializeComponent();
            events = Database.GetEvents();
            comboBoxEvents.ItemsSource = events;
            mainWindow = _mainWindow;
        }

        // PRIVATE METHODES

        private void BtnLoadSelectedEvent(object sender, RoutedEventArgs e)
        {
            if (comboBoxEvents.SelectedIndex > -1)
            {
                NewEvent = (Event)comboBoxEvents.SelectedItem;
                mainWindow.CreateButtons(NewEvent.PricelistId);
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
            this.Close();
        }
    }
}
