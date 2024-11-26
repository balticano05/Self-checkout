using System.Windows;

namespace SelfServiceMachineGui;

public partial class ReceiptWindow : Window
{
    public ReceiptWindow(string receiptText)
    {
        InitializeComponent();
        ReceiptText.Text = receiptText;
    }
}