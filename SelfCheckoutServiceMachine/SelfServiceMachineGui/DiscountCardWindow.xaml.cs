using System.Windows;
using SelfCheckoutServiceMachine.Controller;
using SelfCheckoutServiceMachine.Models;

namespace SelfServiceMachineGui;

public partial class DiscountCardWindow : Window
{

    private DiscountCardController _discountCardController;  
    public DiscountCard FoundDiscountCard { get; private set; }  
  
    public DiscountCardWindow()  
    {  
        InitializeComponent();  
        FoundDiscountCard = new DiscountCard();  
        _discountCardController = new DiscountCardController();  
    }  
  
    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        string cardNumber = CardNumberTextBox.Text;

        if (string.IsNullOrEmpty(cardNumber))
        {
            this.DialogResult = false;
            this.Close();
            return;
        }

        FoundDiscountCard = _discountCardController.searchDiscountCardByNumber(cardNumber);

        if (FoundDiscountCard != null)
        {
            MessageBox.Show($"Card found. Discount: {FoundDiscountCard.Discount}%");
            this.DialogResult = true;
        }
        else
        {
            FoundDiscountCard = new DiscountCard.Builder()
                .SetDiscount(1)
                .SetNumber(cardNumber)
                .Build();
            MessageBox.Show("New card created. Discount: 1%");
            this.DialogResult = true;
        }

        this.Close();
    }
  
    public decimal getDiscountByFoundCard() 
    {  
        return FoundDiscountCard != null ? FoundDiscountCard.Discount : 1;  
    }  
    
}