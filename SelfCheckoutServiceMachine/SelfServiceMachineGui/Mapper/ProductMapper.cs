using System.Globalization;
using SelfCheckoutServiceMachine.Models;

namespace SelfCheckoutServiceMachine.Mapper;

public class ProductMapper : CsvMapper<Product>
{
    public List<Product> MapStringListToProducts(List<string> lineProducts)
    {
        return MapStringListToObjects(lineProducts, parts =>
        {
            if (parts.Length != 8)
            {
                throw new FormatException("Invalid number of parts in line.");
            }
            
            if (!Enum.TryParse(parts[5], out TypeProduct typeProduct))
            {
                throw new ArgumentException($"Invalid TypeProduct value: {parts[5]}");
            }

            return new Product.Builder()
                .SetId(int.Parse(parts[0]))
                .SetName(parts[1])
                .SetPrice(decimal.Parse(parts[2], CultureInfo.InvariantCulture))
                .SetDescription(parts[3])
                .SetWholesalePrice(decimal.Parse(parts[4], CultureInfo.InvariantCulture))
                .SetTypeProduct(Enum.Parse<TypeProduct>(parts[5]))
                .SetWholesaleProduct(bool.Parse(parts[6]))
                .SetQuantityInStock(int.Parse(parts[7]))
                .Build();
        });
    }

    public List<string> MapProductsToStringList(List<Product> products)
    {
        var header = "Id;Name;Price;Description;WholesalePrice;TypeProduct;WholesaleProduct;QuantityInStock";
        var lines = new List<string> { header };

        lines.AddRange(MapObjectsToStringList(products, product =>
            $"{product.Id};{product.Name};{product.Price.ToString(CultureInfo.InvariantCulture)};" +
            $"{product.Description};{product.WholesalePrice.ToString(CultureInfo.InvariantCulture)};" +
            $"{product.Type};{product.WholesaleProduct};{product.QuantityInStock}"
        ));

        return lines;
    }
    
}