using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    /// <summary>
    /// Contains several methods to ensure the validity of a (series of) product(s).
    /// The idea is that this might come from a different location in the future (e.g. from the DynamoDB
    /// database), and hence better to have these methods decoupled from the start
    /// </summary>
    public interface IProductChecker
    {
        bool IsValidProductList(IEnumerable<Product> products, out string errorMessage);

        double CalculateRequiredWidth(IEnumerable<Product> products);
    }
}
