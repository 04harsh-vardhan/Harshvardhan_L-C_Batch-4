using OnShopApi_s.Models;
using OnShopApi_s.Models.Dto;
using OnShopApi_s.Repositories;

namespace OnShopApi_s.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOnShopRepository _repository;
        public OrderService(IOnShopRepository repository)
        {
            _repository = repository;
        }
        public async Task PlaceOrder(OrderPlaceDto orderPlaceDto)
        {
            Order order = new Order(orderPlaceDto.customerId);
            await _repository.CreateOrder(order, orderPlaceDto.productId);
        }

        public async Task<UserOrders> GetAllOrders(string customerId)
        {
            UserOrders userOrders = new();
            List<Order> orders = await _repository.GetOrders(customerId);
            foreach (Order order in orders)
            {
                OrderDetail orderDetails = await _repository.GetOrderDetail(order.orderId);
                userOrders.orderDetails.Add(orderDetails);
            }
            return userOrders;
        }
    }
}
//{
//"customerId": "1",
//  "order": [
//    {
//        "key": "P001",
//      "value": 1
//    }
//  ]
//}