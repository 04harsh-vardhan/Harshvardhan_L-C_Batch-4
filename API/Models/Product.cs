using System.ComponentModel.DataAnnotations;

namespace OnShopApi_s.Models
{
    public class Product
    {
        [Key]
        public string productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int stockQuantity { get; set; }
        public string category { get; set; }
    }
}
