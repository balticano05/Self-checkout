// See https://aka.ms/new-console-template for more information

using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;
using SelfCheckoutServiceMachine.Utils;

public class Program
{
    static void Main(string[] args)
    {
        // CsvFileReader csvFileReader = new CsvFileReader();
        // // List<string> dataProducts = CsvFileReader.ReadFile(Path.Combine("Resources", "Products.csv"));
        // // List<string> dataDiscountCard = CsvFileReader.ReadFile(Path.Combine("Resources", "DiscountCard.csv"));
        // ProductRepository productRepository = new ProductRepository();
        // List<Product> products = productRepository.getAll();
        // Product firstProduct = products[0];
        // Console.WriteLine(firstProduct.Name);

        ProductRepository productRepository = new ProductRepository();
        DiscountCardRepository discountCardRepository = new DiscountCardRepository();
        
        Product testProduct =  new Product.Builder()
            .SetId(44)
            .SetName("Test")
            .SetPrice(5.55m)
            .SetDescription("Test descr")
            .SetWholesalePrice(10.00m)
            .SetTypeProduct(TypeProduct.Alcohol)
            .SetWholesaleProduct(true)
            .SetQuantityInStock(10)
            .Build();
        
    }
}