using OnShopApi_s.Models.Dto;

namespace OnShopApi_s.Services
{
    public interface IOrderService
    {
        public Task PlaceOrder(OrderPlaceDto orderPlaceDto);
        public Task<UserOrders> GetAllOrders(string customerId);
    }
}
