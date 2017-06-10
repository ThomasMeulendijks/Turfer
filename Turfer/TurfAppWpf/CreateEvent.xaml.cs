using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for CreateEvent.xaml
    /// </summary>
    public partial class CreateEvent : Window
    {
        // PRIVATE FIELDS
        private Pricelist newPricelist; 

        // PUBLIC PROPERTIES

        public Event NewEvent { get; set; }
    
        // PUBLIC METHODES
        public CreateEvent()
        {
            InitializeComponent();
            PricelistComoboBox.ItemsSource = Database.GetPricelists();
        }

        // PRIVATE METHODES
        private void BtnSendEventData(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EventNameTextbox.Text) && (PricelistComoboBox.SelectedIndex > -1))
            {
                newPricelist = (Pricelist)PricelistComoboBox.SelectedItem;
                NewEvent = new Event(EventNameTextbox.Text, newPricelist.Id);
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
