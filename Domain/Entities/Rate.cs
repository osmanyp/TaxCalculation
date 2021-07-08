using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculation.Entities
{
    public class Rate
    {
        public float CountryRate { get; set; }
        public float StateRate { get; set; }
        public float CountyRate { get; set; }
        public float CityRate { get; set; }
        public float CombinedDistrictRate { get; set; }
        public float CombinedRate { get; set; }
        public bool FreightTaxable { get; set; }
    }
}
