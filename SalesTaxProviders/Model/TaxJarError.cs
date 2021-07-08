using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TaxProviders.Model
{
    public class TaxJarError
    {
        public HttpStatusCode Status { get; set; }
        public string Error { get; set; }
        public string Detail { get; set; }
    }
}
