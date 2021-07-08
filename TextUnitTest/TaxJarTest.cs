using System;
using Xunit;
using TaxCalculation.Services;
using TaxProviders.Providers;
using System.Threading.Tasks;
using TaxCalculation.Models;
using System.Collections.Generic;
using TaxCalculation.Entities;
using TaxProviders.Exceptions;
using Microsoft.Extensions.Configuration;

namespace TextUnitTest
{
    public class TaxJarTest
    {
        private static IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        private readonly TaxJarProvider m_taxJarProvider = new TaxJarProvider(configuration);

        [Fact]
        public async Task Valid_Get_Rates_For_Full_Location()
        {
            //arrage
            var taxRatesRequest = new TaxRatesRequest
            {
                Zip = "33414",
                Country = "US",
                City = "Wellington",
                State = "Fl",
                Street = "11950 Forest Hill Blvd"
            };

            //act
            TaxService taxService = new TaxService(m_taxJarProvider);
            TaxRatesResponse response = await taxService.GetTaxRatesByLocationAsync(taxRatesRequest);

            //assert
            Assert.NotNull(response.Rate);
            Assert.Equal(0.07f, response.Rate.CombinedRate);
            Assert.Equal(0.01f, response.Rate.CountyRate);
            Assert.Equal(0.06f, response.Rate.StateRate);
        }

        [Fact]
        public void Not_Valid_GetRates_For_Bad_ZipCode()
        {
            TaxService taxService = new TaxService(m_taxJarProvider);
            //arrage
            var taxRatesRequest = new TaxRatesRequest
            {
                Zip = "1"
            };

            //act
            _ = Assert.ThrowsAsync<TaxJarException>(() => taxService.GetTaxRatesByLocationAsync(taxRatesRequest));
        }

        [Fact]
        public void Not_Valid_GetRates_For_Missing_ZipCode()
        {
            TaxService taxService = new TaxService(m_taxJarProvider);
            //arrage
            var taxRatesRequest = new TaxRatesRequest
            {
                Country = "US"
            };

            //act
            _ = Assert.ThrowsAsync<ArgumentException>(() => taxService.GetTaxRatesByLocationAsync(taxRatesRequest));
        }

        [Fact]
        public async Task Valid_Calculate_Taxes_For_Order_To_Collect()
        {
            TaxService taxService = new TaxService(m_taxJarProvider);

            //arrage
            var salesTaxOrderRequest = new SalesTaxOrderRequest()
            {
                FromZip = "07001",
                FromState = "NJ",
                FromCountry = "US",
                ToZip = "07446",
                ToState = "NJ",
                ToCountry = "US",
                Amount = 16.5f,
                Shipping = 1.5f,
                LineItems = new List<Product>() { 
                    new Product() {
                        Quantity = 1,
                        UnitPrice = 15.0f,
                        ProductTaxCode = "31000"
                    }
                }
            };

            //act
            SalesTaxOrderResponse salesTaxOrderResponse = await taxService.CalculateTaxesForOrderAsync(salesTaxOrderRequest);

            //assert
            Assert.Equal(1.09f, salesTaxOrderResponse.Tax.AmountToCollect);
            Assert.Equal(16.5f, salesTaxOrderResponse.Tax.OrderTotalAmount);
            Assert.Equal(0.06625f, salesTaxOrderResponse.Tax.Rate);
        }


        [Fact]
        public void Not_Valid_Calculate_Taxes_For_Order_Missing_ToCountry()
        {
            TaxService taxService = new TaxService(m_taxJarProvider);
            
            //arrage
            var salesTaxOrderRequest = new SalesTaxOrderRequest()
            {
                FromZip = "07001",
                FromState = "NJ",
                FromCountry = "US",
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.5f,
                Shipping = 1.5f,
                LineItems = new List<Product>() {
                    new Product() {
                        Quantity = 1,
                        UnitPrice = 15.0f,
                        ProductTaxCode = "31000"
                    }
                }
            };

            //assert 
            _ = Assert.ThrowsAsync<ArgumentException>(() => taxService.CalculateTaxesForOrderAsync(salesTaxOrderRequest));
        }
    }
}