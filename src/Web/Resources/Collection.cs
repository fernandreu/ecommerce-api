﻿namespace ECommerceAPI.Web.Resources
{
    public class Collection<T> : BaseResource
    {
        public T[] Value { get; set; }
    }
}
