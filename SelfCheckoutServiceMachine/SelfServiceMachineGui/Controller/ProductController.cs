using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;
using SelfCheckoutServiceMachine.Service;

namespace SelfCheckoutServiceMachine.Controller;

public class ProductController
{
    private ProductService _productService;

    public ProductController()
    {
        _productService = new ProductService();
    }

    public List<Product> searchByType(string type)
    {
        return _productService.searchInCatalogByType(type);
    }
    
    public List<Product> searchByName(string name)
    {
        return _productService.searchInCatalogByName(name);
    }
}