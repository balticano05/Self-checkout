using System.IO;
using System.Text;
using SelfCheckoutServiceMachine.Models;

namespace SelfCheckoutServiceMachine.Service;

public class SelfCheckoutService
{
    private readonly ProductService _productService;
    private readonly ShopCartService _shopCartService;
    private readonly PdfReceiptService _pdfReceiptService;

    public SelfCheckoutService()
    {
        _productService = new ProductService();
        _shopCartService = new ShopCartService();
        _pdfReceiptService = new PdfReceiptService();
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

        return true;
    }

    public string GenerateReceipt(decimal finalPrice, decimal balance, DiscountCard card)
    {
        StringBuilder receipt = new StringBuilder();
        receipt.AppendLine("======== RECEIPT ========");
        receipt.AppendLine($"Date: {DateTime.Now}");
        receipt.AppendLine("\n\nProducts");
        receipt.AppendLine("-------------------");

        foreach (var product in _shopCartService.ShowAllProductsInShopCart())
        {
            receipt.AppendLine($"{product.Name} - ${product.Price:F2}");
        }

        receipt.AppendLine("-------------------");
        receipt.AppendLine($"Original Price: ${_shopCartService.ShowPriceInShopCart():F2}");

        if (card != null)
        {
            receipt.AppendLine($"Discount Card: {card.Number}");
            receipt.AppendLine($"Discount Rate: {card.Discount}%");
            receipt.AppendLine($"Discount Amount: ${_shopCartService.ShowPriceInShopCart() - finalPrice:F2}");
            receipt.AppendLine($"Price After Discount: ${finalPrice:F2}");
        }

        receipt.AppendLine('\n' + $"\nPaid Amount: ${balance:F2}");
        receipt.AppendLine($"Change: ${balance - finalPrice:F2}");
        receipt.AppendLine("======== Thank You ========");

        return receipt.ToString();
    }

    public void SaveReceiptToPdf(string receiptContent)
    {
        string fileName = $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string filePath = Path.Combine(Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty, "Receipts"));

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        filePath = Path.Combine(filePath, fileName);

        _pdfReceiptService.GeneratePdfReceipt(receiptContent, filePath);
    }
}