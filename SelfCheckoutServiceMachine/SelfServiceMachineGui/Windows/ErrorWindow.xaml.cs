using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SelfServiceMachineGui;

public partial class ErrorWindow : Window
{
    private string imagePath = Path.Combine(
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
        "Resources",
        "error-icon.png"
    );

    public ErrorWindow(string errorMessage)
    {
        InitializeComponent();

        this.Width = 400;
        this.Height = 200;
        this.Title = "Error";
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.ResizeMode = ResizeMode.NoResize;

        var panel = new StackPanel
        {
            Margin = new Thickness(20)
        };

        var errorIcon = new Image
        {
            Source = new BitmapImage(new Uri(imagePath)),
            Width = 32,
            Height = 32,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var errorText = new TextBlock
        {
            Text = errorMessage,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 20),
            TextAlignment = TextAlignment.Center
        };

        var okButton = new Button
        {
            Content = "OK",
            Width = 80,
            Height = 25,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        okButton.Click += (s, e) => this.Close();

        panel.Children.Add(errorIcon);
        panel.Children.Add(errorText);
        panel.Children.Add(okButton);

        this.Content = panel;
    }
}