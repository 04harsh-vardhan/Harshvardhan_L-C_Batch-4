using System.ComponentModel.DataAnnotations;

namespace OnShopApi_s.Models
{
    public class Customer
    {
        [Key]
        public string customerId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public int age { get; set; }

    }
}
