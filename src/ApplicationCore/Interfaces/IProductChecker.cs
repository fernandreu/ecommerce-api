﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Logistics;

namespace ECommerceAPI.ApplicationCore.Interfaces
{
    /// <summary>
    /// Contains several methods to ensure the validity of a list of products.
    /// The idea is that this might come from a different location in the future (e.g. from the DynamoDB
    /// database), and hence better to have these methods decoupled from the start
    /// </summary>
    public interface IProductChecker
    {
        Task<(bool valid, string error)> IsValidProductListAsync(IEnumerable<Product> products);

        Task<OrderDetails> CalculateOrderDetailsAsync(IEnumerable<Product> products);
    }
}
