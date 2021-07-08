using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Entities
{
    public class Product
    {
        public int Quantity { get; set; }
        public string ProductTaxCode { get; set; }
        public float UnitPrice { get; set; }
        public float Discount { get; set; }
    }
}
