using Newtonsoft.Json;
using System;
using System.Net;

namespace PrimeNumber.Extensibility.Dto
{
    /// <summary>
    /// Exception transfer object
    /// </summary>
    public class ExceptionDto
    {
        /// <summary>
        /// Exception message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Status code
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// Exception detail object
        /// </summary>
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
