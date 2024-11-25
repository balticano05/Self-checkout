using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;

namespace SelfCheckoutServiceMachine.Service;

public class ProductService
{
    private ProductRepository _productRepository;

    public ProductService()
    {
        _productRepository = new ProductRepository();
    }
    
    public List<Product> searchInCatalogByName(string name)
    {
        List<Product> _foundListProducts = new List<Product>();
        for (int i = 0; i < _productRepository.GetAll().Count; i++)
        {
            if (_productRepository.GetAll()[i].Name.Contains(name))
            {
                _foundListProducts.Add(_productRepository.GetAll()[i]);
            }
        }

        return _foundListProducts;
    }

    public List<Product> searchInCatalogByType(string type)
    {
        List<Product> _foundListProducts = new List<Product>();
        for (int i = 0; i <  _productRepository.GetAll().Count; i++)
        {
            if (_productRepository.GetAll()[i].Type.Equals(Enum.Parse<TypeProduct>(type)))
            {
                _foundListProducts.Add(_productRepository.GetAll()[i]);
            }
        }

        return _foundListProducts;
    }
    
}