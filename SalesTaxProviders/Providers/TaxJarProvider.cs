using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaxCalculator.Interfaces;
using TaxCalculator.Models;
using TaxProviders.Helpers;
using TaxCalculator.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaxProviders.Exceptions;
using TaxProviders.Model;
using Microsoft.Extensions.Configuration;

namespace TaxProviders.Providers
{
    public class TaxJarProvider : ITaxCalculator
    {
        private readonly IConfigurationSection _configurationSection;

        public TaxJarProvider(IConfiguration configurationRoot)
        {
            _configurationSection = configurationRoot.GetSection("TaxProviders").GetSection("TaxJar");
        }

        private string ApiKey { 
            get  { 
                return _configurationSection.GetSection("Key").Value;
            } 
        }

        private string BaseURL
        {
            get
            {
                return _configurationSection.GetSection("BaseURL").Value;
            }
        }

        private string TaxesEndPoint
        {
            get
            {
                return _configurationSection.GetSection("EndPoints").GetSection("taxes").Value;
            }
        }

        private string RatesEndPoint
        {
            get
            {
                return _configurationSection.GetSection("EndPoints").GetSection("rates").Value;
            }
        }

        public async Task<SalesTaxOrderResponse> CalculateTaxesForOrderAsync(SalesTaxOrderRequest salesTaxOrderRequest)
        {
            string salesTaxOrderRequestSerialized = JsonConvert.SerializeObject(salesTaxOrderRequest, JsonSerializerSettings());
            var objectAsJson = new StringContent(salesTaxOrderRequestSerialized, Encoding.UTF8, "application/json");

            try
            {
                var client = new HttpClientHelper<SalesTaxOrderResponse>(BaseURL);
                return await client.PostAsync(TaxesEndPoint, objectAsJson, ApiKey);
            }
            catch (BadResponseException ex)
            {
                var taxJarError = JsonConvert.DeserializeObject<TaxJarError>(ex.Response, JsonSerializerSettings());
                throw new TaxJarException(taxJarError.Detail, taxJarError.Status, taxJarError.Error, ex);
            }
        }

        public async Task<TaxRatesResponse> GetTaxRatesByLocationAsync(TaxRatesRequest taxRatesRequest)
        {
            string endPoint = $"{RatesEndPoint}/{taxRatesRequest.Zip}";
            var queryParams = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(taxRatesRequest.Street))
            {
                queryParams.Add("street", taxRatesRequest.Street);
            }

            if (!String.IsNullOrEmpty(taxRatesRequest.City))
            {
                queryParams.Add("city", taxRatesRequest.City);
            }

            if (!String.IsNullOrEmpty(taxRatesRequest.State))
            {
                queryParams.Add("state", taxRatesRequest.State);
            }

            if (!String.IsNullOrEmpty(taxRatesRequest.Country))
            {
                queryParams.Add("country", taxRatesRequest.Country);
            }

            try
            {
                var client = new HttpClientHelper<TaxRatesResponse>(BaseURL);
                return await client.GetAsync(endPoint, queryParams, ApiKey);
            }
            catch (BadResponseException ex)
            {
                var taxJarError = JsonConvert.DeserializeObject<TaxJarError>(ex.Response, JsonSerializerSettings());
                throw new TaxJarException(taxJarError.Detail, taxJarError.Status, taxJarError.Error, ex);
            }
        }

        private static JsonSerializerSettings JsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }
    }
}
