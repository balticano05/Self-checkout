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
        for (int i = 0; i < _productRepository.GetAll().Count; i++)
        {
            if (_productRepository.GetAll()[i].Type.Equals(Enum.Parse<TypeProduct>(type)))
            {
                _foundListProducts.Add(_productRepository.GetAll()[i]);
            }
        }

        return _foundListProducts;
    }

    public void handleProductBuying(ShopCart shopCart, DiscountCard discountCard, int sumOfMoney) 
    {
        for (int i = 0; i < shopCart.Products.Count; i++) 
        {
            bool isFound = false;
            int j = 0;
        
            for (; j < _productRepository.GetAll().Count; j++) 
            {
                if (shopCart.Products[i].Name == _productRepository.GetAll()[j].Name) 
                {
                    isFound = true;
                    break;
                }
            }

            if (!isFound) 
            {
                Console.WriteLine($"Product {shopCart.Products[i].Name} cannot be sold - not found in database.");
                shopCart.TotalPrice -= shopCart.Products[i].Price;
                shopCart.Products.RemoveAt(i);
                i--; 
                continue;
            }

            var productInDb = _productRepository.GetAll()[j];
            if (productInDb.QuantityInStock <= 0)
            {
                shopCart.TotalPrice -= shopCart.Products[i].Price;
                Console.WriteLine($"Product {shopCart.Products[i].Name} cannot be sold - out of stock.");
                shopCart.Products.RemoveAt(i);
                i--;
                continue;
            }

            Product updatedProduct = _productRepository.GetAll()[j];
            updatedProduct.QuantityInStock -= 1;
            _productRepository.Update(updatedProduct);

            if (discountCard != null)
            {
                shopCart.TotalPrice = shopCart.TotalPrice - (shopCart.TotalPrice * discountCard.Discount) / 100;
            }

            if (sumOfMoney >= shopCart.TotalPrice)
            {
                Console.WriteLine("The purchase was successful");
                _productRepository.Save();
            }
            else
            {
                Console.WriteLine("Not enough money.");
            }
            
        }
    }

}