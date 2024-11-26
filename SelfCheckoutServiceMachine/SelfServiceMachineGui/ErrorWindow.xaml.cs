using System.Windows;
using System.Windows.Controls;

namespace SelfServiceMachineGui;

public partial class ErrorWindow : Window
{
    public ErrorWindow(string errorMessage)
    {
        InitializeComponent();
        this.Width = 400;
        this.Height = 200;
        this.Title = "Error";
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        StackPanel panel = new StackPanel
        {
            Margin = new Thickness(10)
        };

        TextBlock errorText = new TextBlock
        {
            Text = errorMessage,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 10, 0, 20)
        };

        Button okButton = new Button
        {
            Content = "OK",
            Width = 70,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        okButton.Click += (s, e) => this.Close();

        panel.Children.Add(errorText);
        panel.Children.Add(okButton);
        this.Content = panel;
    }
}