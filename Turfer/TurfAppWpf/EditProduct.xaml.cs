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
        //Private Fields
        private Pricelist LoadedPricelist;
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
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Product currproduct in ChangedProducts)
                {
                    Database.EditProduct(currproduct.Id, ((Pricelist)cbbPricelist.SelectedItem).Id, currproduct.Name, currproduct.Volume, currproduct.RetailPrice);
                }

                ChangedProducts.Clear();
                string mbString = string.Format("Succesfully Saved {0} products to your pricelist" , ChangedProducts.Count );
                MessageBox.Show( mbString , "Succes");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "error");
            }

            
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
                LoadedPricelist = (Pricelist)cbbPricelist.SelectedItem;
                LoadPricelist(LoadedPricelist);
                cbbProducts.ItemsSource = Database.GetProductsNotInPricelist(LoadedPricelist.Id);
            }
        }

        private void btnAddProducttToPricelist_Click(object sender, RoutedEventArgs e)
        {
            if (cbbProducts.SelectedIndex > -1)
            {
                Product currProduct = (Product) cbbProducts.SelectedItem;
                if (!ChangedProducts.Contains(currProduct)) //Should allways be true unless you try to add the same item twice
                {
                    //adds it to the list of changed products since its not in the pricelist allredy and needs an update
                    ChangedProducts.Add(currProduct);

                    //add it to the pricelist in memory and loads it in the visual datagrid to be editable
                    LoadedPricelist.Products.Add(currProduct);  
                    dgProducts.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Are you trying to break me?!, you CAN'T have the same product twice PLEAS you will kill everything!");
                }
            }
        }

        private void dgProducts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Product currProduct = (Product) e.Row.Item;
            if (!ChangedProducts.Contains(currProduct))
            {
                ChangedProducts.Add(currProduct);
            }

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
