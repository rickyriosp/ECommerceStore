using ShoppingCart.ShoppingCart;
using System.Net.Http.Headers;
using System.Text.Json;

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

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
        {
            using var response = await RequestProductFromProductCatalog(productCatalogIds);

            return await ConvertToShoppingCartItems(response);
        }

        private async Task<HttpResponseMessage> RequestProductFromProductCatalog(int[] productCatalogIds)
        {
            var productsResource = string.Format(getProductPathTemplate, string.Join("U+002C", productCatalogIds));

            // HttpClient to perfrom HTTP Get asynchronously
            return await _httpClient.GetAsync(productsResource);
        }

        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            // Use System.Text.Json to deserialize the JSON from the product catalog microservice
            var products = await JsonSerializer
                .DeserializeAsync<List<ProductCatalogProduct>>(
                    await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new();

            // Create ShoppingCartIem for each product in the response
            return products.Select(p => new ShoppingCartItem(
                                        p.ProductId,
                                        p.ProductName,
                                        p.ProductDescription,
                                        p.Price
                                    ));
        }

        // Private record to represent the product data
        private record ProductCatalogProduct(int ProductId, string ProductName, string ProductDescription, Money Price);
    }
}
