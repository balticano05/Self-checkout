namespace SelfCheckoutServiceMachine.Models;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public decimal WholesalePrice { get; private set; }
    public TypeProduct Type { get; private set; }
    public bool WholesaleProduct { get; private set; }
    public int QuantityInStock { get; private set; }

    private Product() { }

    public class Builder
    {
        private readonly Product _product = new Product();

        public Builder SetId(int id)
        {
            _product.Id = id;
            return this;
        }

        public Builder SetName(string name)
        {
            _product.Name = name;
            return this;
        }

        public Builder SetPrice(decimal price)
        {
            _product.Price = price;
            return this;
        }

        public Builder SetDescription(string description)
        {
            _product.Description = description;
            return this;
        }

        public Builder SetWholesalePrice(decimal wholesalePrice)
        {
            _product.WholesalePrice = wholesalePrice;
            return this;
        }

        public Builder SetTypeProduct(TypeProduct type)
        {
            _product.Type = type;
            return this;
        }

        public Builder SetWholesaleProduct(bool wholesaleProduct)
        {
            _product.WholesaleProduct = wholesaleProduct;
            return this;
        }

        public Builder SetQuantityInStock(int quantityInStock)
        {
            _product.QuantityInStock = quantityInStock;
            return this;
        }

        public Product Build()
        {
            return _product;
        }
    }
}