using System.IO;
using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Mapper;
using SelfCheckoutServiceMachine.Utils;

namespace SelfCheckoutServiceMachine.Repository;

public class ProductRepository
{
    private readonly List<Product> _products;
    private readonly ProductMapper _productMapper;
    private readonly string _filePath;

    public ProductRepository()
    {
        _productMapper = new ProductMapper();
        _filePath = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty,
            "Resources",
            "Products.csv"
        );
        _products = _productMapper.MapStringListToProducts(CsvFileReader.ReadFile(_filePath));
    }

    public void Add(Product product)
    {
        _products.Add(product);
    }

    public Product? Get(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public List<Product> GetAll()
    {
        return _products.ToList();
    }

    public void Update(Product updatedProduct)
    {
        var productIndex = _products.FindIndex(p => p.Id == updatedProduct.Id);
        if (productIndex != -1)
        {
            _products[productIndex] = updatedProduct;
        }
    }

    public void Save()
    {
        CsvFileWriter.WriteFile(_filePath, _productMapper.MapProductsToStringList(_products));
    }
}