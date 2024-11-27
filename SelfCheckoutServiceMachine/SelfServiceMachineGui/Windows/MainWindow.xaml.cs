using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Service;

namespace SelfServiceMachineGui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SelfCheckoutService _selfCheckoutService;

    public MainWindow()
    {
        InitializeComponent();
        _selfCheckoutService = new SelfCheckoutService();
    }

    private void SearchByName_Click(object sender, RoutedEventArgs e)
    {
        string name = NameSearchBox.Text;
        List<Product> results = _selfCheckoutService.SearchProducts(name);
        UpdateResultsListBox(results);
    }

    private void SearchByType_Click(object sender, RoutedEventArgs e)
    {
        if (TypeComboBox.SelectedItem is ComboBoxItem selectedItem &&
            selectedItem.Content.ToString() != "Select a product type")
        {
            string type = selectedItem.Content.ToString();
            List<Product> results = _selfCheckoutService.SearchProductsByType(type);
            UpdateResultsListBox(results);
        }
        else
        {
            ShowNotFoundMessage();
        }
    }

    private void UpdateResultsListBox(List<Product> products)
    {
        ResultsListBox.Items.Clear();
        if (!products.Any())
        {
            ShowNotFoundMessage();
            return;
        }

        var cartProducts = _selfCheckoutService.GetCartProducts();
        foreach (var product in products)
        {
            if (product.QuantityInStock <= 0)
            {
                ResultsListBox.Items.Add(new TextBlock
                {
                    Text = $"{product.Name} - Out of Stock",
                    Foreground = Brushes.Red
                });
            }
            else
            {
                int quantityInCart = cartProducts.Count(p => p.Id == product.Id);
                int remainingStock = product.QuantityInStock - quantityInCart;

                if (remainingStock <= 0)
                {
                    ResultsListBox.Items.Add(new TextBlock
                    {
                        Text = $"{product.Name} - No more items available",
                        Foreground = Brushes.Red
                    });
                }
                else
                {
                    ResultsListBox.Items.Add(
                        $"{product.Price} - {product.Name} - {product.Type} (Available: {remainingStock})");
                }
            }
        }
    }

    private void ShowNotFoundMessage()
    {
        ResultsListBox.Items.Clear();
        TextBlock notFoundText = new TextBlock
        {
            Text = "Not found such product",
            Foreground = Brushes.Teal
        };
        ResultsListBox.Items.Add(notFoundText);
    }

    private void NameSearchBoxGotFocus(object sender, RoutedEventArgs e)
    {
        if (NameSearchBox.Text == "Enter name's product")
        {
            NameSearchBox.Text = "";
            NameSearchBox.Foreground = Brushes.Black;
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

    private void SearchByTypeClick(object sender, RoutedEventArgs e)
    {
        if (TypeComboBox.SelectedItem is ComboBoxItem selectedItem &&
            selectedItem.Content.ToString() != "Select a product type")
        {
            string type = selectedItem.Content.ToString();
            List<Product> results = _selfCheckoutService.SearchProductsByType(type);
            UpdateResultsListBox(results);
        }
        else
        {
            ShowNotFoundMessage();
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
        string pattern = @"^\d{4}$";
        try
        {
            if (ResultsListBox.SelectedItem is string selectedProductString)
            {
                var productDetails = selectedProductString.Split(" - ");
                string productName = productDetails[1];
                Product selectedProduct = _selfCheckoutService.SearchProducts(productName).First();

                if (_selfCheckoutService.AddProductToCart(selectedProduct))
                {
                    UpdateCartListBox();
                    UpdateTotalPrice();
                }
                else
                {
                    new ErrorWindow("Cannot add product to cart - out of stock or limit reached!").ShowDialog();
                }
            }
        }
        catch (Exception ex)
        {
            new ErrorWindow($"Error adding product to cart: {ex.Message}").ShowDialog();
        }
    }

    private void UpdateTotalPrice()
    {
        decimal totalPrice = _selfCheckoutService.GetTotalPrice();
        TotalPriceTextBlock.Text = $"Total Price: ${totalPrice:F2}";
    }

    private void UpdateCartListBox()
    {
        CartListBox.Items.Clear();
        var cartProducts = _selfCheckoutService.GetCartProducts();
        foreach (var product in cartProducts)
        {
            CartListBox.Items.Add($"{product.Price} - {product.Name} - {product.Type}");
        }
    }

    private void Buy_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            DiscountCard discountCard = null;
            var result = MessageBox.Show("Would you like to use a discount card?", "Discount Card",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var discountCardWindow = new DiscountCardWindow();
                if (discountCardWindow.ShowDialog() == true)
                {
                    discountCard = discountCardWindow.FoundDiscountCard;
                }
            }

            var purchaseResult = _selfCheckoutService.ProcessPurchase(discountCard);
            if (!purchaseResult.IsSuccess)
            {
                new ErrorWindow(purchaseResult.Message).ShowDialog();
                return;
            }

            ProcessBalanceAndPurchase(purchaseResult.FinalPrice, discountCard);
        }
        catch (Exception ex)
        {
            new ErrorWindow($"Error processing purchase: {ex.Message}").ShowDialog();
        }
    }

    private void ProcessBalanceAndPurchase(decimal finalPrice, DiscountCard card)
    {
        while (true)
        {
            decimal balance = ShowBalanceInputDialog();
            if (_selfCheckoutService.CompletePurchase(balance, finalPrice))
            {
                if (ProcessPurchase(finalPrice, balance, card))
                {
                    UpdateCartListBox();
                    UpdateTotalPrice();
                }

                break;
            }

            var result = MessageBox.Show(
                "Insufficient funds! Would you like to try again?",
                "Error",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                _selfCheckoutService.ClearCart();
                UpdateCartListBox();
                UpdateTotalPrice();
                break;
            }
        }
    }

    private bool ProcessPurchase(decimal finalPrice, decimal balance, DiscountCard card)
    {
        var printReceiptWindow = new PrintReceiptWindow();
        if (printReceiptWindow.ShowDialog() == true && printReceiptWindow.PrintReceipt)
        {
            var receipt = _selfCheckoutService.GenerateReceipt(finalPrice, balance, card);

            try
            {
                _selfCheckoutService.SaveReceiptToPdf(receipt);
                MessageBox.Show("Receipt has been saved to PDF!", "Success");

                var receiptWindow = new ReceiptWindow(receipt);
                receiptWindow.Show();
                MessageBox.Show($"Final price: {finalPrice:F2}", "Purchase completed");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving receipt to PDF: {ex.Message}", "Error");
                return false;
            }
        }

        return false;
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

        StackPanel panel = new StackPanel { Margin = new Thickness(10) };
        TextBox balanceInput = new TextBox { Margin = new Thickness(0, 5, 0, 5) };
        Button okButton = new Button { Content = "OK", Width = 70 };

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
        _selfCheckoutService.ClearCart();
        UpdateCartListBox();
        UpdateTotalPrice();
    }
}