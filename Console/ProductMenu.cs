using ConsoleApp1.Models;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class ProductMenu
    {
        private readonly string _category;
        private readonly Prompts _prompt;
        private readonly HttpClientRequests _httpRequests;

        public ProductMenu(string category, Prompts prompt, HttpClientRequests httpRequests)
        {
            _category = category;
            _prompt = prompt;
            _httpRequests = httpRequests;
        }

        public async Task Display()
        {
            try
            {
                List<ProductModel> products = await GetAllProducts();
                Console.WriteLine("index | Name | price");
                for (int index = 0; index < products.Count; index++)
                {
                    Console.WriteLine($"{index}      {products[index].productName}      {products[index].price}");
                }
                Console.WriteLine("Type the index to buy the product");
                int userInput = Int32.Parse(Console.ReadLine() ?? "");
                if (userInput == -1)
                {
                    return;
                }
                else
                {
                    await PlaceOrder(products[userInput].productId);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("some exception");
            }

        }

        private async Task<List<ProductModel>> GetAllProducts()
        {
            string response = await _httpRequests.GetRequest(_prompt._ProductsURL);
            List<ProductModel> products = JsonConvert.DeserializeObject<List<ProductModel>>(response);
            List<ProductModel> specificCategoryProducts = new();
            foreach (ProductModel product in products)
            {
                if (product.category == _category)
                {
                    specificCategoryProducts.Add(product);
                }
            }
            return specificCategoryProducts;
        }
        private async Task PlaceOrder(string productId)
        {
            OrderPlaceModel orderPlaceModel = new();
            orderPlaceModel.customerId = Menu._customerId;
            orderPlaceModel.productId = productId;
            await _httpRequests.PostRequest(orderPlaceModel, _prompt._PlaceOrderURL);
            Console.WriteLine("Order has been placed successfully");
        }
    }
}
