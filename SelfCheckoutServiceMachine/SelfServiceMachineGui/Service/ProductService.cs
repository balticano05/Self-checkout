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

    public List<Product> SearchInCatalogByName(string name) =>
        _productRepository.GetAll()
            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

    public List<Product> SearchInCatalogByType(string type) =>
        _productRepository.GetAll()
            .Where(p => p.Type.ToString().Equals(type, StringComparison.OrdinalIgnoreCase))
            .ToList();

    public void UpdateProductStock(List<Product> products)
    {
        var productsToUpdate = _productRepository.GetAll()
            .Join(products,
                dbProduct => dbProduct.Id,
                product => product.Id,
                (dbProduct, _) => dbProduct)
            .Where(p => p.QuantityInStock > 0)
            .Select(p =>
            {
                p.QuantityInStock--;
                return p;
            })
            .ToList();

        productsToUpdate.ForEach(p => _productRepository.Update(p));
        _productRepository.Save();
    }
}