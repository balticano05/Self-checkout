namespace SelfCheckoutServiceMachine.Models;

public class ShopCart
{
    public int Id { get; private set; }
    public Dictionary<int, Product> Products { get; private set; }
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ShopCart() { }

    public class Builder
    {
        private readonly ShopCart _cart = new ShopCart();

        public Builder SetId(int id)
        {
            _cart.Id = id;
            return this;
        }

        public Builder SetProducts(Dictionary<int, Product> products)
        {
            _cart.Products = products;
            return this;
        }

        public Builder SetTotalPrice(decimal totalPrice)
        {
            _cart.TotalPrice = totalPrice;
            return this;
        }

        public Builder SetCreatedAt(DateTime createdAt)
        {
            _cart.CreatedAt = createdAt;
            return this;
        }

        public ShopCart Build()
        {
            return _cart;
        }
    }
}