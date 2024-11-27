using System.Windows;

namespace SelfServiceMachineGui.Handler;

public class GlobalExceptionHandler
{
    private static Action<Exception> _showError;

    public static void Initialize(Action<Exception> showError)
    {
        _showError = showError;
        Application.Current.DispatcherUnhandledException += (s, e) =>
        {
            HandleExcpetion(e.Exception);
            e.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            HandleExcpetion(e.ExceptionObject as Exception);
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            HandleExcpetion(e.Exception);
            e.SetObserved();
        };
    }

    public static void HandleExcpetion(Exception ex)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _showError?.Invoke(ex);
        });
    }
    
} 