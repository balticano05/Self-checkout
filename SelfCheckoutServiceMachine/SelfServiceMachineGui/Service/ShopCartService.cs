using SelfCheckoutServiceMachine.Models;

namespace SelfCheckoutServiceMachine.Service;

public class ShopCartService
{

    private ShopCart _shopCart;

    public ShopCartService()
    {
        _shopCart = new ShopCart();
    }
    
    public void putIn(Product product)
    {
        _shopCart.Products.Add(product);
    }

    public void RemoveFromShopCart(int id)
    {
        for (int i = 0; i < _shopCart.Products.Count; i++)
        {
            if (_shopCart.Products[i].Id == id)
            {
                _shopCart.Products.RemoveAt(id);
                break;
            }
        }
    }

    public decimal ShowPriceInShopCart()
    {
        for (int i = 0; i < _shopCart.Products.Count; i++)
        {
            _shopCart.TotalPrice += _shopCart.Products[i].Price;
        }

        return _shopCart.TotalPrice;
    }

    public List<Product> ShowAllProductsInShopCart()
    {
        return _shopCart.Products;
    }

    public ShopCart GetPurchasableProductsInShoppingCart()
    {
        _shopCart.CreatedAt = DateTime.Now;
        ShopCart purchasableShopCart = _shopCart;
        _shopCart = new ShopCart();
        return purchasableShopCart;
    }

}