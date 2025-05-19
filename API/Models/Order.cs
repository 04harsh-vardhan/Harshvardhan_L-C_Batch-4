using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnShopApi_s.Models
{
    public class Order
    {
        [Key]
        public string orderId { get; set; }
        public string customerId { get; set; }

        public byte isPurchased { get; set; }

        [ForeignKey(nameof(customerId))]
        public Customer customer { get; set; }

        public Order(string customerId)
        {
            this.orderId = Guid.NewGuid().ToString();
            this.customerId = customerId;
        }

    }
}
