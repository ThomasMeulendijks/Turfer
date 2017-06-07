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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Runtime.InteropServices;
using TurfAppWpf;

namespace TurfAppWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Event mainEvent;

        //
        //Private methodes
        //


        // gets the button info and sends a Event.SoldProduct() comand with the data
        // this function will be behinde all buttons created with the "CreateButtons()" function
        private void BtnProductAdd(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int productID = Convert.ToInt32(button.Uid);
            int amount = Convert.ToInt32(LbAddAmount.Content);
            LbAddAmount.Content = 1;
            mainEvent.SoldProduct(productID, amount);
        }

        #region [Open window methodes]
        // opens the event creator dialog
        private void BtnOpenEventCreator(object sender, RoutedEventArgs e)
        {
            CreateEvent dialog = new CreateEvent();
            dialog.ShowDialog();

            // if the dialog has a positive result and the enterd data is valid Creat an instance of event 
            if (dialog.DialogResult.Value && dialog.DialogResult.HasValue)
            {

                // gets event names and checks if it exists
                bool NameExists = false;
                foreach (Event currEvent in Database.GetEvents())
                {
                    if (!NameExists)
                    {
                        if (currEvent.Name == dialog.NewEvent.Name)
                        {
                            NameExists = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (!NameExists)
                {
                    mainEvent = new Event(dialog.NewEvent.Name, dialog.NewEvent.Id);
                    mainEvent.CreateEvent();
                    LblCurrentEvent.Content = mainEvent.Name;
                }
                else
                {
                    MessageBox.Show("Event already exists");
                }
            }
        }

        // opens the load event dialog 
        private void BtnLoadEvent(object sender, RoutedEventArgs e)
        {
            LoadEvent dialog = new LoadEvent(this);
            dialog.ShowDialog();

            // if the dialog has a positive result  makes a new instance of event with the loaded data
            if (dialog.DialogResult.Value && dialog.DialogResult.HasValue)
            {
                mainEvent = dialog.NewEvent;
                LblCurrentEvent.Content = mainEvent.Name;
            }
        }

        // opens the Product Creator dialog
        private void BtnOpenProductCreator(object sender, RoutedEventArgs e)
        {
          
            CreateProduct dialog = new CreateProduct();
            dialog.ShowDialog();


        }
        // opens the Product Editor dialog
        private void BtnOpenProductEditor(object sender, RoutedEventArgs e)
        {
            EditProduct dialog = new EditProduct();
            dialog.ShowDialog();
        }

        #endregion

        //
        //Public Methodes
        //


        // gets all the products out of the database and creates buttons for them
        public void CreateButtons(int pricelistId)
        {
            lbFris.Items.Clear();
            List<Product> products = Database.GetProducts(pricelistId);
            foreach (Product currProduct in products)
            {
                Button button = new Button
                {

                    Width = 150,
                    Height = 90,
                    Content = currProduct.Name,
                    Uid = currProduct.Id.ToString()
                };
                Thickness margin = button.Margin;
                margin.Left = 0.5;
                margin.Right = 0.5;
                margin.Bottom = 5;
                button.Margin = margin;
                button.Click += BtnProductAdd; // add onlick event to be "BtnProductAdd"
                lbFris.Items.Add(button);    // adds it to the listbox

            }
        }
        

        //
        // other HUD buttons
        //

        private void BtnAddAmountDwn_OnClick(object sender, RoutedEventArgs e)
        {
            LbAddAmount.Content = (Convert.ToInt32(LbAddAmount.Content) - 1).ToString();
        }

        private void BtnAddAmountUP_OnClick(object sender, RoutedEventArgs e)
        {
            LbAddAmount.Content = (Convert.ToInt32(LbAddAmount.Content) + 1).ToString();
        }

        //
        //Constructors
        //
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
