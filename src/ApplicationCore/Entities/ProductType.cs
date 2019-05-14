namespace ECommerceAPI.ApplicationCore.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public double Weight { get; set; }

        public double Price { get; set; }
    }
}
