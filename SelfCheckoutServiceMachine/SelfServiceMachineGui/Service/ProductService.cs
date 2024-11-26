using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;

namespace SelfCheckoutServiceMachine.Service;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService()
    {
        _productRepository = new ProductRepository();
    }

    public List<Product> SearchInCatalogByName(string name)
    {
        return _productRepository.GetAll()
            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Product> SearchInCatalogByType(string type)
    {
        return _productRepository.GetAll()
            .Where(p => p.Type.ToString().Equals(type, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void UpdateProductStock(List<Product> products)
    {
        foreach (var product in products)
        {
            var dbProduct = _productRepository.Get(product.Id);
            if (dbProduct != null && dbProduct.QuantityInStock > 0)
            {
                dbProduct.QuantityInStock--;
                _productRepository.Update(dbProduct);
            }
        }
        _productRepository.Save();
    }

}