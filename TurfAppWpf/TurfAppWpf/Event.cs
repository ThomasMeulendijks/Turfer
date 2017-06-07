using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace TurfAppWpf
{
    public class Event
    {
        // PRIVATE FIELDS

        // PUBLIC PROPERTIES
        public int Id { get; set; }

        public string Name { get; set; }
        public int PricelistId { get; set; }


        // PUBLIC METHODES
        public void SoldProduct(int productID, int amount)
        {
            Database.SoldProduct(productID, amount, Id);
        }

        public override string ToString()
        {
            return Name;
        }
        public void CreateEvent()
        {
            Id = Database.CreateEventReturnEventID(Name, PricelistId);
        }
        // PRIVATE METHODES
        // CONSTRUCTOR
        public Event(int id, string eventName, int pricelistId)
        {
            Id = id;
            Name = eventName;
            PricelistId = pricelistId;
           
        }
        public Event(string eventName, int pricelistId)
        {
            Name = eventName;
            PricelistId = pricelistId;

        }

    }
}