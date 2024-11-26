using SelfCheckoutServiceMachine.Models;
using SelfServiceMachineGui.Dto;

namespace SelfCheckoutServiceMachine.Service;

public class ShopCartService
{

    private ShopCart _shopCart;

    public ShopCartService()
    {
        _shopCart = new ShopCart();
    }

    public bool CanAddProduct(Product product)
    {
        int currentQuantityInCart = _shopCart.Products.Count(p => p.Id == product.Id);
        return currentQuantityInCart < product.QuantityInStock;
    }

    public void PutIn(Product product)
    {
        if (CanAddProduct(product))
        {
            _shopCart.Products.Add(product);
        }
        else
        {
            throw new InvalidOperationException("Not enough items in stock!");
        }
    }

    public decimal ShowPriceInShopCart()
    {
        return _shopCart.Products.Sum(p => p.Price);
    }

    public List<Product> ShowAllProductsInShopCart()
    {
        return _shopCart.Products;
    }

    public void ClearShopCart()
    {
        _shopCart.Products.Clear();
        _shopCart.TotalPrice = 0.0m;
    }

    public PurchaseResult PrepareForPurchase()
    {
        if (!_shopCart.Products.Any())
        {
            return new PurchaseResult { IsSuccess = false, Message = "Cart is empty!" };
        }

        foreach (var product in _shopCart.Products)
        {
            if (product.QuantityInStock <= 0)
            {
                return new PurchaseResult 
                { 
                    IsSuccess = false, 
                    Message = $"Product {product.Name} is out of stock!" 
                };
            }
        }

        return new PurchaseResult { IsSuccess = true };
    }

    public decimal CalculateFinalPrice(DiscountCard discountCard)
    {
        decimal totalPrice = ShowPriceInShopCart();
        if (discountCard != null)
        {
            return totalPrice - (totalPrice * discountCard.Discount / 100);
        }
        return totalPrice;
    }

}