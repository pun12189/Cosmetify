using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BahiKitaab.Helper
{
    public class WindowManager
    {
        public static Window ChangeWindowContent(Window window, object viewModel, string title, string controlPath)
        {
            window.Title = title;
            window.Background = Brushes.White;
            window.Foreground = Brushes.Black;
            window.WindowState = WindowState.Maximized;
            window.ResizeMode = ResizeMode.CanMinimize;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/BahiKitaab;component/Resources/Icon.ico", UriKind.RelativeOrAbsolute));

            var controlAssembly = Assembly.Load("BahiKitaab");
            var controlType = controlAssembly.GetType(controlPath);
            var newControl = Activator.CreateInstance(controlType) as UserControl;
            newControl.DataContext = viewModel;
            window.Content = newControl;

            return window;
        }
  }
}
