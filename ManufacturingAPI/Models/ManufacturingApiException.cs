using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManufacturingAPI.Models
{
    /// <summary>
    /// Root class from where all custom exceptions for this project will be derived
    /// </summary>
    public class ManufacturingApiException : Exception
    {
        public ManufacturingApiException()
        {
        }

        public ManufacturingApiException(string message)
            : base(message)
        {
        }

        public ManufacturingApiException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
