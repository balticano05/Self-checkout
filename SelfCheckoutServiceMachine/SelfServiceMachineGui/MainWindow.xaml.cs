using System.Text;
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
        if (TypeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content.ToString() != "Select a product type")
        {
            string type = selectedItem.Content.ToString();
            List<Product> results = _productController.searchByType(type);
            UpdateResultsListBox(results);
        }
        else
        {
            ResultsListBox.Items.Clear();
            TextBlock notFoundText = new TextBlock
            {
                Text = "Not found such product",
                Foreground = Brushes.Teal
            };
            ResultsListBox.Items.Add(notFoundText);
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
                ResultsListBox.Items.Add($"{product.Price} - {product.Name} - {product.Type} ");
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
            string productName = productDetails[1];
            
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
        TotalPriceTextBlock.Text = $"Total Price: ${totalPrice:F2}";
    }
    
    private void UpdateCartListBox()
    {
        CartListBox.Items.Clear();
        List<Product> cartProducts = _shopCartController.ShowAllProductsInShopCart();

        foreach (var product in cartProducts)
        {
            CartListBox.Items.Add($"{product.Price} - {product.Name} - {product.Type} ");
        }
    }

    private void Buy_Click(object sender, RoutedEventArgs e)
    {
        ShopCart purchasableCart = _shopCartController.GetPurchasableProductsInShoppingCart();
        if (purchasableCart.Products.Count == 0)
        {
            MessageBox.Show("Cart is empty!");
            return;
        }

        decimal finalPrice = purchasableCart.TotalPrice;
        DiscountCardWindow discountCardWindow = new DiscountCardWindow();
        bool? result = discountCardWindow.ShowDialog();

        if (result == true)
        {
            decimal discount = discountCardWindow.getDiscountByFoundCard();
            finalPrice = purchasableCart.TotalPrice - (purchasableCart.TotalPrice * discount / 100);
        }

        ProcessBalanceAndPurchase(finalPrice, discountCardWindow.FoundDiscountCard);
    }

    private void ProcessBalanceAndPurchase(decimal finalPrice, DiscountCard card)
    {
        while (true)
        {
            decimal balance = ShowBalanceInputDialog();
            if (balance >= finalPrice)
            {
                if (card != null)
                {
                    card.Discount = balance - finalPrice;
                }
                ProcessPurchase(finalPrice, balance, card);
                break;
            }
            else
            {
                var result = MessageBox.Show(
                    "Insufficient funds! Would you like to try again?",
                    "Error",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    _shopCartController.clearShopCart();
                    UpdateCartListBox();
                    UpdateTotalPrice();
                    break;
                }
            }
        }
    }

    private void ProcessPurchase(decimal finalPrice, decimal balance, DiscountCard card)
    {
        var printReceiptWindow = new PrintReceiptWindow();
        if (printReceiptWindow.ShowDialog() == true && printReceiptWindow.PrintReceipt)
        {
            StringBuilder receipt = new StringBuilder();
            receipt.AppendLine("=== RECEIPT ===");
            receipt.AppendLine($"Date: {DateTime.Now}");
            receipt.AppendLine("Products:");
            receipt.AppendLine("-------------------");

            foreach (var product in _shopCartController.ShowAllProductsInShopCart())
            {
                receipt.AppendLine($"{product.Name} - ${product.Price:F2}");
            }

            receipt.AppendLine("-------------------");
            receipt.AppendLine($"Total Price: ${_shopCartController.ShowPriceInShopCart():F2}");
        
            if (card != null)
            {
                receipt.AppendLine($"Discount Card Applied: {card.Discount}%");
                receipt.AppendLine($"Final Price: ${finalPrice:F2}");
            }

            receipt.AppendLine($"Paid Amount: ${balance:F2}");
            receipt.AppendLine($"Change: ${balance - finalPrice:F2}");
            receipt.AppendLine("=== Thank You ===");

            var receiptWindow = new ReceiptWindow(receipt.ToString());
            receiptWindow.Show();
        }

        MessageBox.Show($"Final price: {finalPrice:F2}", "Purchase completed");
        _shopCartController.clearShopCart();
        UpdateCartListBox();
        UpdateTotalPrice();
    }

private decimal ShowBalanceInputDialog()
{
    Window balanceWindow = new Window
    {
        Title = "Enter balance",
        Width = 300,
        Height = 150,
        WindowStartupLocation = WindowStartupLocation.CenterScreen
    };

    StackPanel panel = new StackPanel
    {
        Margin = new Thickness(10)
    };

    TextBox balanceInput = new TextBox
    {
        Margin = new Thickness(0, 5, 0, 5)
    };

    Button okButton = new Button
    {
        Content = "OK",
        Width = 70
    };

    decimal balance = 0;
    okButton.Click += (s, e) =>
    {
        if (decimal.TryParse(balanceInput.Text, out balance))
        {
            balanceWindow.DialogResult = true;
        }
        else
        {
            MessageBox.Show("Enter a valid amount!");
        }
    };

    panel.Children.Add(new TextBlock { Text = "Enter amount:" });
    panel.Children.Add(balanceInput);
    panel.Children.Add(okButton);
    balanceWindow.Content = panel;
    balanceWindow.ShowDialog();

    return balance;
}
    
    private void ClearCart_Click(object sender, RoutedEventArgs e)
    {
        _shopCartController.clearShopCart();
        UpdateCartListBox();
        UpdateTotalPrice();
    }
    
}