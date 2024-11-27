using SelfCheckoutServiceMachine.Models;
using SelfServiceMachineGui.Dto;

namespace SelfCheckoutServiceMachine.Service;

public class ShopCartService
{
    private readonly ShopCart _shopCart;

    public ShopCartService()
    {
        _shopCart = new ShopCart();
    }

    public bool CanAddProduct(Product product) =>
        _shopCart.Products.Count(p => p.Id == product.Id) < product.QuantityInStock;

    public void PutIn(Product product)
    {
        if (!CanAddProduct(product))
            throw new InvalidOperationException("Not enough items in stock!");

        _shopCart.Products.Add(product);
    }

    public decimal ShowPriceInShopCart() =>
        _shopCart.Products.Sum(p => p.Price);

    public List<Product> ShowAllProductsInShopCart() =>
        _shopCart.Products;

    public void ClearShopCart()
    {
        _shopCart.Products.Clear();
        _shopCart.TotalPrice = 0.0m;
    }

    public PurchaseResult PrepareForPurchase()
    {
        if (!_shopCart.Products.Any())
            return new PurchaseResult { IsSuccess = false, Message = "Cart is empty!" };

        var outOfStockProduct = _shopCart.Products
            .FirstOrDefault(p => p.QuantityInStock <= 0);

        return outOfStockProduct != null
            ? new PurchaseResult
            {
                IsSuccess = false,
                Message = $"Product {outOfStockProduct.Name} is out of stock!"
            }
            : new PurchaseResult { IsSuccess = true };
    }

    public decimal CalculateFinalPrice(DiscountCard discountCard) =>
        discountCard == null
            ? ShowPriceInShopCart()
            : ShowPriceInShopCart() * (1 - discountCard.Discount / 100m);
}