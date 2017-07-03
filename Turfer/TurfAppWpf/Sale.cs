using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurfAppWpf
{  
    public class Sale
    {
        //PUBLIC PROPERTYS
        public int EventId { get; set; }
        public int AmountSold { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }

        //CONSTRUCTORS

        public Sale(int eventId, int amountSold, string productName, decimal productPrice)
        {
            EventId = eventId;
            AmountSold = amountSold;
            ProductName = productName;
            ProductPrice = productPrice;
            TotalPrice = AmountSold * ProductPrice;
        }
    }
}
