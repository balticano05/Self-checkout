using System.IO;

namespace SelfCheckoutServiceMachine.Resources;

public class AppSettings
{
    public static string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    public static string resourcesPath = Path.Combine(projectDirectory, "Resources/Products.csv");
    static string productsFilePath = Path.Combine(resourcesPath, "Products.csv");
    static string discountCardFilePath = Path.Combine(resourcesPath, "DiscountCard.csv");
}