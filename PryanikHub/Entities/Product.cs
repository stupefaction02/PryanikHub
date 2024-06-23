namespace PryanikHub.Entities
{
    /// <summary>
    /// DTO for a product 
    /// </summary>
    /// 
    //[Owned]
    public class Product
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PreviewImageRef { get; set; }

        public string[] DetailedImagesRefs { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
