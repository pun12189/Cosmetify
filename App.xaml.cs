using System.Configuration;
using System.Data;
using System.Windows;

namespace BahiKitaab
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App() {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Helper.Helper.BugReport(e.Exception);
            }
            catch (Exception ex)
            {
                Helper.Helper.LogError(ex);
            }
        }
    }

}
