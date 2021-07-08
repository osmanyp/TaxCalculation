using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Entities
{
    public class TaxRatesRequest
    {
        public string Country { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
