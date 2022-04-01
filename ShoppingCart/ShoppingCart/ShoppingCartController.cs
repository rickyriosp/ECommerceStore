using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.ShoppingCart
{
    [Route("/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartStore _shoppingCartStore;

        public ShoppingCartController(IShoppingCartStore shoppingCartStore)
        {
            _shoppingCartStore = shoppingCartStore;
        }

        // GET /<ShoppingCartController>/5
        [HttpGet("{userId:int}")]
        public ShoppingCart Get(int userId)
        {
            return _shoppingCartStore.Get(userId);
        }

        // POST /<ShoppingCartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
