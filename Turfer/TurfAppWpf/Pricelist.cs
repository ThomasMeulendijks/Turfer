using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TurfAppWpf
{
    public class Pricelist
    {

        // PRIVATE FIELDS

        // PUBLIC PROPERTIES
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> Products { get; set; }
        // CONSTRUCTORS
        public Pricelist(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Pricelist(int id, string name, List<Product> products)
        {
            Id = id;
            Name = name;
            Products = products;
        }

        // METHODS
        public override string ToString()
        {
            return Name;
        }
        // EVENT HANDLERS



    }

}
