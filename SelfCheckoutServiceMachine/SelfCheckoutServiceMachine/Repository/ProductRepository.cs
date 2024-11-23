using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Mapper;
using SelfCheckoutServiceMachine.Resources;
using SelfCheckoutServiceMachine.Utils;

namespace SelfCheckoutServiceMachine.Repository;

public class ProductRepository
{
    private List<Product> _products;
    private ProductMapper _productMapper;
    private string filePath;

    public ProductRepository()
    {
        _productMapper = new ProductMapper();
        filePath = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, 
            "Resources", 
            "Products.csv"
        );
        _products = _productMapper.MapStringListToProducts(CsvFileReader.ReadFile(filePath));
    }

    public void Add(Product product)
    {
        _products.Add(product);
    }

    public Product Get(int id) 
    {
        foreach (var product in _products)
        {
            if (product.Id.Equals(id))
            {
                return product;
            }
        }
        return null;
    }

    public List<Product> GetAll() 
    {
        return _products;
    }

    public void Update(Product updatedProduct)
    {
        for (int i = 0; i < _products.Count; i++)
        {
            if (_products[i].Id.Equals(updatedProduct.Id))
            {
                _products[i] = updatedProduct;
                break;
            }
        }
    }

    public void Save()
    {
        CsvFileWriter.WriteFile(filePath, _productMapper.MapProductsToStringList(_products));    
    }
    
}