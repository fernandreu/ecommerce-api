﻿using System.ComponentModel;

using Newtonsoft.Json;

namespace ECommerceAPI.Web.Resources
{
    /// <summary>
    /// Class that handles the showing of the navigation links on each displayed resource.
    /// Concept taken from the REST API course by Nate Barbettini.
    /// </summary>
    public class Link
    {
        public const string GetMethod = "GET";

        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }
        
        [JsonProperty(Order = -2, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }

        // Stores the route name before being rewritten by the LinkRewritingFilter
        [JsonIgnore]
        public string RouteName { get; set; }
        
        // Stores the route parameters before being rewritten by the LinkRewritingFilter
        [JsonIgnore]
        public object RouteValues { get; set; }

        public static Link To(string routeName, object routeValues = null) =>
            new Link
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = GetMethod,
                Relations = null,
            };

        public static Link ToCollection(string routeName, object routeValues = null) =>
            new Link
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = GetMethod,
                Relations = new[] { "Collection" }
            };
    }
}
