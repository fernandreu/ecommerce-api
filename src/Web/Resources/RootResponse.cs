namespace ECommerceAPI.Web.Resources
{
    public class RootResponse : BaseResource
    {
        public Link Customers { get; set; }

        public Link ProductTypes { get; set; }
    }
}
