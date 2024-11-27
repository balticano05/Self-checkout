using System.Text.RegularExpressions;
using System.Windows;
using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Service;

namespace SelfServiceMachineGui;

public partial class DiscountCardWindow : Window
{
    private readonly DiscountCardService _discountCardService;
    public DiscountCard FoundDiscountCard { get; private set; }

    public DiscountCardWindow()
    {
        InitializeComponent();
        _discountCardService = new DiscountCardService();
        FoundDiscountCard = new DiscountCard();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        string cardNumber = CardNumberTextBox.Text;

        if (string.IsNullOrEmpty(cardNumber))
        {
            DialogResult = false;
            Close();
            return;
        }

        if (!Regex.IsMatch(cardNumber, @"^\d{4}$"))
        {
            MessageBox.Show("Invalid card format. Card number must be exactly 4 digits.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        FoundDiscountCard = _discountCardService.SearchDiscountCardByNumber(cardNumber);

        if (FoundDiscountCard != null)
        {
            MessageBox.Show($"Card found. Discount: {FoundDiscountCard.Discount}%");
            DialogResult = true;
        }
        else
        {
            FoundDiscountCard = _discountCardService.CreateNewDiscountCard(cardNumber);
            MessageBox.Show("New card created. Discount: 1%");
            DialogResult = true;
        }

        Close();
    }

    public decimal GetDiscountByFoundCard()
    {
        return FoundDiscountCard?.Discount ?? 1;
    }
}