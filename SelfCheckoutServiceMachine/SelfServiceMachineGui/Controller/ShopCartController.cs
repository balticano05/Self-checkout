using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Service;

namespace SelfCheckoutServiceMachine.Controller;

public class ShopCartController
{
    private ShopCartService _shopCartService;

    public ShopCartController()
    {
        _shopCartService = new ShopCartService();
    }
    
    public void putIn(Product product)
    {
        _shopCartService.putIn(product);
    }

    public void RemoveFromShopCart(int id)
    {
        _shopCartService.RemoveFromShopCart(id);
    }

    public decimal ShowPriceInShopCart()
    {
        return _shopCartService.ShowPriceInShopCart();
    }

    public List<Product> ShowAllProductsInShopCart()
    {
        return _shopCartService.ShowAllProductsInShopCart();
    }

    public ShopCart GetPurchasableProductsInShoppingCart()
    {
        return _shopCartService.GetPurchasableProductsInShoppingCart();
    }
    
}