using Cosmetify.Model;
using Cosmetify.ViewModel;
using MahApps.Metro.Controls;
using System.Windows;

namespace Cosmetify.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this);
        }
    }
}
