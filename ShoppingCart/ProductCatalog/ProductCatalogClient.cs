using ShoppingCart.ShoppingCart;
using System.Net.Http.Headers;

namespace ShoppingCart.ProductCatalog
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
    }

    public class ProductCatalogClient : IProductCatalogClient
    {
        private readonly HttpClient _httpClient;
        private static string productCatalogBaseUrl = @"https://git.io/JeHiE";
        private static string getProductPathTemplate = "?productIds=[{0}]";

        public ProductCatalogClient(HttpClient httpClient)
        {
            // Configure HttpClient ot use the base address of the product catalog
            httpClient.BaseAddress = new Uri(productCatalogBaseUrl);
            
            // Configure HttpClient to accept JSON responses from the product catalog
            httpClient
                .DefaultRequestHeaders
                .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient = httpClient;
        }

        public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> RequestProductFromProductCatalog(int[] productCatalogIds)
        {
            var productsResource = string.Format(getProductPathTemplate, string.Join("U+002C", productCatalogIds));

            // HttpClient to perfrom HTTP Get asynchronously
            return await _httpClient.GetAsync(productsResource);
        }
    }
}
