using Microsoft.AspNetCore.Mvc;
using ShoppingCart.EventFeed;
using ShoppingCart.ProductCatalog;

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

        // DELETE /<ShoppingCartController>/5
        [HttpDelete("{userId:int}/items")]
        public ShoppingCart Delete(int userId, [FromBody] int[] productIds)
        {
            var shoppingCart = _shoppingCartStore.Get(userId);
            shoppingCart.RemoveItems(productIds, _eventStore);
            _shoppingCartStore.Save(shoppingCart);

            return shoppingCart;
        }
    }
}
