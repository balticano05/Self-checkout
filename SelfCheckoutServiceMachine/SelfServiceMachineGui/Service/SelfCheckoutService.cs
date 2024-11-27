using System.Text;
using SelfCheckoutServiceMachine.Models;

namespace SelfCheckoutServiceMachine.Service;

public class SelfCheckoutService
{
    private readonly ProductService _productService;
    private readonly ShopCartService _shopCartService;

    public SelfCheckoutService()
    {
        _productService = new ProductService();
        _shopCartService = new ShopCartService();
    }

    public List<Product> SearchProducts(string name)
    {
        return _productService.SearchInCatalogByName(name);
    }

    public List<Product> SearchProductsByType(string type)
    {
        return _productService.SearchInCatalogByType(type);
    }

    public bool AddProductToCart(Product product)
    {
        if (product.QuantityInStock <= 0)
            return false;

        if (!_shopCartService.CanAddProduct(product))
            return false;

        _shopCartService.PutIn(product);
        return true;
    }

    public decimal GetTotalPrice()
    {
        return _shopCartService.ShowPriceInShopCart();
    }

    public List<Product> GetCartProducts()
    {
        return _shopCartService.ShowAllProductsInShopCart();
    }

    public void ClearCart()
    {
        _shopCartService.ClearShopCart();
    }

    public (bool IsSuccess, string Message, decimal FinalPrice) ProcessPurchase(DiscountCard discountCard = null)
    {
        var purchaseResult = _shopCartService.PrepareForPurchase();
        if (!purchaseResult.IsSuccess)
            return (false, purchaseResult.Message, 0);

        decimal finalPrice = discountCard != null 
            ? _shopCartService.CalculateFinalPrice(discountCard)
            : _shopCartService.ShowPriceInShopCart();

        return (true, "Success", finalPrice);
    }

    public bool CompletePurchase(decimal balance, decimal finalPrice)
    {
        if (balance < finalPrice)
            return false;

        _productService.UpdateProductStock(_shopCartService.ShowAllProductsInShopCart());
        _shopCartService.ClearShopCart();
        return true;
    }

    public string GenerateReceipt(decimal finalPrice, decimal balance, DiscountCard card)
    {
        StringBuilder receipt = new StringBuilder();
        receipt.AppendLine("=== RECEIPT ===");
        receipt.AppendLine($"Date: {DateTime.Now}");
        receipt.AppendLine("Products:");
        receipt.AppendLine("-------------------");

        foreach (var product in _shopCartService.ShowAllProductsInShopCart())
        {
            receipt.AppendLine($"{product.Name} - ${product.Price:F2}");
        }

        receipt.AppendLine("-------------------");
        receipt.AppendLine($"Total Price: ${_shopCartService.ShowPriceInShopCart():F2}");
        
        if (card != null)
        {
            receipt.AppendLine($"Discount Card Applied: {card.Discount}%");
            receipt.AppendLine($"Final Price: ${finalPrice:F2}");
        }

        receipt.AppendLine($"Paid Amount: ${balance:F2}");
        receipt.AppendLine($"Change: ${balance - finalPrice:F2}");
        receipt.AppendLine("=== Thank You ===");

        return receipt.ToString();
    }
    
}