using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.ShoppingCart
{
    [Route("/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartStore _shoppingCartStore;
        private readonly IProductCatalogClient _productCatalogClient;
        private readonly IEventStore _eventStore;

        public ShoppingCartController(IShoppingCartStore shoppingCartStore, 
                                      IProductCatalogClient productCatalogClient,
                                      IEventStore eventStore)
        {
            _shoppingCartStore = shoppingCartStore;
            _productCatalogClient = productCatalogClient;
            _eventStore = eventStore;
        }

        // GET /<ShoppingCartController>/5
        [HttpGet("{userId:int}")]
        public ShoppingCart Get(int userId)
        {
            return _shoppingCartStore.Get(userId);
        }

        // POST /<ShoppingCartController>/5/items
        [HttpPost("/{userId:int}/items")]
        public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
        {
            var shoppingCart = _shoppingCartStore.Get(userId);

            // Call to Product Catalog microservice
            var shoppingCartItems = await _productCatalogClient.GetShoppingCartItems(productIds);

            shoppingCart.AddItems(shoppingCartItems);
            _shoppingCartStore.Save(shoppingCart);

            return shoppingCart;
        }

        // PUT /<ShoppingCartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE /<ShoppingCartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
