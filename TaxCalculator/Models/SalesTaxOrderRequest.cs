using System;
using System.Collections.Generic;
using System.Text;
using TaxCalculator.Entities;

namespace TaxCalculator.Models
{
    public class SalesTaxOrderRequest
    {
        public string FromCountry { get; set; }
        public string FromZip { get; set; }
        public string FromState { get; set; }
        public string ToCountry { get; set; }
        public string ToZip { get; set; }
        public string ToState { get; set; }
        public float Amount { get; set; }
        public float Shipping { get; set; }
        public IEnumerable<Product> LineItems { get; set; }
    }
}
