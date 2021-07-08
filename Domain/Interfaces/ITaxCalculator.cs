using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Models;
using TaxCalculation.Entities;

namespace TaxCalculation.Interfaces
{
    public interface ITaxCalculator
    {
        Task<TaxRatesResponse> GetTaxRatesByLocationAsync(TaxRatesRequest location);
        Task<SalesTaxOrderResponse> CalculateTaxesForOrderAsync(SalesTaxOrderRequest salesTaxOrderRequest);
    }
}
