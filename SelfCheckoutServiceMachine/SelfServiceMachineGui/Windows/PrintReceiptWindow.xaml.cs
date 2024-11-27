using System.Windows;

namespace SelfServiceMachineGui;

public partial class PrintReceiptWindow : Window
{
    public bool PrintReceipt { get; private set; }

    public PrintReceiptWindow()
    {
        InitializeComponent();
    }

    private void YesButton_Click(object sender, RoutedEventArgs e)
    {
        PrintReceipt = true;
        DialogResult = true;
    }

    private void NoButton_Click(object sender, RoutedEventArgs e)
    {
        PrintReceipt = false;
        DialogResult = true;
    }
}