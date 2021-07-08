using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TaxProviders.Exceptions
{
    public class BadResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }

        public BadResponseException(string message, HttpStatusCode statusCode, string response) : base(message)
        {
            StatusCode = statusCode;
            Response = response;
        }
    }
}
