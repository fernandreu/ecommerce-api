namespace ECommerceAPI.ApplicationCore.Entities
{
    public class Product : BaseEntity
    {
        public int Quantity { get; set; }

        public string ProductType { get; set; }
    }
}
