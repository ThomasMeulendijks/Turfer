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

namespace TurfAppWpf
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        List<Pricelist> Pricelists;
        public EditProduct()
        {
            InitializeComponent();
            Pricelists = Database.GetPricelists();
            cbbPricelist.ItemsSource = Pricelists;
            dgProducts.IsReadOnly = false;
            dgProducts.CanUserAddRows = false;
            dgProducts.CanUserDeleteRows = false;

        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadPricelist(Pricelist currPricelist)
        {
            dgProducts.Items.Clear();
            foreach (Product currProduct in currPricelist.Products)
            {
                
                dgProducts.Items.Add(currProduct);
            }
        }

        private void cbbPricelist_DropDownClosed(object sender, EventArgs e)
        {
            if (cbbPricelist.SelectedIndex > -1)
            {
                Pricelist currPricelist = (Pricelist)cbbPricelist.SelectedItem;
                LoadPricelist(currPricelist);
            }
        }

        private void btnAddProducttToPricelist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgProducts_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            MessageBox.Show("edit");
        }
    }
}
