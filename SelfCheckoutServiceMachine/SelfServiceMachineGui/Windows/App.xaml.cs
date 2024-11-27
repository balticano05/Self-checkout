using System.Windows;
using SelfServiceMachineGui.Handler;

namespace SelfServiceMachineGui;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        GlobalExceptionHandler.Initialize(ex =>
        {
            var errorWindow = new ErrorWindow(ex.Message);
            errorWindow.ShowDialog();
        });
        
    }
    
}