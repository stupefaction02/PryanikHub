using System.Text.Json.Serialization;

namespace PryanikHub.Entities
{
    public class OrderLine
    {
        public int OrderLineId { get; set; }

        public int OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }

        public Product Product { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
