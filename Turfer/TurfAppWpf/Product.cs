using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurfAppWpf
{
    public class Product
    {
        // PRIVATE FIELDS


        // PUBLIC PROPERTIES
        public int Id { get; set; }
        public int Pricelist_Id { get; set; }

        public string Name { get; set; }

        public int Volume { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal RetailPrice { get; set; }
        // CONSTRUCTORS
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Product(int id, string name, int pricelist_id , decimal retailPrice)
        {
            Id = id;
            Name = name;
            Pricelist_Id = pricelist_id;
            RetailPrice = retailPrice;
        }
        // METHODS
        public override string ToString()
        {
            return Name;
        }
        // EVENT HANDLERS


    }
}
