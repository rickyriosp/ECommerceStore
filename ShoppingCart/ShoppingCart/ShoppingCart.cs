using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart
{
    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> _items = new();

        public int UserId { get; }
        public IEnumerable<ShoppingCartItem> Items => _items;

        public ShoppingCart(int userId)
        {
            this.UserId = userId;
        }

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach (var item in shoppingCartItems)
            {
                if (_items.Add(item))
                {
                    eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });
                }
            }
        }

        public void RemoveItems(int[] productCatalogIds, IEventStore eventStore)
        {
            if (_items.RemoveWhere(i => productCatalogIds.Contains(i.ProductCatalogId)) > 0)
            {
                eventStore.Raise("ShoppingCartItemRemoved", new { UserId, productCatalogIds });
            }
        }
    }

    public record ShoppingCartItem(int ProductCatalogId, string ProductName, string Description, Money Price)
    {
        public virtual bool Equals(ShoppingCartItem? obj)
        {
            var result = obj != null && this.ProductCatalogId.Equals(obj.ProductCatalogId);
            return result;
        }

        public override int GetHashCode()
        {
            return this.ProductCatalogId.GetHashCode();
        }
    }

    public record Money(string Currency, decimal Amount);
}
