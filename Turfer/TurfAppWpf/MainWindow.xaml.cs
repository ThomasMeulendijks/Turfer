﻿using System;
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
using System.Globalization;
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
            UpdateListViewSale();
        }

        #region [Sales list and everything with it]
        private void UpdateListViewSale()
        { 
            // I am using an incredably bad workaround to hide the admin collum that contains the prices by making the collums so big you cant see it
            if (tcMainWindow.SelectedIndex == 0)
            {
                setCollumWidths(175);
                
            }
            else if (tcMainWindow.SelectedIndex == 1)
            {
                setCollumWidths(115);
                
            }
            if (mainEvent != null)
            {
                lvSale.ItemsSource = null;
                lvSale.Items.Clear();
                lvSale.ItemsSource = Database.GetSoldProducts(mainEvent.Id);
            }
            
        }

        private void setCollumWidths(int width)
        {
            for (int i = 0; i < gvSale.Columns.Count; i++)
            {
                gvSale.Columns[i].Width = width;
            }
        }

        private void tcMainWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                Console.WriteLine("testhelp");
                UpdateListViewSale(); 
            }
        }
        #endregion]
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
                    mainEvent = dialog.NewEvent;
                    mainEvent.CreateEvent();
                    lbCurrEvent.Content = mainEvent.Name;
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
                lbCurrEvent.Content = mainEvent.Name;
                UpdateListViewSale();
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
            lbSoda.Items.Clear();
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
                lbSoda.Items.Add(button);    // adds it to the listbox

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
