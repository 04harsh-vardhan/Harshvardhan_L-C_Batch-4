using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnShopApi_s.Models
{
    public class OrderDetail
    {
        [Key]
        public string orderDetailId { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }
        public string orderId { get; set; }
        [ForeignKey(nameof(orderId))]
        public Order Order { get; set; }
        [ForeignKey(nameof(productId))]
        public Product Product { get; set; }

        public OrderDetail(string orderId, string productId)
        {
            this.orderId = orderId;
            this.productId = productId;
            this.orderDetailId = Guid.NewGuid().ToString();
        }
    }
}
