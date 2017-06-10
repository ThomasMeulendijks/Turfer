using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace TurfAppWpf
{
    static class Database
    {
        //Connect
        public static void Connect(SqlConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBoxResult result = MessageBox.Show("Error: " + e.Message  +"\n" + "Click OK to reconnect" , "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Connect(conn);
                }
                else
                {
                    Environment.Exit(-1);
                }
            }
        }

        //
        // Read database
        //
        public static List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);

                using (SqlCommand command = new SqlCommand("select * from Event", conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new Event(
                            Convert.ToInt16(reader["ID"].ToString()),
                            reader["Name"].ToString(),
                            Convert.ToInt32(reader["Pricelist_ID"].ToString())));
                    }
                    return events;
                }
            }
        }
        public static List<Product> GetProducts()
        {

            List<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);

                using (SqlCommand command = new SqlCommand("SELECT Product_Pricelist.ID AS PP_ID , Product_Pricelist.Product_ID, Product.Name as Product_Name , Product.Volume_mL , Product.PurchasePrice, Product_Pricelist.Pricelist_ID, Pricelist.Name AS Pricelist_Name, Product_Pricelist.Price " +
                    "FROM Product_Pricelist " +
                    "INNER JOIN Product ON Product_Pricelist.Product_ID = Product.ID " +
                    "INNER JOIN Pricelist ON Product_Pricelist.Pricelist_ID = Pricelist.ID; ", conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product(
                            Convert.ToInt16(reader["Product_ID"].ToString()),
                            reader["Product_Name"].ToString(),
                            Convert.ToInt16(reader["Pricelist_ID"].ToString()),
                            Convert.ToDecimal(reader["Price"].ToString())
                            ));
                    }
                    return products;
                }
            }
        }

        // Gets all products that are defined in the pricelist with the given ID
        public static List<Product> GetProducts(int PricelistId)
        {

            List<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);
                string commandString = string.Format("SELECT Product_Pricelist.ID AS PP_ID, Product_Pricelist.Product_ID, Product.Name as Product_Name, Product.Volume_mL, Product.PurchasePrice, Product_Pricelist.Pricelist_ID, Pricelist.Name AS Pricelist_Name, Product_Pricelist.Price " +
                    "FROM Product_Pricelist " +
                    "INNER JOIN Product ON Product_Pricelist.Product_ID = Product.ID " +
                    "INNER JOIN Pricelist ON Product_Pricelist.Pricelist_ID = Pricelist.ID " +
                    "WHERE Pricelist_ID = {0}", PricelistId );
                using (SqlCommand command = new SqlCommand(commandString, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product(
                            Convert.ToInt16(reader["Product_ID"].ToString()),
                            reader["Product_Name"].ToString(),
                            Convert.ToInt16(reader["Pricelist_ID"].ToString()),
                            Convert.ToDecimal(reader["Price"].ToString())
                            ));
                    }
                    return products;
                }
            }
        }

        // gets all the products that are not in the pricelist
        public static List<Product> GetProductsNotInPricelist(int PricelistId)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);
                string commandString = string.Format("SELECT ID as Product_ID, Name as Product_Name, Volume_mL, PurchasePrice  " +
                    "FROM Product where Product.ID NOT IN (Select Product_ID From Product_Pricelist WHERE Pricelist_ID = {0})", PricelistId);
                using (SqlCommand command = new SqlCommand(commandString, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product(
                            Convert.ToInt16(reader["Product_ID"].ToString()),
                            reader["Product_Name"].ToString()
                            ));
                    }
                    return products;
                }
            }
        }
        public static List<Pricelist> GetPricelists()
        {
            List<Pricelist> pricelists = new List<Pricelist>();
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);

                using (SqlCommand command = new SqlCommand("select * from Pricelist", conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int pricelist_Id;
                        pricelists.Add(new Pricelist(
                            pricelist_Id = Convert.ToInt16(reader["ID"].ToString()),
                            reader["Name"].ToString(),
                       
                            // Opens the function get products with this pricelist id
                            GetProducts(pricelist_Id)
                            ));
                    }
                }
            }
            return pricelists;
        }

        //
        // Write database
        //
        public static int CreateEventReturnEventID(string eventName, int pricelistID) 
        {
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);
                string commandString = string.Format("INSERT INTO [Event] (Pricelist_ID, Name) VALUES ( {0} , '{1}' )" +
                                                     " select IDENT_CURRENT('Event') as lastid;", pricelistID, eventName);
                using (SqlCommand command = new SqlCommand(commandString, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    int EventID = Convert.ToInt32(reader["lastid"].ToString());
                    return EventID;
                }

            }
        }
        public static void CreateProduct(string productName)
        {
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);
                string commandString = string.Format("INSERT INTO [Product] (Name) VALUES ('{0}');" , productName);
                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    command.ExecuteNonQuery();
                }

            }
        }
        public static void EditProduct(int productId ,int pricelistId,string productName,int volume, decimal retailPrice)
        {
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);
                string commandString = string.Format("IF EXISTS( " +
                                                    "Select Product.ID, Product.Name, Product.Volume_mL, Product_Pricelist.Pricelist_ID, Product_Pricelist.Price From Product_Pricelist " +
                                                    "INNER JOIN Product ON Product_Pricelist.Product_ID = Product.ID " +
                                                    "Where Product_Pricelist.Product_ID =  {0} " +
                                                    "AND Product_Pricelist.Pricelist_ID = {1}) " +
                                                    "BEGIN " +
                                                    "Update Product_Pricelist SET Price = {4} " +
                                                    "WHERE Product_ID = {0} AND Pricelist_ID = {1} " +
                                                    "END " +
                                                    "ELSE " +
                                                    "BEGIN " +
                                                    "INSERT INTO Product_Pricelist(Product_ID, Pricelist_ID, Price) VALUES({0} , {1}, {4}) " +
                                                    "END " +
                                                    "UPDATE Product SET[Name] = '{2}' WHERE ID = {0} " +
                                                    "UPDATE Product SET Volume_mL = {3} WHERE ID = {0}; ", productId, pricelistId, productName, volume, retailPrice);

                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    command.ExecuteNonQuery();
                }

            }
        }

        //checks if a product is allredy sold in this event if so increases the "Count"  if not inserts a new row with the product and sets the count.
        public static void SoldProduct(int productID, int amount, int eventID)
        {
            using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
            {
                Connect(conn);
                string commandString = string.Format("DECLARE @PP_ID INT SELECT @PP_ID = ID from Product_Pricelist " +
                                                     "where Product_ID = {1} and Pricelist_ID = (select Pricelist_ID from Event where ID = {0}) " +
                                                     "IF EXISTS(Select* from [Sale] where [Event_ID] = {0} and [Product_Pricelist_ID] = @PP_ID) " +
                                                     "UPDATE Sale SET Count = Count + {2} where[Event_ID] = {0} and [Product_Pricelist_ID] = @PP_ID " +
                                                     "ELSE insert into sale(Event_ID , Product_Pricelist_ID , Count) VALUES({0}, @PP_ID, {2}); "
                                                     , eventID, productID, amount);

                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //
        // Old here for memorys
        //

        #region [Old code no longer used]

        //public static int GetProductID(string productName)
        //{
        //    using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
        //    {
        //        int id;
        //        conn.Open();
        //        string commandString = string.Format("select ID from Product where Name = '{0}' ", productName);
        //        using (SqlCommand command = new SqlCommand(commandString, conn))
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            reader.Read();
        //            id = Convert.ToInt32(reader["ID"].ToString());
        //        }
        //        return id;
        //    }
        //}
        //public static int GetEventIDFromName(string Name)
        //{
        //    using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
        //    {
        //        conn.Open();
        //        string commandString = string.Format("select ID from Event where Name = '{0}' ", Name);
        //        using (SqlCommand command = new SqlCommand(commandString, conn))
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            reader.Read();
        //            int EventID = Convert.ToInt32(reader["ID"].ToString());
        //            return EventID;
        //        }
        //    }
        //}
        //public static int GetPricelistIDFromPricelistName(string PricelistName)
        //{
        //    using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
        //    {
        //        conn.Open();
        //        string commandString = string.Format("select ID from Pricelist where Name = '{0}' ", PricelistName);
        //        using (SqlCommand command = new SqlCommand(commandString, conn))
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            reader.Read();
        //            int PricelistID = Convert.ToInt32(reader["ID"].ToString());
        //            return PricelistID;
        //        }
        //    }
        //}
        //public static int GetPricelistIDFromEventName(string eventName)
        //{
        //    using (SqlConnection conn = new SqlConnection(Helper.CnnVal()))
        //    {
        //        conn.Open();
        //        string commandString = string.Format("select Pricelist_ID from Event where Name = '{0}' ", eventName);
        //        using (SqlCommand command = new SqlCommand(commandString, conn))
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            reader.Read();
        //            int PricelistID = Convert.ToInt32(reader["Pricelist_ID"].ToString());
        //            return PricelistID;
        //        }
        //    }
        //}
#endregion
    }
}
