using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TaxProviders.Exceptions
{
    public class TaxJarException : Exception
    {
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public TaxJarException(string message, HttpStatusCode statusCode, string error)
            :  this(message, statusCode, error, null)
        {
        }

        public TaxJarException(string message, HttpStatusCode statusCode, string error, Exception innerException) 
            : base(message, innerException)
        {
            Error = error;
            StatusCode = statusCode;
        }
    }
}
