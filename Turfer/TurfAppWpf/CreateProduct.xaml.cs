using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace TurfAppWpf
{
    /// <summary>
    /// Interaction logic for CreateProduct.xaml
    /// </summary>
    public partial class CreateProduct : Window
    {
        public CreateProduct()
        {
            InitializeComponent();
            
        }

        private void btnCreateProduct(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbProductName.Text))
            {
                string messageboxConfermation = string.Format("Do you want to Create this Product: {0}?", tbProductName.Text);
                MessageBoxResult result = MessageBox.Show(messageboxConfermation, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Database.CreateProduct(tbProductName.Text);
                    string messageboxdone = string.Format("Product: {0} Created", tbProductName.Text);
                    MessageBox.Show(messageboxdone);
                    DialogResult = true;
                }
                

            }
            else
            {
                MessageBox.Show("Error: Pleas enter a name");
            }
        }
    }
}
