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
        FoundDiscountCard = _discountCardController.searchDiscountCardByNumber(cardNumber);  
  
        if (FoundDiscountCard != null)  
        {  
            MessageBox.Show($"Card was found. Discount: {FoundDiscountCard.Discount}%");  
            this.DialogResult = true;  
        }  
        else  
        {  
            MessageBox.Show("Custom card. Discount: 1%");  
            this.DialogResult = false;  
        }  
        this.Close();  
    }  
  
    public decimal getDiscountByFoundCard() 
    {  
        return FoundDiscountCard != null ? FoundDiscountCard.Discount : 1;  
    }  
    
}