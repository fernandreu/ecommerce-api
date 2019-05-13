using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceAPI.ApplicationCore.Entities;

namespace ECommerceAPI.ApplicationCore.Interfaces
{
    /// <summary>
    /// Contains several methods to ensure the validity of a list of products.
    /// The idea is that this might come from a different location in the future (e.g. from the DynamoDB
    /// database), and hence better to have these methods decoupled from the start
    /// </summary>
    public interface IProductChecker
    {
        Task<Tuple<bool, string>> IsValidProductListAsync(IEnumerable<Product> products);

        Task<double> CalculateRequiredWidthAsync(IEnumerable<Product> products);
    }
}
