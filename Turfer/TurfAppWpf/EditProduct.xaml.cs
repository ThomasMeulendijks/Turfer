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
        //PUB PROPERTYS
        public List<Product> ChangedProducts { get; set; }
        // CONSTRUCTOR
        public EditProduct()
        {
            InitializeComponent();
            List<Pricelist> Pricelists = Database.GetPricelists();
            ChangedProducts = new List<Product>();
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
            foreach (Product currproduct in ChangedProducts)
            {
                Database.EditProduct(currproduct.Id, ((Pricelist)cbbPricelist.SelectedItem).Id, currproduct.Name, currproduct.Volume, currproduct.RetailPrice);
            }
            Close();
        }

        private void LoadPricelist(Pricelist currPricelist)
        {
            dgProducts.Items.Clear();
            dgProducts.ItemsSource = currPricelist.Products;
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

        private void dgProducts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ChangedProducts.Add((Product)e.Row.Item);
            //switch (e.Column.SortMemberPath)
            //{
            //    case "RetailPrice" :
            //        MessageBox.Show(((Product)e.Row.Item).RetailPrice.ToString());
            //        break;
            //    case "Name":
            //        MessageBox.Show(((Product)e.Row.Item).Name);
            //        break;
            //    case "Volume":
            //        MessageBox.Show(((Product)e.Row.Item).Volume.ToString());
            //        break;
            //}
        }

        private void cbbPricelist_DropDownOpened(object sender, EventArgs e)
        {
            dgProducts.ItemsSource = null;
            dgProducts.Items.Clear();
        }
    }
}
