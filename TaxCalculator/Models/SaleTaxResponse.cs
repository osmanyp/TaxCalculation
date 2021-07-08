using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Models
{
    public class SaleTaxResponse
    {
        public float Rate { get; set; }
        public float OrderTotalAmount { get; set; }
        public float TaxableAmount { get; set; }
        public float AmountToCollect { get; set; }
    }
}
