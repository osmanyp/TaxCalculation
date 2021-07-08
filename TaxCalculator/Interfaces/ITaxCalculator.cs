using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Models;
using TaxCalculator.Entities;

namespace TaxCalculator.Interfaces
{
    public interface ITaxCalculator
    {
        Task<TaxRatesResponse> GetTaxRatesByLocationAsync(TaxRatesRequest location);
        Task<SalesTaxOrderResponse> CalculateTaxesForOrderAsync(SalesTaxOrderRequest salesTaxOrderRequest);
    }
}
