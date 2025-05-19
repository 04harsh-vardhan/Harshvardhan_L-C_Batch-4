namespace OnShopApi_s.Models.Dto
{
    public class UserOrders
    {
        public List<OrderDetail> orderDetails { get; set; }
        public UserOrders()
        {
            this.orderDetails = new();
        }
    }
}
