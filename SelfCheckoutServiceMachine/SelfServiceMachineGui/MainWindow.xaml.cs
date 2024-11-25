using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SelfCheckoutServiceMachine.Controller;
using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;
using SelfCheckoutServiceMachine.Service;

namespace SelfServiceMachineGui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ProductController _productController;
    private ShopCartController _shopCartController;
    
    public MainWindow()
    {
        InitializeComponent();
        _productController = new ProductController();
        _shopCartController = new ShopCartController();
    }

    private void SearchByName_Click(object sender, RoutedEventArgs e)
    {
        string name = NameSearchBox.Text;
        List<Product> results = _productController.searchByName(name);
        UpdateResultsListBox(results);
    }

    private void SearchByType_Click(object sender, RoutedEventArgs e)
    {
        if (TypeComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string type = selectedItem.Content.ToString();
            List<Product> results = _productController.searchByType(type);
            UpdateResultsListBox(results);
        }
    }

    private void UpdateResultsListBox(List<Product> products)
    {
        ResultsListBox.Items.Clear();
    
        if (products.Count == 0)
        {
            TextBlock notFoundText = new TextBlock
            {
                Text = "Not found such product",
                Foreground = Brushes.Teal
            };
            ResultsListBox.Items.Add(notFoundText);
        }
        else
        {
            foreach (var product in products)
            {
                ResultsListBox.Items.Add($"{product.Name} - {product.Type}");
            }
        }
    }
    
    private void NameSearchBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (NameSearchBox.Text == "Enter name's product")
        {
            NameSearchBox.Text = "";
            NameSearchBox.Foreground = Brushes.Black;
        }
    }

    private void NameSearchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameSearchBox.Text))
        {
            NameSearchBox.Text = "Enter name's product";
            NameSearchBox.Foreground = Brushes.Gray;
        }
    }
    
    private void AddToCart_Click(object sender, RoutedEventArgs e)
    {
        if (ResultsListBox.SelectedItem is string selectedProductString)
        {
            var productDetails = selectedProductString.Split(" - ");
            string productName = productDetails[0];
            
            Product selectedProduct = _productController.searchByName(productName).First();
            if (selectedProduct != null)
            {
                _shopCartController.putIn(selectedProduct);
                UpdateCartListBox();
                UpdateTotalPrice();
            }
            
        }

    }
    
    private void UpdateTotalPrice()
    {
        decimal totalPrice = _shopCartController.ShowPriceInShopCart();
        MessageBox.Show($"Total Price: {totalPrice}", "Cart Total");
    }
    
    private void UpdateCartListBox()
    {
        CartListBox.Items.Clear();
        List<Product> cartProducts = _shopCartController.ShowAllProductsInShopCart();

        foreach (var product in cartProducts)
        {
            CartListBox.Items.Add($"{product.Name} - {product.Type}");
        }
    }

    private void Buy_Click(object sender, RoutedEventArgs e)
    {
        ShopCart purchasableCart = _shopCartController.GetPurchasableProductsInShoppingCart();
        MessageBox.Show($"Total Price: {purchasableCart.TotalPrice}", "Purchase Complete");
        UpdateCartListBox();
    }
    
}