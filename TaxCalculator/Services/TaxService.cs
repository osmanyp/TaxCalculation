using System;
using System.Collections.Generic;
using System.Text;
using TaxCalculator.Interfaces;
using TaxCalculator.Entities;
using TaxCalculator.Models;
using System.Threading.Tasks;
using System.Linq;

namespace TaxCalculator.Services
{
    public class TaxService
    {
        private ITaxCalculator _taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public async Task<TaxRatesResponse> GetTaxRatesByLocationAsync(TaxRatesRequest taxRatesRequest) 
        {
            if (String.IsNullOrEmpty(taxRatesRequest.Zip))
                throw new ArgumentException("The parameter zipcode is requred.");
            return await _taxCalculator.GetTaxRatesByLocationAsync(taxRatesRequest);
        }

        public async Task<SalesTaxOrderResponse> CalculateTaxesForOrderAsync(SalesTaxOrderRequest salesTaxOrderRequest)
        {
            if (String.IsNullOrEmpty(salesTaxOrderRequest.ToCountry))
                throw new ArgumentException("The ToCountry parameter  is requred.");

            if (salesTaxOrderRequest.Shipping == 0)
                throw new ArgumentException("The Shipping parameter is requred.");

            if (salesTaxOrderRequest.Amount == 0)
                throw new ArgumentException("The Amount parameter is requred.");

            if (String.IsNullOrEmpty(salesTaxOrderRequest.ToZip))
                throw new ArgumentException("The ToZip parameter is requred.");

            if (String.IsNullOrEmpty(salesTaxOrderRequest.ToState))
                throw new ArgumentException("The ToState parameter is requred.");

            if (salesTaxOrderRequest.LineItems.Count() == 0)
                throw new ArgumentException("The LineItems parameter is requred.");

            return await _taxCalculator.CalculateTaxesForOrderAsync(salesTaxOrderRequest);
        }
    }
}
