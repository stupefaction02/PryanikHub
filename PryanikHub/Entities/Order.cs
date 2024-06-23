using System.Text.Json.Serialization;

namespace PryanikHub.Entities
{
    /// <summary>
    /// DTO for an order
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("orderLines")]
        public IEnumerable<OrderLine> OrderLines { get; set; }

        [JsonPropertyName("shippingDate")]
        //[JsonConverter(JsonDate)]
        public DateTime ShippingDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
