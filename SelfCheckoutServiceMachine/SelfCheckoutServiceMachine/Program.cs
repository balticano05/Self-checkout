// See https://aka.ms/new-console-template for more information


using System.Windows;
using SelfServiceMachineGui;

public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var app = new Application();
        var mainWindow = new MainWindow();
        app.Run(mainWindow);
    }
}
